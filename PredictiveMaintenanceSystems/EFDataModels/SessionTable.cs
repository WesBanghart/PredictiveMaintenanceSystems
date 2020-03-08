using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [NotMapped]
    class SessionTable
    {
        public float Version { get; set; }
    }
}
