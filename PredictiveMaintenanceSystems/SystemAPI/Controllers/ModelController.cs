using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;
using ServicesLibrary;
using ServicesLibrary.Model.Run;
using ServicesLibrary.Model.Update;

namespace SystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly EFSystemContext _context;
        private readonly ServicesLibrary.Model.BackgroundTaskQueue backgroundTaskQueue;

        public ModelController(EFSystemContext context, ServicesLibrary.Model.BackgroundTaskQueue queue)
        {
            _context = context;
            backgroundTaskQueue = queue;
        }

        // GET: api/Model
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelTable>>> GetModels()
        {
            return await _context.Models.ToListAsync();
        }

        // GET: api/Model/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelTable>> GetModel(Guid id)
        {
            var modelTable = await _context.Models.FindAsync(id);

            if (modelTable == null)
            {
                return NotFound();
            }

            return modelTable;
        }

        // PUT: api/Model/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(Guid id, ModelTable modelTable)
        {
            if (id != modelTable.ModelId)
            {
                return BadRequest();
            }

            _context.Entry(modelTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
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

        // POST: api/Model
        [HttpPost]
        public async Task<ActionResult<ModelTable>> PostModel(ModelTable modelTable)
        {
            _context.Models.Add(modelTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModel", new { id = modelTable.ModelId }, modelTable);
        }

        // Put: api/Model/
        [HttpPut("{id}/Save")]
        public async Task<ActionResult<ModelTable>> PutModelSave(Guid modelId, string configuration)
        {
            if (!ModelExists(modelId))
            {
                return BadRequest();
            }

            ModelTable updatedModel = await _context.Models.FindAsync(modelId);
            updatedModel.Configuration = configuration;
            updatedModel.LastUpdated = DateTime.Now;

            _context.Entry(updatedModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(modelId))
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

        [HttpPut("{id}/Save-And-Run")]
        public async Task<ActionResult<ModelTable>> PutModelSaveAndRun(Guid modelId, string configuration)
        {
            if (!ModelExists(modelId))
            {
                return BadRequest();
            }

            ModelTable updatedModel = await _context.Models.FindAsync(modelId);
            updatedModel.Configuration = configuration;
            updatedModel.LastUpdated = DateTime.Now;

            _context.Entry(updatedModel).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
                backgroundTaskQueue.QueueModelRunWorkItem(modelId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(modelId))
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

        [HttpPut("{id}/Save-And-Train")]
        public async Task<ActionResult<ModelTable>> PutModelSaveAndTrain(Guid modelId, string configuration)
        {
            if (!ModelExists(modelId))
            {
                return BadRequest();
            }

            ModelTable updatedModel = await _context.Models.FindAsync(modelId);
            updatedModel.Configuration = configuration;
            updatedModel.LastUpdated = DateTime.Now;

            _context.Entry(updatedModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                backgroundTaskQueue.QueueModelUpdateWorkItem(modelId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(modelId))
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

        // DELETE: api/Model/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ModelTable>> DeleteModelTable(Guid id)
        {
            var modelTable = await _context.Models.FindAsync(id);
            if (modelTable == null)
            {
                return NotFound();
            }

            _context.Models.Remove(modelTable);
            await _context.SaveChangesAsync();

            return modelTable;
        }

        private bool ModelExists(Guid id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
