using EFDataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model
{
    public class ModelTaskController
    {
        private EFSystemContext _context;

        public string ModelJson { get; set; }
        public string ModelConfiguration { get; set; }
        public MLContext MLContext { get; set; }
        public IDataView TrainDataView { get; set; }
        public IDataView TestDataView { get; set; }
        public ITransformer Transformer { get; set; }

        public ModelTaskController(EFSystemContext context)
        {
            _context = context;
        }


        //------------- PUBLIC OPERATIONS ----------------------------------------------------
        public async Task<bool> RunModel(Guid ModelId, CancellationToken cancellationToken)
        {
            if (await UpdateModel(ModelId, cancellationToken))
            {
                var results = Transformer.Transform(TestDataView);
                //TODO: save results
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateModel(Guid ModelId, CancellationToken cancellationToken)
        {
            if (_RetrieveModel(ModelId))
            {
                if (_ParseModelJSON())
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


        //--------------- PRIVATE DATABASE ACCESSORS -----------------------------------------
        private bool _RetrieveModel(Guid ModelId)
        {
            throw new NotImplementedException();
        }

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
        private bool _ParseModelJSON()
        {
            throw new NotImplementedException();
        }

        private bool _ParseAppendJSON()
        {
            throw new NotImplementedException();
        }


        //--------------- PRIVATE DATA LOAD FUNCTIONS ----------------------------------------
        private bool _LoadFromDatabase()
        {
            throw new NotImplementedException();
        }

        private bool _LoadFromSingleFile()
        {
            throw new NotImplementedException();
        }

        private bool _LoadFromMultipleFiles()
        {
            throw new NotImplementedException();
        }


        //=============== DATA PREP TRANSFORMATIONS ===========================================
        //--------------- COLUMN-WISE OPS -----------------------------------------------------
        private bool _Concatenate()
        {
            throw new NotImplementedException();
        }

        private bool _CopyColumns()
        {
            throw new NotImplementedException();
        }

        private bool _DropColumns()
        {
            throw new NotImplementedException();
        }

        private bool _SelectColumns()
        {
            throw new NotImplementedException();
        }

        private bool _FilterRowsByColumn()
        {
            throw new NotImplementedException();
        }

        //--------------- MISSING VALUES ------------------------------------------------------
        private bool _IndicateMissingValues()
        {
            throw new NotImplementedException();
        }

        private bool _ReplaceMissingValues()
        {
            throw new NotImplementedException();
        }

        //--------------- NORMALIZE DATA ------------------------------------------------------
        private bool _NormalizeMeanVariance()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeLogMeanVariance()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeLpNorm()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeGlobalContrast()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeBinning()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeSupervisedBinning()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeMinMax()
        {
            throw new NotImplementedException();
        }

        //--------------- CONVERT DATA TYPE ---------------------------------------------------
        private bool _ConvertType()
        {
            throw new NotImplementedException();
        }

        private bool _MapValue()
        {
            throw new NotImplementedException();
        }

        private bool _MapValueToKey()
        {
            throw new NotImplementedException();
        }

        private bool _MapKeyToValue()
        {
            throw new NotImplementedException();
        }

        private bool _MapKeyToVector()
        {
            throw new NotImplementedException();
        }

        private bool _MapKeyToBinaryVector()
        {
            throw new NotImplementedException();
        }

        private bool _Hash()
        {
            throw new NotImplementedException();
        }

        //--------------- TEXT TRANSFORMATIONS ------------------------------------------------
        private bool _FeaturizeText()
        {
            throw new NotImplementedException();
        }

        private bool _TokenizeIntoWords()
        {
            throw new NotImplementedException();
        }

        private bool _TokenizeIntoCharactersAsKeys()
        {
            throw new NotImplementedException();
        }

        private bool _NormalizeText()
        {
            throw new NotImplementedException();
        }

        private bool _ProduceNgrams()
        {
            throw new NotImplementedException();
        }

        private bool _ProduceWordBags()
        {
            throw new NotImplementedException();
        }

        private bool _ProduceHashedNgrams()
        {
            throw new NotImplementedException();
        }

        private bool _ProduceHashedWordBags()
        {
            throw new NotImplementedException();
        }

        private bool _RemoveDefaultStopWords()
        {
            throw new NotImplementedException();
        }

        private bool _RemoveStopWords()
        {
            throw new NotImplementedException();
        }

        private bool _LatentDirichletAllocation()
        {
            throw new NotImplementedException();
        }

        private bool _ApplyWordEmbedding()
        {
            throw new NotImplementedException();
        }

        //--------------- IMAGE TRANSFORMATIONS -----------------------------------------------
        private bool _ConvertToGrayscale()
        {
            throw new NotImplementedException();
        }

        private bool _ConvertToImage()
        {
            throw new NotImplementedException();
        }

        private bool _ExtractPixels()
        {
            throw new NotImplementedException();
        }

        private bool _LoadImages()
        {
            throw new NotImplementedException();
        }

        private bool _ResizeImages()
        {
            throw new NotImplementedException();
        }

        private bool _DnnFeaturizeImage()
        {
            throw new NotImplementedException();
        }

        //--------------- CATEGORICAL TRANSFORMATIONS -----------------------------------------
        private bool _OneHotEncoding()
        {
            throw new NotImplementedException();
        }

        private bool _OneHotHashEncoding()
        {
            throw new NotImplementedException();
        }

        //--------------- TIME-SERIES TRANSFORMATIONS -----------------------------------------
        private bool _DetectAnomalyBySrCnn()
        {
            throw new NotImplementedException();
        }

        private bool _DetectChangePointBySsa()
        {
            throw new NotImplementedException();
        }

        private bool _DetectIidChangePoint()
        {
            throw new NotImplementedException();
        }

        private bool _ForecastBySsa()
        {
            throw new NotImplementedException();
        }

        private bool _DetectSpikeBySsa()
        {
            throw new NotImplementedException();
        }

        private bool _DetectIidSpike()
        {
            throw new NotImplementedException();
        }

        //--------------- FEATURE SELECTION ---------------------------------------------------
        private bool _SelectFeaturesBasedOnCount()
        {
            throw new NotImplementedException();
        }

        private bool _SelectFeaturesBasedOnMutualInformation()
        {
            throw new NotImplementedException();
        }

        //--------------- FEATURE TRANSFORMATIONS ---------------------------------------------
        private bool _ApproximatedKernelMap()
        {
            throw new NotImplementedException();
        }

        private bool _ProjectToPrincipalComponents()
        {
            throw new NotImplementedException();
        }

        private bool _CalculateFeatureContribution()
        {
            throw new NotImplementedException();
        }

        //--------------- CALIBRATION TRANSFORMATIONS -----------------------------------------
        private bool _EstimatedPlatt()
        {
            throw new NotImplementedException();
        }

        private bool _FixedPlatt()
        {
            throw new NotImplementedException();
        }

        private bool _NaiveDistribution()
        {
            throw new NotImplementedException();
        }

        private bool _IsotonicDistribution()
        {
            throw new NotImplementedException();
        }

        //--------------- DEEP LEARNING TRANSFORMATIONS ---------------------------------------
        private bool _ApplyOnnxModel()
        {
            throw new NotImplementedException();
        }

        private bool _LoadTensorFlowModel()
        {
            throw new NotImplementedException();
        }

        //--------------- CUSTOM MAPPING ------------------------------------------------------
        private bool _CustomMapping()
        {
            throw new NotImplementedException();
        }


        //=============== ANOMALY DETECTION FUNCTIONS =========================================
        private bool _RandomizedPcaTrainer()
        {
            throw new NotImplementedException();
        }


        //=============== BINARY CLASSIFICATION FUNCTIONS =====================================
        private bool _AveragedPerceptronTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _SdcaLogisticRegressionBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _SdcaNonCalibratedBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _SymbolicSgdLogisticRegressionBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _LbfgsLogisticRegressionBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _LightGbmBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FastTreeBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FastForestBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _GamBinaryTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FieldAwareFactorizationMachineTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _PriorTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _LinearSvmTrainer()
        {
            throw new NotImplementedException();
        }


        //=============== CLUSTERING FUNCTIONS ================================================
        private bool _KMeansTrainer()
        {
            throw new NotImplementedException();
        }


        //=============== MULTICLASS CLASSIFICATION FUNCTIONS =================================
        private bool _LightGbmMulticlassTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _SdcaMaximumEntropyMulticlassTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _SdcaNonCalibratedMulticlassTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _LbfgsMaximumEntropyMulticlassTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _NaiveBayesMulticlassTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _OneVersusAllTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _PairwiseCouplingTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _ImageClassificationTrainer()
        {
            throw new NotImplementedException();
        }


        //=============== RANKING FUNCTIONS ===================================================
        private bool _LightGbmRankingTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FastTreeRankingTrainer()
        {
            throw new NotImplementedException();
        }


        //=============== REGRESSION FUNCTIONS ================================================
        private bool _LbfgsPoissonRegressionTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _LightGbmRegressionTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _SdcaRegressionTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _OlsTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _OnlineGradientDescentTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FastTreeRegressionTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FastTreeTweedieTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _FastForestRegressionTrainer()
        {
            throw new NotImplementedException();
        }

        private bool _GamRegressionTrainer()
        {
            throw new NotImplementedException();
        }


        //=============== OTHER FUNCTIONS ======================================================
        //--------------- RECOMMENDATION -------------------------------------------------------
        private bool _MatrixFactorizationTrainer()
        {
            throw new NotImplementedException();
        }
    }
}
