using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class ColumnWiseTransformExtensions
    {
        //--------------- COLUMN-WISE OPS -----------------------------------------------------
        public static IEstimator<ITransformer> _Concatenate(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string[] inputColumns = componentObject.Value<string>("InputColumnNames").Split(", ");
            return MLContext.Transforms.Concatenate(outputColumn, inputColumns);
        }

        public static IEstimator<ITransformer> _CopyColumns(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.CopyColumns(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _DropColumns(this MLContext MLContext, JToken componentObject)
        {
            string[] dropColumns = componentObject.Value<string>("ColumnNames").Split(", ");
            return MLContext.Transforms.DropColumns(dropColumns);
        }

        public static IEstimator<ITransformer> _SelectColumns(this MLContext MLContext, JToken componentObject)
        {
            string[] selectColumns = componentObject.Value<string>("ColumnNames").Split(", ");
            return MLContext.Transforms.SelectColumns(selectColumns);
        }
    }
}
