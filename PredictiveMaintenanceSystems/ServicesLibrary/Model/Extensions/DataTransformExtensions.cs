using Microsoft.ML;
using Microsoft.ML.Data;
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
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            DataKind dataKind = Enum.Parse<DataKind>(componentObject.Value<string>("Datatype"));
            return MLContext.Transforms.Conversion.ConvertType(outputColumn, inputColumn, dataKind);
        }

        public static IEstimator<ITransformer> _MapValue(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _MapValueToKey(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.Conversion.MapValueToKey(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _MapKeyToValue(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.Conversion.MapKeyToValue(outputColumn, inputColumn);
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
