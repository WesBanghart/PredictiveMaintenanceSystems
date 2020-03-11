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
    public class ModelController : ControllerBase
    {
        private readonly EFSystemContext _context;

        public ModelController(EFSystemContext context)
        {
            _context = context;
        }

        // GET: api/Model
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelTable>>> GetModels()
        {
            return await _context.Models.ToListAsync();
        }

        // GET: api/Model/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelTable>> GetModelTable(Guid id)
        {
            var modelTable = await _context.Models.FindAsync(id);

            if (modelTable == null)
            {
                return NotFound();
            }

            return modelTable;
        }

        // PUT: api/Model/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModelTable(Guid id, ModelTable modelTable)
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
                if (!ModelTableExists(id))
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ModelTable>> PostModelTable(ModelTable modelTable)
        {
            _context.Models.Add(modelTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModelTable", new { id = modelTable.ModelId }, modelTable);
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

        private bool ModelTableExists(Guid id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
