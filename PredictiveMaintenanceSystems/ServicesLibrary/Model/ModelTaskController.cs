using EFDataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Newtonsoft.Json.Linq;
using ServicesLibrary.Model.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesLibrary.Model
{
    public class ModelTaskController
    {
        private EFSystemContext _context;

        public string ModelJson { get; set; }
        Guid TrainDataSourceId { get; set; }
        Guid TestDataSourceId { get; set; }
        public MLContext MLContext { get; set; }
        public IDataView TrainDataView { get; set; }
        public IDataView TestDataView { get; set; }
        public ITransformer Transformer { get; set; }

        public ModelTaskController(EFSystemContext context)
        {
            _context = context;
        }


        //------------- PUBLIC OPERATIONS ----------------------------------------------------
        public async Task<bool> RunModel(Guid ModelId, CancellationToken cancellationToken)
        {
            if (await UpdateModel(ModelId, cancellationToken))
            {
                var results = Transformer.Transform(TestDataView);
                //TODO: save results
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateModel(Guid ModelId, CancellationToken cancellationToken)
        {
            try
            {
                if (_RetrieveModel(ModelId))
                {
                    if (_ParseModelJSON())
                    {
                        byte[] modelZip = _SaveModel(ModelId);
                        var modelEntry = await _context.FindAsync<EFDataModels.ModelTable>(ModelId, cancellationToken);
                        modelEntry.File = modelZip;
                        modelEntry.LastUpdated = DateTime.Now;
                        _context.Entry(modelEntry).State = EntityState.Modified;

                        try
                        {
                            await _context.SaveChangesAsync(cancellationToken);
                            return true;
                        }
                        catch (Exception)
                        {
                            //TODO: log exception
                            return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                //TODO: log exception
                return false;
            }
        }


        //--------------- PRIVATE DATABASE ACCESSORS -----------------------------------------
        private bool _RetrieveModel(Guid ModelId)
        {
            var modelEntry = _context.Find<EFDataModels.ModelTable>(ModelId);

            if (modelEntry == null) return false;

            ModelJson = modelEntry.Configuration;

            return true;
        }

        private byte[] _SaveModel(Guid ModelId)
        {
            string tempZip = $"temp/{ModelId}_temp.zip";
            string tempDir = $"temp/{ModelId}_temp/";

            MLContext.Model.Save(Transformer, TrainDataView.Schema, tempZip);

            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            Directory.CreateDirectory(tempDir);
            ZipFile.ExtractToDirectory(tempZip, tempDir);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (string file in Directory.EnumerateFiles(tempDir))
                    {
                        zipArchive.CreateEntryFromFile($"{tempDir}{file}", file);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        //--------------- PRIVATE PARSE FUNCTIONS --------------------------------------------
        private bool _ParseModelJSON()
        {
            JObject modelObject = JObject.Parse(ModelJson);
            
            //parse training data source id
            TrainDataSourceId = Guid.Parse((string)modelObject["Data"]["TrainSetSource"]["DataSourceId"]);
            
            //parse testing data source id
            if (modelObject.TryGetValue("TestSetSource", out JToken token))
            {
                TestDataSourceId = Guid.Parse((string)modelObject["Data"]["TestSetSource"]["DataSourceId"]);
            }

            //parse model configuration
            string mlContextSeed = (string)modelObject["Model"]["MLContext"]["Seed"];
            
            if (int.TryParse(mlContextSeed, out int seed)) MLContext = new MLContext(seed);
            else MLContext = new MLContext();

            string modelType = (string)modelObject["Model"]["ComponentList"][0]["ComponentType"];
            JToken componentObject = modelObject.SelectToken("Model.ComponentList[0]");
            
            if (String.IsNullOrEmpty(modelType)) return false;

            switch (modelType)
            {
                case "Concatenate":
                    IEstimator<ITransformer> modelEstimator = MLContext._Concatenate(componentObject);
                    break;
                case "CopyColumns":
                case "DropColumns":
                case "SelectColumns":
                case "FilterRowsByColumn":
                default:
                    return false;
            }

            return true;
        }
    }
}
