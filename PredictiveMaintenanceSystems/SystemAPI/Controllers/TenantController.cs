using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemAPI.Data;
using SystemAPI.Models;

namespace SystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly APIContext _context;

        public TenantController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantModel>>> GetTenantModels()
        {
            return await _context.TenantModels.ToListAsync();
        }

        // GET: api/Tenant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantModel>> GetTenantModel(string id)
        {
            var tenantModel = await _context.TenantModels.FindAsync(id);

            if (tenantModel == null)
            {
                return NotFound();
            }

            return tenantModel;
        }

        // PUT: api/Tenant/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenantModel(string id, TenantModel tenantModel)
        {
            if (id != tenantModel.TenantId)
            {
                return BadRequest();
            }

            _context.Entry(tenantModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantModelExists(id))
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

        // POST: api/Tenant
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TenantModel>> PostTenantModel(TenantModel tenantModel)
        {
            _context.TenantModels.Add(tenantModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TenantModelExists(tenantModel.TenantId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTenantModel", new { id = tenantModel.TenantId }, tenantModel);
        }

        // DELETE: api/Tenant/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TenantModel>> DeleteTenantModel(string id)
        {
            var tenantModel = await _context.TenantModels.FindAsync(id);
            if (tenantModel == null)
            {
                return NotFound();
            }

            _context.TenantModels.Remove(tenantModel);
            await _context.SaveChangesAsync();

            return tenantModel;
        }

        private bool TenantModelExists(string id)
        {
            return _context.TenantModels.Any(e => e.TenantId == id);
        }
    }
}
