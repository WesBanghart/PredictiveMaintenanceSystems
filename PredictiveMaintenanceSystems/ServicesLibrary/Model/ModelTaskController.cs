using EFDataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Newtonsoft.Json.Linq;
using ServicesLibrary.Model.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model
{
    /// <summary>
    /// The class that controls the operation of model-related tasks.
    /// </summary>
    public class ModelTaskController
    {
        /// <summary>
        /// The context class for the target database.
        /// </summary>
        private EFSystemContext _context;

        /// <summary>
        /// The JSON string representing the configuration of the model.
        /// </summary>
        public string ModelJson { get; set; }
        /// <summary>
        /// The ID of the data source from which training data should be received.
        /// </summary>
        Guid TrainDataSourceId { get; set; }
        /// <summary>
        /// The ID of the data source from which testing data should be received. Optional.
        /// </summary>
        Guid TestDataSourceId { get; set; }
        /// <summary>
        /// The MLContext object representing the ML.NET configuration for the model.
        /// </summary>
        public MLContext MLContext { get; set; }
        /// <summary>
        /// The IDataView object representing the training data.
        /// </summary>
        public IDataView TrainDataView { get; set; }
        /// <summary>
        /// The IDataView object representing the testing data.
        /// </summary>
        public IDataView TestDataView { get; set; }
        /// <summary>
        /// The transformer representing the ML.NET data pipeline for the model.
        /// </summary>
        public ITransformer Transformer { get; set; }
        /// <summary>
        /// The IDataView object representing the results received from running the data pipeline.
        /// </summary>
        public IDataView Results { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context class for the target database.</param>
        public ModelTaskController(EFSystemContext context)
        {
            _context = context;
        }


        //------------- PUBLIC OPERATIONS ----------------------------------------------------
        /// <summary>
        /// Task to update and run a model.
        /// </summary>
        /// <param name="ModelId">The model ID relating to the database entry for the model to be run.</param>
        /// <param name="cancellationToken">The async cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public async Task<bool> RunModel(Guid ModelId, CancellationToken cancellationToken)
        {
            try
            {
                if (_RetrieveModel(ModelId))
                {
                    if (_ParseModelJson())
                    {
                        if(TestDataView == null) Results = Transformer.Transform(TrainDataView); 
                        else Results = Transformer.Transform(TestDataView);
                        byte[] modelZip = _SaveModel(ModelId);
                        var modelEntry = await _context.FindAsync<EFDataModels.ModelTable>(ModelId, cancellationToken);
                        modelEntry.File = modelZip;
                        modelEntry.LastUpdated = DateTime.Now;
                        _context.Entry(modelEntry).State = EntityState.Modified;

                        try
                        {
                            await _context.SaveChangesAsync(cancellationToken);
                            return true;
                        }
                        catch (Exception)
                        {
                            //TODO: log exception
                            return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                //TODO: log exception
                return false;
            }
        }

        /// <summary>
        /// Task to update a model.
        /// </summary>
        /// <param name="ModelId">The model ID relating to the database entry for the model to be updated.</param>
        /// <param name="cancellationToken">The async cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public async Task<bool> UpdateModel(Guid ModelId, CancellationToken cancellationToken)
        {
            try
            {
                if (_RetrieveModel(ModelId))
                {
                    if (_ParseModelJson())
                    {
                        byte[] modelZip = _SaveModel(ModelId);
                        var modelEntry = await _context.FindAsync<EFDataModels.ModelTable>(ModelId, cancellationToken);
                        modelEntry.File = modelZip;
                        modelEntry.LastUpdated = DateTime.Now;
                        _context.Entry(modelEntry).State = EntityState.Modified;

                        try
                        {
                            await _context.SaveChangesAsync(cancellationToken);
                            return true;
                        }
                        catch (Exception)
                        {
                            //TODO: log exception
                            return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                //TODO: log exception
                return false;
            }
        }


        //--------------- PRIVATE DATABASE ACCESSORS -----------------------------------------
        /// <summary>
        /// Command to retrieve the model from the database.
        /// </summary>
        /// <param name="ModelId">The model ID relating to the database entry for the model to be updated.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool _RetrieveModel(Guid ModelId)
        {
            var modelEntry = _context.Find<EFDataModels.ModelTable>(ModelId);

            if (modelEntry == null) return false;

            ModelJson = modelEntry.Configuration;

            return true;
        }

        /// <summary>
        /// Command to create the zip file of the model and results.
        /// </summary>
        /// <param name="ModelId">The model ID relating to the database entry for the model to be updated.</param>
        /// <returns>A byte array representing the zip file of the model and results.</returns>
        private byte[] _SaveModel(Guid ModelId)
        {
            string tempZip = $"temp/{ModelId}_temp.zip";
            string tempDir = $"temp/{ModelId}_temp/";

            MLContext.Model.Save(Transformer, TrainDataView.Schema, tempZip);

            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            Directory.CreateDirectory(tempDir);
            ZipFile.ExtractToDirectory(tempZip, tempDir);

            if (Results != null)
            {
                using (FileStream fileStream = new FileStream($"{tempDir}results.tsv", FileMode.Create))
                {
                    MLContext.Data.SaveAsText(Results, fileStream);
                }
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (string file in Directory.EnumerateFiles(tempDir))
                    {
                        zipArchive.CreateEntryFromFile($"{tempDir}{file}", file);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        //--------------- PRIVATE PARSE FUNCTIONS --------------------------------------------
        /// <summary>
        /// Command to parse the model JSON and populate the MLContext configuration
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        private bool _ParseModelJson()
        {
            JObject modelObject = JObject.Parse(ModelJson);
            
            //parse training data source id
            TrainDataSourceId = Guid.Parse((string)modelObject["Data"]["TrainSetSource"]["DataSourceId"]);
            var trainSourceEntry = _context.Find<EFDataModels.DataSourceTable>(TrainDataSourceId);
            if (trainSourceEntry == null) return false;
            string trainSourceConfig = trainSourceEntry.Configuration;
            JToken trainSourceJson = JToken.Parse(trainSourceConfig);
            if (!_ParseDataSourceJson(ref trainSourceEntry, trainSourceJson, false)) return false;
            
            //parse testing data source id
            if (modelObject.TryGetValue("TestSetSource", out JToken token))
            {
                TestDataSourceId = Guid.Parse((string)token["DataSourceId"]);
                var testSourceEntry = _context.Find<EFDataModels.DataSourceTable>(TrainDataSourceId);
                if (testSourceEntry == null) return false;
                string testSourceConfig = testSourceEntry.Configuration;
                JToken testSourceJson = JToken.Parse(testSourceConfig);
                _ParseDataSourceJson(ref testSourceEntry, testSourceJson, true);
            }
            else
            {
                TestDataView = TrainDataView;
            }

            //parse model configuration
            string mlContextSeed = (string)modelObject["Model"]["MLContext"]["Seed"];
            
            if (int.TryParse(mlContextSeed, out int seed)) MLContext = new MLContext(seed);
            else MLContext = new MLContext();

            EstimatorChain<ITransformer> pipeline = new EstimatorChain<ITransformer>();

            foreach (JToken component in modelObject["Model"]["ComponentList"].Children())
            {
                string modelType = component.Value<string>("ComponentType");
                JToken componentObject = component["Options"];

                if (String.IsNullOrEmpty(modelType)) return false;

                if (!_ParseComponentJson(ref pipeline, modelType, componentObject)) return false;
            }

            Transformer = pipeline.Fit(TrainDataView);

            return true;
        }

        /// <summary>
        /// Command to parse the data source from the model JSON and populate the train and test DataViews.
        /// </summary>
        /// <param name="dataSource">Reference to the data sources database table.</param>
        /// <param name="dataSourceJson">The data source JSON configuration to parse.</param>
        /// <param name="isTestData">True if this is the TestDataView, false if the TrainDataView.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool _ParseDataSourceJson(ref DataSourceTable dataSource, JToken dataSourceJson, bool isTestData)
        {
            string sourceType = dataSourceJson.Value<string>("Type");

            switch (sourceType)
            {
                case "Binary":
                    if (isTestData) TestDataView = MLContext._LoadFromBinary(dataSourceJson);
                    else TrainDataView = MLContext._LoadFromBinary(dataSourceJson);
                    return true;
                
                case "Database":
                    if (isTestData) TestDataView = MLContext._LoadFromDatabase(dataSourceJson);
                    else TrainDataView = MLContext._LoadFromDatabase(dataSourceJson);
                    return true;
                
                case "File":
                    string fileName = dataSource.FileName;
                    byte[] fileBytes = dataSource.File;
                    FileInfo tempFile = new FileInfo($"temp/{dataSource.DataSourceId}/{fileName}");
                    if (tempFile.Exists) tempFile.Delete();
                    using(FileStream fileStream = new FileStream(tempFile.FullName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fileStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                    dataSourceJson["LocalPath"] = tempFile.FullName;
                    if (isTestData) TestDataView = MLContext._LoadFromFile(dataSourceJson);
                    else TrainDataView = MLContext._LoadFromFile(dataSourceJson);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Command to parse the model component from the model JSON and add it to the data pipeline.
        /// </summary>
        /// <param name="pipeline">Reference to the EstimatorChain that is being appended to.</param>
        /// <param name="modelType">The string representing the type of component to add to the pipeline.</param>
        /// <param name="componentObject">The JSON configuration of the model component being appended.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool _ParseComponentJson(ref EstimatorChain<ITransformer> pipeline, string modelType, JToken componentObject)
        {
            switch (modelType)
            {
                //------------------------ A ---------------------------------------------------------------------------------
                case "ApplyOnnxModel":
                    pipeline.Append(MLContext._ApplyOnnxModel(componentObject));
                    return true;

                case "ApplyWordEmbedding":
                    pipeline.Append(MLContext._ApplyWordEmbedding(componentObject));
                    return true;

                case "ApproximatedKernelMap":
                    pipeline.Append(MLContext._ApproximatedKernelMap(componentObject));
                    return true;

                case "AveragedPerceptron":
                    pipeline.Append(MLContext._AveragedPerceptronTrainer(componentObject));
                    return true;


                //------------------------ C ---------------------------------------------------------------------------------
                case "CalculateFeatureContribution":
                    pipeline.Append(MLContext._CalculateFeatureContribution(componentObject));
                    return true;

                case "Concatenate":
                    pipeline.Append(MLContext._Concatenate(componentObject));
                    return true;

                case "ConvertToGreyscale":
                    pipeline.Append(MLContext._ConvertToGrayscale(componentObject));
                    return true;

                case "ConvertToImage":
                    pipeline.Append(MLContext._ConvertToImage(componentObject));
                    return true;

                case "ConvertType":
                    pipeline.Append(MLContext._ConvertType(componentObject));
                    return true;

                case "CopyColumns":
                    pipeline.Append(MLContext._CopyColumns(componentObject));
                    return true;


                //------------------------ D ---------------------------------------------------------------------------------
                case "DetectAnomalyBySrCnn":
                    pipeline.Append(MLContext._DetectAnomalyBySrCnn(componentObject));
                    return true;

                case "DetectChangePointBySsa":
                    pipeline.Append(MLContext._DetectChangePointBySsa(componentObject));
                    return true;

                case "DetectIidChangePoint":
                    pipeline.Append(MLContext._DetectIidChangePoint(componentObject));
                    return true;

                case "DetectIidSpike":
                    pipeline.Append(MLContext._DetectIidSpike(componentObject));
                    return true;

                case "DetectSpikeBySsa":
                    pipeline.Append(MLContext._DetectSpikeBySsa(componentObject));
                    return true;

                case "DnnFeaturizeImage":
                    pipeline.Append(MLContext._DnnFeaturizeImage(componentObject));
                    return true;

                case "DropColumns":
                    pipeline.Append(MLContext._DropColumns(componentObject));
                    return true;


                //------------------------ E ---------------------------------------------------------------------------------
                case "ExtractPixels":
                    pipeline.Append(MLContext._ExtractPixels(componentObject));
                    return true;


                //------------------------ F ---------------------------------------------------------------------------------
                case "FastForestBinary":
                    pipeline.Append(MLContext._FastForestBinaryTrainer(componentObject));
                    return true;

                case "FastForestRegression":
                    pipeline.Append(MLContext._FastForestRegressionTrainer(componentObject));
                    return true;

                case "FastTreeBinary":
                    pipeline.Append(MLContext._FastTreeBinaryTrainer(componentObject));
                    return true;

                case "FastTreeRanking":
                    pipeline.Append(MLContext._FastTreeRankingTrainer(componentObject));
                    return true;

                case "FastTreeRegression":
                    pipeline.Append(MLContext._FastTreeRegressionTrainer(componentObject));
                    return true;

                case "FastTreeTweedie":
                    pipeline.Append(MLContext._FastTreeTweedieTrainer(componentObject));
                    return true;

                case "FeaturizeText":
                    pipeline.Append(MLContext._FeaturizeText(componentObject));
                    return true;

                case "FieldAwareFactorizationMachine":
                    pipeline.Append(MLContext._FieldAwareFactorizationMachineTrainer(componentObject));
                    return true;

                case "ForecastBySsa":
                    pipeline.Append(MLContext._ForecastBySsa(componentObject));
                    return true;


                //------------------------ G ---------------------------------------------------------------------------------
                case "GamBinary":
                    pipeline.Append(MLContext._GamBinaryTrainer(componentObject));
                    return true;

                case "GamRegression":
                    pipeline.Append(MLContext._GamRegressionTrainer(componentObject));
                    return true;


                //------------------------ H ---------------------------------------------------------------------------------
                case "Hash":
                    pipeline.Append(MLContext._Hash(componentObject));
                    return true;


                //------------------------ I ---------------------------------------------------------------------------------
                case "IndicateMissingValues":
                    pipeline.Append(MLContext._IndicateMissingValues(componentObject));
                    return true;


                //------------------------ K ---------------------------------------------------------------------------------
                case "KMeans":
                    pipeline.Append(MLContext._KMeansTrainer(componentObject));
                    return true;


                //------------------------ L ---------------------------------------------------------------------------------
                case "LatentDirichletAllocation":
                    pipeline.Append(MLContext._LatentDirichletAllocation(componentObject));
                    return true;

                case "LbfgsLogisticRegressionBinary":
                    pipeline.Append(MLContext._LbfgsLogisticRegressionBinaryTrainer(componentObject));
                    return true;

                case "LbfgsMaximumEntropyMulticlass":
                    pipeline.Append(MLContext._LbfgsMaximumEntropyMulticlassTrainer(componentObject));
                    return true;

                case "LbfgsPoissonRegression":
                    pipeline.Append(MLContext._LbfgsPoissonRegressionTrainer(componentObject));
                    return true;

                case "LightGbmBinary":
                    pipeline.Append(MLContext._LightGbmBinaryTrainer(componentObject));
                    return true;

                case "LightGbmMulticlass":
                    pipeline.Append(MLContext._LightGbmMulticlassTrainer(componentObject));
                    return true;

                case "LightGbmRanking":
                    pipeline.Append(MLContext._LightGbmRankingTrainer(componentObject));
                    return true;

                case "LightGbmRegression":
                    pipeline.Append(MLContext._LightGbmRegressionTrainer(componentObject));
                    return true;

                case "LinearSvm":
                    pipeline.Append(MLContext._LinearSvmTrainer(componentObject));
                    return true;

                case "LoadImages":
                    pipeline.Append(MLContext._LoadImages(componentObject));
                    return true;

                case "LoadTensorFlowModel":
                    pipeline.Append(MLContext._LoadTensorFlowModel(componentObject));
                    return true;


                //------------------------ M ---------------------------------------------------------------------------------
                case "MapKeyToBinaryVector":
                    pipeline.Append(MLContext._MapKeyToBinaryVector(componentObject));
                    return true;

                case "MapKeyToValue":
                    pipeline.Append(MLContext._MapKeyToValue(componentObject));
                    return true;

                case "MapKeyToVector":
                    pipeline.Append(MLContext._MapKeyToVector(componentObject));
                    return true;

                case "MapValue":
                    pipeline.Append(MLContext._MapValue(componentObject));
                    return true;

                case "MapValueToKey":
                    pipeline.Append(MLContext._MapValueToKey(componentObject));
                    return true;

                case "MatrixFactorization":
                    pipeline.Append(MLContext._MatrixFactorizationTrainer(componentObject));
                    return true;


                //------------------------ N ---------------------------------------------------------------------------------
                case "NaiveBayesMulticlass":
                    pipeline.Append(MLContext._NaiveBayesMulticlassTrainer(componentObject));
                    return true;

                case "NormalizeBinning":
                    pipeline.Append(MLContext._NormalizeBinning(componentObject));
                    return true;

                case "NormalizeGlobalContrast":
                    pipeline.Append(MLContext._NormalizeGlobalContrast(componentObject));
                    return true;

                case "NormalizeLogMeanVariance":
                    pipeline.Append(MLContext._NormalizeLogMeanVariance(componentObject));
                    return true;

                case "NormalizeLpNorm":
                    pipeline.Append(MLContext._NormalizeLpNorm(componentObject));
                    return true;

                case "NormalizeMeanVariance":
                    pipeline.Append(MLContext._NormalizeMeanVariance(componentObject));
                    return true;

                case "NormalizeMinMax":
                    pipeline.Append(MLContext._NormalizeMinMax(componentObject));
                    return true;

                case "NormalizeSupervisedBinning":
                    pipeline.Append(MLContext._NormalizeSupervisedBinning(componentObject));
                    return true;

                case "NormalizeText":
                    pipeline.Append(MLContext._NormalizeText(componentObject));
                    return true;


                //------------------------ O ---------------------------------------------------------------------------------
                case "OneHotEncoding":
                    pipeline.Append(MLContext._OneHotEncoding(componentObject));
                    return true;

                case "OneHotHashEncoding":
                    pipeline.Append(MLContext._OneHotHashEncoding(componentObject));
                    return true;

                case "OnlineGradientDescent":
                    pipeline.Append(MLContext._OnlineGradientDescentTrainer(componentObject));
                    return true;

                case "OlsTrainer":
                    pipeline.Append(MLContext._OlsTrainer(componentObject));
                    return true;


                //------------------------ P ---------------------------------------------------------------------------------
                case "Prior":
                    pipeline.Append(MLContext._PriorTrainer(componentObject));
                    return true;

                case "ProduceHashedNgrams":
                    pipeline.Append(MLContext._ProduceHashedNgrams(componentObject));
                    return true;

                case "ProduceHashedWordBags":
                    pipeline.Append(MLContext._ProduceHashedWordBags(componentObject));
                    return true;

                case "ProduceNgrams":
                    pipeline.Append(MLContext._ProduceNgrams(componentObject));
                    return true;

                case "ProduceWordBags":
                    pipeline.Append(MLContext._ProduceWordBags(componentObject));
                    return true;

                case "ProjectToPrincipalComponents":
                    pipeline.Append(MLContext._ProjectToPrincipalComponents(componentObject));
                    return true;


                //------------------------ R ---------------------------------------------------------------------------------
                case "RandomizedPca":
                    pipeline.Append(MLContext._RandomizedPcaTrainer(componentObject));
                    return true;

                case "RemoveDefaultStopWords":
                    pipeline.Append(MLContext._RemoveDefaultStopWords(componentObject));
                    return true;

                case "RemoveStopWords":
                    pipeline.Append(MLContext._RemoveStopWords(componentObject));
                    return true;

                case "ReplaceMissingValues":
                    pipeline.Append(MLContext._ReplaceMissingValues(componentObject));
                    return true;

                case "ResizeImages":
                    pipeline.Append(MLContext._ResizeImages(componentObject));
                    return true;


                //------------------------ S ---------------------------------------------------------------------------------
                case "SdcaLogisticRegressionBinary":
                    pipeline.Append(MLContext._SdcaLogisticRegressionBinaryTrainer(componentObject));
                    return true;

                case "SdcaMaximumEntropyMulticlass":
                    pipeline.Append(MLContext._SdcaMaximumEntropyMulticlassTrainer(componentObject));
                    return true;

                case "SdcaNonCalibratedBinary":
                    pipeline.Append(MLContext._SdcaNonCalibratedBinaryTrainer(componentObject));
                    return true;

                case "SdcaNonCalibratedMulticlass":
                    pipeline.Append(MLContext._SdcaNonCalibratedMulticlassTrainer(componentObject));
                    return true;

                case "SdcaRegression":
                    pipeline.Append(MLContext._SdcaRegressionTrainer(componentObject));
                    return true;

                case "SelectColumns":
                    pipeline.Append(MLContext._SelectColumns(componentObject));
                    return true;

                case "SelectFeaturesBasedOnCount":
                    pipeline.Append(MLContext._SelectFeaturesBasedOnCount(componentObject));
                    return true;

                case "SelectFeaturesBasedOnMutualInformation":
                    pipeline.Append(MLContext._SelectFeaturesBasedOnMutualInformation(componentObject));
                    return true;

                case "SymbolicSgdLogisticRegressionBinary":
                    pipeline.Append(MLContext._SymbolicSgdLogisticRegressionBinaryTrainer(componentObject));
                    return true;


                //------------------------ T ---------------------------------------------------------------------------------
                case "TokenizeIntoCharactersAsKeys":
                    pipeline.Append(MLContext._TokenizeIntoCharactersAsKeys(componentObject));
                    return true;

                case "TokenizeIntoWords":
                    pipeline.Append(MLContext._TokenizeIntoWords(componentObject));
                    return true;


                //------------------------ default ---------------------------------------------------------------------------
                default:
                    return false;
            }
        }
    }
}
