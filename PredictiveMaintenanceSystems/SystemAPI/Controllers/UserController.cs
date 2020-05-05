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
    /// API Controller for Users
    /// </summary>
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //System Context
        private readonly EFSystemContext _context;

        /// <summary>
        /// Constructor for the User controller
        /// </summary>
        /// <param name="context"></param>
        public UserController(EFSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all current Users.
        /// </summary>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Gets a user given a User ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates an existing user given the UserID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedUser"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, [FromBody] UserTable updatedUser)
        {

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest($"Error: User with {id} does not exist.");
            }


            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.LastUpdate = DateTime.Now;

            _context.Entry(user).State = EntityState.Modified;

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

        /// <summary>
        /// Creates a new User.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUser([FromBody] UserTable newUser)
        {
            var tenant = await _context.Tenants.FindAsync(newUser.TenantId);
            if (tenant == null)
            {
                return NotFound($"Tenant with ID: {newUser.TenantId} not found.");
            }
            UserTable user = new UserTable
            {
                UserId = new Guid(),
                TenantId = newUser.TenantId,
                Tenant = newUser.Tenant,
                UserName = newUser.UserName,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = newUser.UserId }, newUser);
        }

        /// <summary>
        /// Deletes a user given a User ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return NotFound("Error: Associated Tenant Table not found.");
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
