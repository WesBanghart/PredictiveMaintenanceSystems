using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class NormalizeTransformExtensions
    {
        //--------------- NORMALIZE DATA ------------------------------------------------------
        public static IEstimator<ITransformer> _NormalizeMeanVariance(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NormalizeLogMeanVariance(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NormalizeLpNorm(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NormalizeGlobalContrast(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NormalizeBinning(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NormalizeSupervisedBinning(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NormalizeMinMax(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
