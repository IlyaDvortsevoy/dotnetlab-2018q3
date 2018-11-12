using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MLA_task.BLL.Interface;
using MLA_task.BLL.Interface.Models;
using MLA_task.BLL.Interface.Exceptions;
using NLog;

namespace MLA_task.Controllers
{
    public class DemoController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IDemoModelService _demoModelService;

        public DemoController(ILogger logger, IDemoModelService demoModelService)
        {
            _logger = logger;
            _demoModelService = demoModelService;
        }

        public async Task<IHttpActionResult> Get()
        {
            _logger.Info("receiving all items");

            try
            {
                var models = await _demoModelService.GetAllDemoModelsAsync();

                return Ok(models.Select(model => new
                {
                    Id = model.Id,
                    Name = model.Name,
                    Created = model.Created,
                    Modified = model.Modified,
                    CommonInfo = model.CommonInfo
                }));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Server error occured while trying to get all items");
                return this.InternalServerError(ex);
            }
        }
        
        public async Task<IHttpActionResult> Get(int id)
        {
            _logger.Info($"receiving item with id {id}");

            try
            {
                var model = await _demoModelService.GetDemoModelByIdAsync(id);

                _logger.Info($"item with id {id} has been received.");

                return Ok(model);
            }
            catch (DemoServiceException ex)
            {
                _logger.Info(ex, $"Wrong ID {id} has been requested");
                return this.BadRequest("Wrong ID");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Server error occured while trying to get item with id {id}");
                return this.InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> Post([FromBody]DemoModel model)
        {
            _logger.Info($"adding model with name {model.Name}");

            if (model.Name == "bla-bla")
            {
                _logger.Info($"Wrong model name {model.Name} detected");
                return this.BadRequest("WrongName");
            }

            try
            {
                await _demoModelService.AddDemoModelAsync(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Server error occured while trying to add item with name {model.Name}");
                return this.InternalServerError();
            }

            return Ok();
        }
    }
}