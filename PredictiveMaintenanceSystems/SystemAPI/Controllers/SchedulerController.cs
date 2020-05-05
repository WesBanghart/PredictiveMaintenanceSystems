using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;
using Microsoft.AspNetCore.Cors;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// API controller for the scheduler
    /// </summary>
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly EFSystemContext _context;

        /// <summary>
        /// Constructor for the scheduler controller.
        /// </summary>
        /// <param name="context"></param>
        public SchedulerController(EFSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all current Schedules for a User.
        /// </summary>
        /// <returns></returns>
        // GET: api/Scheduler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchedulerTable>>> GetSchedulers()
        {
            return await _context.Schedulers.ToListAsync();
        }

        /// <summary>
        /// Gets a schedule given the schedule ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Scheduler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchedulerTable>> GetSchedulerTable(Guid id)
        {
            var schedulerTable = await _context.Schedulers.FindAsync(id);

            if (schedulerTable == null)
            {
                return NotFound();
            }

            return schedulerTable;
        }

        /// <summary>
        /// Updates an existing schedule given a schedule ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="schedulerTable"></param>
        /// <returns></returns>
        // PUT: api/Scheduler/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedulerTable(Guid id, [FromBody] SchedulerTable schedulerTable)
        {
            var schedule = await _context.Schedulers.FindAsync(id);
            if (schedule == null)
            {
                return BadRequest($"Schedule with ID:{id} does not exit.");
            }

            schedule.ScheduleConfiguration = schedulerTable.ScheduleConfiguration;
            schedule.IsScheduled = schedulerTable.IsScheduled;
            schedule.LastRan = schedulerTable.LastRan;
            schedule.LastUpdated = DateTime.Now;
            

            _context.Entry(schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchedulerTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new Schedule.
        /// </summary>
        /// <param name="schedulerTable"></param>
        /// <returns></returns>
        // POST: api/Scheduler
        [HttpPost]
        public async Task<ActionResult<SchedulerTable>> PostSchedulerTable([FromBody] SchedulerTable schedulerTable)
        {
            var user = await _context.Users.FindAsync(schedulerTable.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID:{schedulerTable.UserId} does not exist.");
            }

            SchedulerTable newSchedulerTable = new SchedulerTable
            {
                ScheduleId = new Guid(),
                ScheduleConfiguration = schedulerTable.ScheduleConfiguration,
                IsScheduled = schedulerTable.IsScheduled,
                LastRan = null,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now,
                UserId = schedulerTable.UserId,
                User = schedulerTable.User
            };


            _context.Schedulers.Add(newSchedulerTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSchedulerTable", new { id = newSchedulerTable.ScheduleId }, newSchedulerTable);
        }

        /// <summary>
        /// Deletes a schedule given the schedule ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Scheduler/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SchedulerTable>> DeleteSchedulerTable(Guid id)
        {
            var schedulerTable = await _context.Schedulers.FindAsync(id);
            if (schedulerTable == null)
            {
                return NotFound();
            }

            _context.Schedulers.Remove(schedulerTable);
            await _context.SaveChangesAsync();

            return schedulerTable;
        }

        private bool SchedulerTableExists(Guid id)
        {
            return _context.Schedulers.Any(e => e.ScheduleId == id);
        }
    }
}
