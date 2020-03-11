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
    public class TenantController : ControllerBase
    {
        private readonly EFSystemContext _context;

        public TenantController(EFSystemContext context)
        {
            _context = context;
        }

        // GET: api/Tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantTable>>> GetTenants()
        {
            return await _context.Tenants.ToListAsync();
        }

        // GET: api/Tenant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantTable>> GetTenantTable(Guid id)
        {
            var tenantTable = await _context.Tenants.FindAsync(id);

            if (tenantTable == null)
            {
                return NotFound();
            }

            return tenantTable;
        }

        // PUT: api/Tenant/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenantTable(Guid id, TenantTable tenantTable)
        {
            if (id != tenantTable.TenantId)
            {
                return BadRequest();
            }

            _context.Entry(tenantTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantTableExists(id))
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
        public async Task<ActionResult<TenantTable>> PostTenantTable(TenantTable tenantTable)
        {
            _context.Tenants.Add(tenantTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenantTable", new { id = tenantTable.TenantId }, tenantTable);
        }

        // DELETE: api/Tenant/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TenantTable>> DeleteTenantTable(Guid id)
        {
            var tenantTable = await _context.Tenants.FindAsync(id);
            if (tenantTable == null)
            {
                return NotFound();
            }

            _context.Tenants.Remove(tenantTable);
            await _context.SaveChangesAsync();

            return tenantTable;
        }

        private bool TenantTableExists(Guid id)
        {
            return _context.Tenants.Any(e => e.TenantId == id);
        }
    }
}
