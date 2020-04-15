using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class DataLoadExtensions
    {
        //--------------- public static DATA LOAD FUNCTIONS ----------------------------------------
        public static IDataView _LoadFromDatabase(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //return MLContext.Data.CreateDatabaseLoader();
        }

        public static IDataView _LoadFromSingleFile(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //return MLContext.Data.LoadFromTextFile();
        }

        public static IDataView _LoadFromMultipleFiles(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IDataView _LoadFromBinary(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //return MLContext.Data.LoadFromBinary();
        }
    }
}
