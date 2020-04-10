using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;
using System.Collections;

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
        public async Task<ActionResult<TenantTable>> GetTenant(Guid id)
        {
            var tenantTable = await _context.Tenants.FindAsync(id);

            if (tenantTable == null)
            {
                return NotFound();
            }

            return tenantTable;
        }

        // GET: api/Tenant/Company/Hamilton
        //[HttpGet("{companyName:string}")]
        [HttpGet("Company/{companyName}")]
        public async Task<ActionResult<TenantTable>> GetTenant(string companyName)
        {
            var tenantTable = await _context.Tenants.SingleAsync(x => x.Company == companyName);

            if (tenantTable == null)
            {
                return NotFound();
            }
            return tenantTable;
        }

        // Get: api/Tenant/{id}/Models
        //[HttpGet("{id}/Models")]
        //public async Task<ActionResult<IEnumerable<ModelTable>>> GetTenantModels(Guid id)
        //{
        //    var tenantTable = await _context.Tenants.FindAsync(id);

        //    if (tenantTable == null)
        //    {
        //        return NotFound();
        //    }

        //    if (tenantTable.Models == null || tenantTable.Models.Count < 1)
        //    {
        //        return NotFound("No Models Found.");
        //    }

        //    return tenantTable.Models.ToList();
        //}

        // Get: api/Tenant/{id}/Users
        [HttpGet("{id}/Users")]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetTenantUsers(Guid id)
        {
            var tenantTable = await _context.Tenants.FindAsync(id);

            if (tenantTable == null)
            {
                return NotFound();
            }

            if (tenantTable.Users == null || tenantTable.Users.Count < 1)
            {
                return NotFound("No Models Found.");
            }

            return tenantTable.Users.ToList();
        }

        // PUT: api/Tenant/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant(Guid id, TenantTable tenantTable)
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
        [HttpPost]
        public async Task<ActionResult<TenantTable>> PostTenant(string company, string contactName, string contactPhone, string contactEmail)
        {

            TenantTable newTable = new TenantTable
            {
                TenantId = new Guid(),
                Company = company,
                ContactName = contactName,
                ContactEmail = contactEmail,
                ContactPhone = contactPhone,
                Users = new List<UserTable>(),
            };
            _context.Tenants.Add(newTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenant", new { id = newTable.TenantId }, newTable);
        }

        //TODO: fix cascade deleting
        // DELETE: api/Tenant/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TenantTable>> DeleteTenant(Guid id)
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
