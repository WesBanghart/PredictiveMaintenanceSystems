using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;
using Microsoft.AspNetCore.Cors;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// Controller for Datasources
    /// </summary>
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataSourceController : ControllerBase
    {
        //System Context
        private readonly EFSystemContext _context;

        /// <summary>
        /// DataSource controller constructor
        /// </summary>
        /// <param name="context"></param>
        public DataSourceController(EFSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets All Data Sources/
        /// </summary>
        /// <returns></returns>
        // GET: api/DataSource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataSourceTable>>> GetDataSources()
        {
            return await _context.DataSources.ToListAsync();
        }

        /// <summary>
        /// Gets A Data Source given a Data Source ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates a Datasource given a Data Source ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataSourceTable"></param>
        /// <returns></returns>
        // PUT: api/DataSource/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataSourceTable(Guid id, [FromBody] DataSourceTable dataSourceTable)
        {
            var dataSource = await _context.DataSources.FindAsync(id);

            if (dataSource == null)
            {
                return NotFound();
            }

            dataSource.DataSourceName = dataSourceTable.DataSourceName;
            dataSource.Configuration = dataSourceTable.Configuration;
            dataSource.IsStreaming = dataSourceTable.IsStreaming;
            dataSource.ConnectionString = dataSourceTable.ConnectionString;
            dataSource.LastUpdated = DateTime.Now;

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

        /// <summary>
        /// Creates a new Data Source.
        /// </summary>
        /// <param name="dataSourceTable"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        // POST: api/DataSource
        [HttpPost]
        public async Task<ActionResult<DataSourceTable>> PostDataSourceTable([FromBody] DataSourceTable dataSourceTable, [FromForm]IFormFile body)
        {
            // Check if User exists
            var userTable = await _context.Users.FindAsync(dataSourceTable.UserId);
            if(userTable == null)
            {
                return NotFound();
            }

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            //Create new Datasource table
            DataSourceTable newDataSource = new DataSourceTable
            {
                DataSourceId = new Guid(),
                DataSourceName = dataSourceTable.DataSourceName,
                Configuration = dataSourceTable.Configuration,
                ConnectionString = dataSourceTable.ConnectionString,
                UserId = dataSourceTable.UserId,
                IsStreaming = dataSourceTable.IsStreaming,
                User = userTable,
                File = fileBytes,
                FileName = body.FileName,
                FileContentDisposition = body.ContentDisposition,
                FileContentType = body.ContentType,
                FileLength = body.Length,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now                
            };

            //Add and save changes
            _context.DataSources.Add(newDataSource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDataSourceTable", new { id = newDataSource.DataSourceId }, newDataSource);
        }

        /// <summary>
        /// Uploads a file to an existing Data Source.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UploadDataSourceFile(Guid id, [FromForm] IFormFile body)
        {
            var dataSourceTable = await _context.DataSources.FindAsync(id);
            if (dataSourceTable == null)
            {
                return NotFound($"Could Not Find datasource with ID: {id}");
            }

            try
            {
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await body.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                dataSourceTable.File = fileBytes;
                dataSourceTable.FileContentDisposition = body.ContentDisposition;
                dataSourceTable.FileContentType = body.ContentType;
                dataSourceTable.FileLength = body.Length;
                dataSourceTable.FileName = body.FileName;
                dataSourceTable.LastUpdated = DateTime.Now;

                _context.Entry(dataSourceTable).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Error reading file: {e}");
            }

            return Ok();
        }

        /// <summary>
        /// Deletes a DataSource given a Data Source ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        public class FileFeature
        {
            public enum FileType
            {
                ZIP,
                CSV
            }
            public string Path { get; set; }
           // public FileType Type { get; set; }
        }

    }
}
