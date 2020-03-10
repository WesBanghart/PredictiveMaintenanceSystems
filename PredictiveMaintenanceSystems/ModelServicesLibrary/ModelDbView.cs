using System;
using System.Collections.Generic;
using System.Text;

namespace ModelServicesLibrary
{
    public class ModelDbView
    {
        public Guid ModelId { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string ModelName { get; set; }
        public string Configuration { get; set; }
        public byte[] ModelFile { get; set; }
    }
}
