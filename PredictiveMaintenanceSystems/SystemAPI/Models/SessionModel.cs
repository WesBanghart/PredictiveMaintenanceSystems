using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SystemAPI.Models
{
    [NotMapped]
    public class SessionModel
    {
        
        public float Version { get; set; }
        //TODO Add prop for Auth token
    }
}
