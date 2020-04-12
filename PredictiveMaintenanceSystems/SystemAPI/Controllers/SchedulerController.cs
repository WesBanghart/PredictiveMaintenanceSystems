using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;

namespace SystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly EFSystemContext _context;

        public SchedulerController(EFSystemContext context)
        {
            _context = context;
        }

        // GET: api/Scheduler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchedulerTable>>> GetSchedulers()
        {
            return await _context.Schedulers.ToListAsync();
        }

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

        // PUT: api/Scheduler/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
