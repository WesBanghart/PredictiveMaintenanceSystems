using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelServicesLibrary
{
    class Model
    {
        public string ModelId { get; set; }
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public string ModelName { get; set; }
        public string Configuration { get; set; }
        public byte[] ModelFile { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime Created { get; set; }
        public MLContext MLContext { get; set; }
        public IDataView DataView { get; set; }
        public ITransformer Transformer { get; set; }
    }
}
