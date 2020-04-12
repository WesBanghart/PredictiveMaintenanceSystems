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
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _CopyColumns(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _DropColumns(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SelectColumns(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FilterRowsByColumn(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
