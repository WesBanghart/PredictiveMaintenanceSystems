using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class DataTransformExtensions
    {
        //--------------- CONVERT DATA TYPE ---------------------------------------------------
        public static IEstimator<ITransformer> _ConvertType(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _MapValue(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _MapValueToKey(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _MapKeyToValue(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _MapKeyToVector(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _MapKeyToBinaryVector(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _Hash(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
