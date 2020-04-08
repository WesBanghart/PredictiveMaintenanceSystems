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
    public class DataSourceController : ControllerBase
    {
        private readonly EFSystemContext _context;

        public DataSourceController(EFSystemContext context)
        {
            _context = context;
        }

        // GET: api/DataSource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataSourceTable>>> GetDataSources()
        {
            return await _context.DataSources.ToListAsync();
        }

        // GET: api/DataSource/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataSourceTable>> GetDataSourceTable(Guid id)
        {
            var dataSourceTable = await _context.DataSources.FindAsync(id);

            if (dataSourceTable == null)
            {
                return NotFound();
            }

            return dataSourceTable;
        }

        // PUT: api/DataSource/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataSourceTable(Guid id, string dataSourceName, string configuration, string connectionString, [FromQuery(Name = "modelIds")] List<Guid> modelIds = null)
        {
            var dataSource = await _context.DataSources.FindAsync(id);

            if (dataSource == null)
            {
                return NotFound();
            }

            if (dataSource.DataSourceId == id)
            {
                return BadRequest();
            }

            dataSource.DataSourceName = dataSourceName;
            dataSource.Configuration = configuration;
            dataSource.ConnectionString = connectionString;

            if (modelIds != null && modelIds.Count > 0)
            {
                foreach (var modelId in modelIds)
                {
                    if(!ModelInDataSource(dataSource, modelId))
                    {
                        var mdl = await _context.Models.FindAsync(modelId);
                        dataSource.Models.Add(mdl);
                        mdl.DataSources.Add(dataSource);
                    }
                }
            }

            _context.Entry(dataSource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataSourceTableExists(id))
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

        // POST: api/DataSource
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DataSourceTable>> PostDataSourceTable(string dataSourceName, string configuration, string connectionString, Guid userId, Guid tenantId, [FromQuery(Name = "modelIds")] List<Guid> modelIds = null)
        {
            // Check if User exists
            var userTable = await _context.Users.FindAsync(userId);
            if(userTable == null)
            {
                return NotFound();
            }

            //Check if Tenant exists
            var tenantTable = await _context.Tenants.FindAsync(tenantId);
            if(tenantTable == null)
            {
                return NotFound();
            }

            //Create new Datasource table
            DataSourceTable newDataSource = new DataSourceTable
            {
                DataSourceId = new Guid(),
                DataSourceName = dataSourceName,
                Configuration = configuration,
                ConnectionString = connectionString,
                UserId = userId,
                User = userTable,
                TenantId = tenantId,
                Tenant = tenantTable,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now                
            };

            List<ModelTable> modelTables = new List<ModelTable>();

            //Handle models
            if(modelIds != null && modelIds.Count > 0)
            {
                foreach (var guid in modelIds)
                {
                    var model = await _context.Models.FindAsync(guid);
                    if (model == null)
                    {
                        return NotFound($"Error: model with ID:{guid} not found.");
                    }
                    modelTables.Append(model);
                    model.DataSources.Append(newDataSource);
                }
            }

            newDataSource.Models = modelTables;

            //Add and save changes
            _context.DataSources.Add(newDataSource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDataSourceTable", new { id = newDataSource.DataSourceId }, newDataSource);
        }

        // DELETE: api/DataSource/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DataSourceTable>> DeleteDataSourceTable(Guid id)
        {
            var dataSourceTable = await _context.DataSources.FindAsync(id);
            if (dataSourceTable == null)
            {
                return NotFound();
            }

            _context.DataSources.Remove(dataSourceTable);
            await _context.SaveChangesAsync();

            return dataSourceTable;
        }

        private bool DataSourceTableExists(Guid id)
        {
            return _context.DataSources.Any(e => e.DataSourceId == id);
        }

        private bool ModelInDataSource(DataSourceTable dataSource, Guid modelId)
        {
            foreach (var model in dataSource.Models)
            {
                if (model.ModelId == modelId)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
