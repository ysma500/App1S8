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
        private readonly IAuthorization _auth;

        public SondagesController(ISondage repo, ILogger<SondagesController> logger, IAuthorization auth)
        {
            _repo = repo;
            _logger = logger;
            _auth = auth;
        }
        
        //GET api/sondages/{id}
        [HttpGet("{id}")]
        public IActionResult GetSondageById([FromHeader(Name = "Authorization-Token")] string token, int id)
        {
            if (_auth.ValidateToken(token)){
                try
                {
                    var var_sondage = _repo.GetSondageById(id);
                    if (var_sondage != null)
                    {
                        return Ok(var_sondage);
                    }
                    else
                    {
                        _logger.LogError(String.Format("Persistence layer exception with GetSondageById using index: \"{0}\". Request was passed from IP: {1}", id, this.HttpContext.Connection.RemoteIpAddress.ToString()));
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    _logger.LogError(String.Format("Invalid form index GetSondageById using index: \"{0}\". Request was passed from IP: {1}. Error message: {2}", id, this.HttpContext.Connection.RemoteIpAddress.ToString(), e.ToString()));
                }
                catch (Exception e)
                {
                    _logger.LogError(String.Format("Unknown error using index: \"{0}\". Request was passed from IP: {1}. Error message: {2}", id, this.HttpContext.Connection.RemoteIpAddress.ToString(), e.ToString()));
                }
            }
            else
            {
                _logger.LogError(String.Format("Bad Token error using token: \"{0}\". Request was passed from IP: {1}.", token, this.HttpContext.Connection.RemoteIpAddress.ToString()));
            }
            return BadRequest();
        }
    }
}
