using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class TimeSeriesTransformExtensions
    {
        //--------------- TIME-SERIES TRANSFORMATIONS -----------------------------------------
        public static IEstimator<ITransformer> _DetectAnomalyBySrCnn(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _DetectChangePointBySsa(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _DetectIidChangePoint(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ForecastBySsa(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _DetectSpikeBySsa(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _DetectIidSpike(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
