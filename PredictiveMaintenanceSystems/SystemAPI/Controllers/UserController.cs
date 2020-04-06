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
    public class UserController : ControllerBase
    {
        private readonly EFSystemContext _context;

        public UserController(EFSystemContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTable>> GetUser(Guid id)
        {
            var userTable = await _context.Users.FindAsync(id);

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, string userName, string email, string firstName, string lastName)
        {
            if (UserTable(id))
            {
                return BadRequest($"User with {id} does not exist.");
            }

            var userTable = await _context.Users.FindAsync(id);

            userTable.UserName = userName;
            userTable.Email = email;
            userTable.FirstName = firstName;
            userTable.LastName = lastName;
            userTable.LastUpdate = DateTime.Now;

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTable(id))
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

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUser(string userName, string email, string firstName, string lastName, Guid tenantID)
        {
            var tenant = await _context.Tenants.FindAsync(tenantID);
            if (tenant == null)
            {
                return NotFound($"Tenant with ID: {tenantID} not found.");
            }
            UserTable newUser = new UserTable
            {
                UserId = new Guid(),
                TenantId = tenantID,
                Tenant = tenant,
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now,
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = newUser.UserId }, newUser);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserTable>> DeleteUser(Guid id)
        {
            var userTable = await _context.Users.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            var tenantTable = await _context.Tenants.FindAsync(userTable.TenantId);
            if(tenantTable == null)
            {
                return NotFound("Error: Assosiated Tentant Table not found.");
            }

            tenantTable.Users.Remove(userTable);

            _context.Users.Remove(userTable);
            await _context.SaveChangesAsync();

            return userTable;
        }

        private bool UserTable(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
