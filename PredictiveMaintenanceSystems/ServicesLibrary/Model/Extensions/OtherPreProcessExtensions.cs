using Microsoft.ML;
using Microsoft.ML.Transforms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class OtherPreProcessExtensions
    {
        //--------------- MISSING VALUES ------------------------------------------------------
        public static IEstimator<ITransformer> _IndicateMissingValues(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.IndicateMissingValues(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _ReplaceMissingValues(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.ReplaceMissingValues(outputColumn, inputColumn);
        }


        //--------------- CATEGORICAL TRANSFORMATIONS -----------------------------------------
        public static IEstimator<ITransformer> _OneHotEncoding(this MLContext MLContext, JToken componentObject)
        {
            //TODO: determine how to pass IDataView parameter
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _OneHotHashEncoding(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            OneHotEncodingEstimator.OutputKind outputKind = Enum.Parse<OneHotEncodingEstimator.OutputKind>(componentObject.Value<string>("OutputKind"));
            int numBits = componentObject.Value<int>("NumberOfBits");
            uint seed = componentObject.Value<uint>("Seed");
            bool useOrderedHashing = componentObject.Value<bool>("UseOrderedHashing");
            int maxNumInverts = componentObject.Value<int>("MaximumNumberOfInverts");
            return MLContext.Transforms.Categorical.OneHotHashEncoding(outputColumn, inputColumn, outputKind, numBits,
                                                                       seed, useOrderedHashing, maxNumInverts);
        }


        //--------------- FEATURE SELECTION ---------------------------------------------------
        public static IEstimator<ITransformer> _SelectFeaturesBasedOnCount(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            long count = componentObject.Value<long>("Count");
            return MLContext.Transforms.FeatureSelection.SelectFeaturesBasedOnCount(outputColumn, inputColumn, count);
        }

        public static IEstimator<ITransformer> _SelectFeaturesBasedOnMutualInformation(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            int outputSlots = componentObject.Value<int>("SlotsInOutput");
            int numBins = componentObject.Value<int>("NumberOfBins");
            return MLContext.Transforms.FeatureSelection.SelectFeaturesBasedOnMutualInformation(outputColumn, inputColumn, labelColumn, outputSlots, numBins);
        }

        //--------------- FEATURE TRANSFORMATIONS ---------------------------------------------
        public static IEstimator<ITransformer> _ApproximatedKernelMap(this MLContext MLContext, JToken componentObject)
        {
            //TODO: look into KernelBase
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ProjectToPrincipalComponents(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            int rank = componentObject.Value<int>("Rank");
            int overSampling = componentObject.Value<int>("Oversampling");
            bool ensureZeroMean = componentObject.Value<bool>("EnsureZeroMean");
            int seed = componentObject.Value<int>("Seed");
            return MLContext.Transforms.ProjectToPrincipalComponents(outputColumn, inputColumn, exampleWeightColumn,
                                                                     rank, overSampling, ensureZeroMean, seed);
        }

        public static IEstimator<ITransformer> _CalculateFeatureContribution(this MLContext MLContext, JToken componentObject)
        {
            //TODO: look into how to properly create prediction transformer needed for calculate feature contribution
            throw new NotImplementedException();
        }

        //--------------- DEEP LEARNING TRANSFORMATIONS ---------------------------------------
        public static IEstimator<ITransformer> _ApplyOnnxModel(this MLContext MLContext, JToken componentObject)
        {
            //TODO: look into how ML.NET handles adding Onnx model into data pipeline
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LoadTensorFlowModel(this MLContext MLContext, JToken componentObject)
        {
            //TODO: look into how ML.NET handles adding TensorFlow model into data pipeline
            throw new NotImplementedException();
        }


        //--------------- RECOMMENDATION -------------------------------------------------------
        public static IEstimator<ITransformer> _MatrixFactorizationTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string matrixColumnIndex = componentObject.Value<string>("MatrixColumnIndexColumnName");
            string matrixRowIndex = componentObject.Value<string>("MatrixRowIndexColumnName");
            int approximationRank = componentObject.Value<int>("ApproximationRank");
            double learningRate = componentObject.Value<double>("LearningRate");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.Recommendation().Trainers.MatrixFactorization(labelColumn, matrixColumnIndex,
                                                                           matrixRowIndex, approximationRank,
                                                                           learningRate, numIterations);
        }
    }
}
