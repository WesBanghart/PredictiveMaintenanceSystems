using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("scheduler_tbl")]
    class SchedulerTable
    {
        [Key]
        public string ScheduleId { get; set; }

        //[ForeignKey]
        public string ModelId { get; set; }

        //[ForeignKey]
        public string UserId { get; set; }

        //[ForeignKey]
        public string TenantId { get; set; }

        public string ScheduleConfiguration { get; set; }

        public DateTime? LastRan { get; set; }

        public DateTime Created { get; set; }

        public bool IsScheduled { get; set; }
    }
}
