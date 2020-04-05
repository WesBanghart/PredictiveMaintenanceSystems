using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServicesLibrary;
using ServicesLibrary.Interfaces;
using ServicesLibrary.Model.Run;
using ServicesLibrary.Model.Update;

namespace SystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly EFSystemContext _context;
        //private readonly ServicesLibrary.Model.BackgroundTaskQueue backgroundTaskQueue;
        private readonly IBackgroundTaskQueue backgroundTaskQueue;
        private List<string> _modelOptions = new List<string> {"save", "saveandrun", "saveandtrain"};

        public ModelController(EFSystemContext context, IBackgroundTaskQueue queue)
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

        // Note: valid option values should be "save", "saveandrun", "saveandtrain" - default is "save"
        // POST: api/Model
        [HttpPost]
        public async Task<ActionResult<ModelTable>> PostModel(string modelName, string configuration, Guid userId, Guid tenantId, string option = "save" , List<Guid> dataSourceIdList = null)
        {
            option = option.Replace("\"", "");
            if (!_modelOptions.Contains(option))
            {
                return BadRequest("Invalid option: valid values are: \"save\", \"saveandrun\", \"saveandtrain\"");
            }
            //Check if Tenant and User Id exists
            if (await _context.Users.FindAsync(userId) == null)
            {
                return NotFound();
            }
            if (await _context.Tenants.FindAsync(tenantId) == null)
            {
                return NotFound();
            }

            List<DataSourceTable> dataSources = new List<DataSourceTable>();

            //Handle data sources
            if (dataSourceIdList != null && dataSourceIdList.Count > 0)
            {
                foreach (var guid in dataSourceIdList)
                {
                    var dataSource = await _context.DataSources.FindAsync(guid);
                    //collect data sources
                    if (dataSource == null)
                    {
                        return NotFound();
                    }
                    dataSources.Append(dataSource);
                }
            }

            //Create new Model object
            ModelTable newModel = new ModelTable
            {
                ModelName = modelName,
                Configuration = configuration,
                ModelId = new Guid(),
                File = null,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now,
                DataSources = dataSources
            };
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();

            //Run the model if the option is selected
            if (option.Equals("saveandrun"))
            {
                try
                {
                    backgroundTaskQueue.QueueModelRunWorkItem(newModel.ModelId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //Train the model if the option is selected
            if (option.Equals("saveandtrain"))
            {
                try
                {
                    backgroundTaskQueue.QueueModelUpdateWorkItem(newModel.ModelId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return CreatedAtAction("GetModel", new { id = newModel.ModelId }, newModel);
        }

        // Note: valid option values should be "save", "saveandrun", "saveandtrain" - default is "save"
        // PUT: api/Model/5
        [HttpPut]
        public async Task<ActionResult<ModelTable>> PutModel(Guid id, string configuration, string option = "save")
        {
            option = option.Replace("\"", "");
            if (!_modelOptions.Contains(option))
            {
                return BadRequest("Invalid option: valid values are: \"save\", \"saveandrun\", \"saveandtrain\"");
            }

            //patch models

            var model = await _context.Models.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }


            model.Configuration = configuration;
            model.LastUpdated = DateTime.Now;

            _context.Entry(model).State = EntityState.Modified;

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
                    return BadRequest("Error encountered while saving changes.");
                    throw;
                }
            }

            //Run the model if the option is selected
            if (option.Equals("saveandrun"))
            {
                try
                {
                    backgroundTaskQueue.QueueModelRunWorkItem(model.ModelId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //Train the model if the option is selected
            if (option.Equals("saveandtrain"))
            {
                try
                {
                    backgroundTaskQueue.QueueModelUpdateWorkItem(model.ModelId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
