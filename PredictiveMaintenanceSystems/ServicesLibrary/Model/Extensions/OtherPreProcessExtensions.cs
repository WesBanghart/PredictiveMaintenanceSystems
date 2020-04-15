using Microsoft.ML;
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
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _OneHotHashEncoding(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }


        //--------------- FEATURE SELECTION ---------------------------------------------------
        public static IEstimator<ITransformer> _SelectFeaturesBasedOnCount(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SelectFeaturesBasedOnMutualInformation(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        //--------------- FEATURE TRANSFORMATIONS ---------------------------------------------
        public static IEstimator<ITransformer> _ApproximatedKernelMap(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ProjectToPrincipalComponents(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _CalculateFeatureContribution(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        //--------------- CALIBRATION TRANSFORMATIONS -----------------------------------------
        public static IEstimator<ITransformer> _EstimatedPlatt(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FixedPlatt(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NaiveDistribution(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _IsotonicDistribution(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        //--------------- DEEP LEARNING TRANSFORMATIONS ---------------------------------------
        public static IEstimator<ITransformer> _ApplyOnnxModel(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LoadTensorFlowModel(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        //--------------- CUSTOM MAPPING ------------------------------------------------------
        public static IEstimator<ITransformer> _CustomMapping(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }


        //--------------- RECOMMENDATION -------------------------------------------------------
        public static IEstimator<ITransformer> _MatrixFactorizationTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
