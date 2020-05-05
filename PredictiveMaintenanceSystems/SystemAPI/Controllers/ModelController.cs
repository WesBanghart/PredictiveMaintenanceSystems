using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFDataModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServicesLibrary;
using ServicesLibrary.Interfaces;
using ServicesLibrary.Model.Run;
using ServicesLibrary.Model.Update;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// API Controller for Models
    /// </summary>
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        //System context
        private readonly EFSystemContext _context;
        //private readonly ServicesLibrary.Model.BackgroundTaskQueue backgroundTaskQueue;
        //Background task queue interface
        private readonly IBackgroundTaskQueue backgroundTaskQueue;
        //Model Option list
        private List<string> _modelOptions = new List<string> {"save", "saveandrun", "saveandtrain"};

        /// <summary>
        /// Constructor for the Model Controller.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="queue"></param>
        public ModelController(EFSystemContext context, IBackgroundTaskQueue queue)
        {
            _context = context;
            backgroundTaskQueue = queue;
        }

        /// <summary>
        /// Gets all Models.
        /// </summary>
        /// <returns></returns>
        // GET: api/Model
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelTable>>> GetModels()
        {
            return await _context.Models.ToListAsync();
        }

        /// <summary>
        /// Gets a Model Given a Model ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new Model and allows the user to run, train, or save the model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        // POST: api/Model
        [HttpPost("{option}")]
        public async Task<ActionResult<ModelTable>> PostModel([FromBody] ModelTable model, string option = "save")
        {
            option = option.Replace("\"", "");
            if (!_modelOptions.Contains(option))
            {
                return BadRequest("Invalid option: valid values are: \"save\", \"saveandrun\", \"saveandtrain\"");
            }
            //Check if Tenant and User Id exists
            if (await _context.Users.FindAsync(model.UserId) == null)
            {
                return NotFound();
            }

            //Create new Model object
            ModelTable newModel = new ModelTable
            {
                ModelName = model.ModelName,
                Configuration = model.Configuration,
                ModelId = new Guid(),
                File = null,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now,
                UserId = model.UserId
            };

            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();

            //Run the model if the option is selected
            if (option.Equals("saveandrun"))
            {
                try
                {
                    Task.Factory.StartNew((() =>
                    {
                        backgroundTaskQueue.QueueModelRunWorkItem(model.ModelId);
                    }));
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
                    Task.Factory.StartNew((() =>
                    {
                        backgroundTaskQueue.QueueModelUpdateWorkItem(model.ModelId);
                    }));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return CreatedAtAction("GetModel", new { id = newModel.ModelId }, newModel);
        }

        /// <summary>
        /// Updates an existing model. Allows the user to Run, Train, or save the updated Model.
        /// </summary>
        /// <param name="updatedModel"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        // PUT: api/Model/save
        [HttpPut("{option}")]
        public async Task<ActionResult<ModelTable>> PutModel([FromBody] ModelTable updatedModel, string option = "save")
        {
            option = option.Replace("\"", "");
            if (!_modelOptions.Contains(option))
            {
                return BadRequest("Invalid option: valid values are: \"save\", \"saveandrun\", \"saveandtrain\"");
            }

            //patch models
            var model = await _context.Models.FindAsync(updatedModel.ModelId);

            if (model == null)
            {
                return NotFound();
            }

            model.Configuration = updatedModel.Configuration;
            model.ModelName = updatedModel.ModelName;
            model.LastUpdated = DateTime.Now;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(model.ModelId))
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
                    Task.Factory.StartNew((() =>
                    {
                        backgroundTaskQueue.QueueModelRunWorkItem(model.ModelId);
                    }));
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
                    Task.Factory.StartNew((() =>
                    {
                        backgroundTaskQueue.QueueModelUpdateWorkItem(model.ModelId);
                    }));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a Model given a model ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
