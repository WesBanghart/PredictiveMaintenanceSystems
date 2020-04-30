using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class DataLoadExtensions
    {
        //--------------- public static DATA LOAD FUNCTIONS ----------------------------------------
        public static IDataView _LoadFromDatabase(this MLContext MLContext, JToken componentObject)
        {
            List<DatabaseLoader.Column> columns = new List<DatabaseLoader.Column>();

            foreach(JToken columnObject in componentObject["Columns"].Children())
            {
                string name = columnObject.Value<string>("Name");
                int colIndex = columnObject.Value<int>("ColIndex");
                DbType dbType = Enum.Parse<DbType>(columnObject.Value<string>("DataType"));
                columns.Add(new DatabaseLoader.Column(name, dbType, colIndex));
            }

            DatabaseLoader loader = MLContext.Data.CreateDatabaseLoader(columns.ToArray());
            string connectionString = componentObject.Value<string>("ConnectionString");
            string sqlCommand = componentObject.Value<string>("SqlCommand");
            DatabaseSource databaseSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);

            return loader.Load(databaseSource);
        }

        public static IDataView _LoadFromFile(this MLContext MLContext, JToken componentObject)
        {
            List<TextLoader.Column> columns = new List<TextLoader.Column>();

            foreach (JToken columnObject in componentObject["Columns"].Children())
            {
                string name = columnObject.Value<string>("Name");
                int colIndex = columnObject.Value<int>("ColIndex");
                DataKind dataKind = Enum.Parse<DataKind>(columnObject.Value<string>("DataType"));
                columns.Add(new TextLoader.Column(name, dataKind, colIndex));
            }

            string path = componentObject.Value<string>("Path");
            char separatorChar = componentObject.Value<char>("SeparatorChar");
            bool hasHeader = componentObject.Value<bool>("HasHeader");
            bool allowQuoting = componentObject.Value<bool>("AllowQuoting");
            bool trimWhitespace = componentObject.Value<bool>("TrimWhitespace");
            bool allowSparse = componentObject.Value<bool>("AllowSparse");

            return MLContext.Data.LoadFromTextFile(path, columns.ToArray(), separatorChar, hasHeader, allowQuoting, trimWhitespace, allowSparse);
        }

        public static IDataView _LoadFromBinary(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //return MLContext.Data.LoadFromBinary();
        }

        public static IDataView _FilterRowsByColumn(this MLContext MLContext, ref IDataView dataView, JToken componentObject)
        {
            string columnName = componentObject.Value<string>("ColumnName");
            float lowerBound = componentObject.Value<float>("LowerBound");
            float upperBound = componentObject.Value<float>("UpperBound");
            return MLContext.Data.FilterRowsByColumn(dataView, columnName, lowerBound, upperBound);
        }

        public static IDataView _FilterRowsByMissingValues(this MLContext MLContext, ref IDataView dataView, JToken componentObject)
        {
            string[] columns = componentObject.Value<string>("ColumnNames").Split(", ");
            return MLContext.Data.FilterRowsByMissingValues(dataView, columns);
        }
    }
}
