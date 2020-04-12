using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFDataModels
{
    [Table("scheduler_tbl")]
    public class SchedulerTable
    {
        //Primary key for scheduler entries
        [Key]
        public Guid ScheduleId { get; set; }
        //The configuration of the scheduler
        public string ScheduleConfiguration { get; set; }
        //Represents if the scheduler should be ran or skipped
        public bool IsScheduled { get; set; }
        //The datetime the scheduler was last ran
        public DateTime? LastRan { get; set; }
        //The datetime the scheduler entry was created
        public DateTime Created { get; set; }
        //The datetime the scheduler entry was last updated
        public DateTime? LastUpdated { get; set; }

        //Timestamp value, used as a concurrency token, value is automatically generated on insert/update
        [Timestamp]
        public byte[] Timestamp { get; set; }

        //Represents the relationship to a single user entity
        public Guid UserId { get; set; }
        public UserTable User { get; set; }
    }
}
