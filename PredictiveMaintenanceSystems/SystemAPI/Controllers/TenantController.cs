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
        public async Task<IActionResult> PutTenant(Guid id, [FromBody] TenantTable tenantTable)
        {
            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return NotFound();
            }

            tenant.Company = tenantTable.Company;
            tenant.ContactEmail = tenantTable.ContactEmail;
            tenant.ContactName = tenantTable.ContactName;
            tenant.ContactPhone = tenant.ContactPhone;

            _context.Entry(tenant).State = EntityState.Modified;

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
      //  public async Task<ActionResult<TenantTable>> PostTenant(string company, string contactName, string contactPhone, string contactEmail)
        public async Task<ActionResult<TenantTable>> PostTenant([FromBody] TenantTable tenant)
        {

            TenantTable newTable = new TenantTable
            {
                TenantId = new Guid(),
                Company = tenant.Company,
                ContactName = tenant.ContactName,
                ContactEmail = tenant.ContactEmail,
                ContactPhone = tenant.ContactPhone,
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
