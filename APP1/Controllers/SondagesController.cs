using APP1.Data;
using APP1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Controllers
{
    [Route("api/sondages")]
    [ApiController]
    public class SondagesController : ControllerBase
    {
        private readonly ISondage _repo;

        private readonly ILogger _logger;

        public SondagesController(ISondage repo, ILogger<SondagesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        //GET api/sondages/{id}
        [HttpGet("{id}")]
        public ActionResult <Sondage> GetSondageById(int id)
        {
            //String this.HttpContext.Request.Headers.HeaderAuthorization; 
            try
            {
                var var_sondage = _repo.GetSondageById(id);
                if (var_sondage != null)
                {
                    return Ok(var_sondage);
                } else
                {
                    _logger.LogError(String.Format("Persistence layer exception with GetSondageById using index: \"{0}\". Request was passed from IP: {1}", id, "localhost"));
                }
            }
            catch(IndexOutOfRangeException e)
            {
                _logger.LogError(String.Format("Invalid form index GetSondageById using index: \"{0}\". Request was passed from IP: {1}. Error message: {2}", id, "localhost", e.ToString()));
            }
            catch(Exception e)
            {
                _logger.LogError(String.Format("Unknown error using index: \"{0}\". Request was passed from IP: {1}. Error message: {2}", id, "localhost", e.ToString()));
            }
            return BadRequest();
        }
    }
}
