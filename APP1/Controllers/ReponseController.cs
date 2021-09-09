using APP1.Data;
using APP1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Controllers
{
    [Route("api/reponse")]
    [ApiController]
    public class ReponseController : ControllerBase
    {
        private readonly ISondageReponse _repo;

        private readonly ILogger _logger;

        public ReponseController(ISondageReponse repo, ILogger<ReponseController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        //POST api/reponse
        [HttpPost()]
        public async Task<ActionResult> PostReponseAsync()
        {
            try
            {
                Request.EnableBuffering();
                var rawRequestBody = await new StreamReader(Request.Body).ReadToEndAsync();

                //faire appel
                _repo.PostSondageReponse(new Reponse { JsonString = rawRequestBody.ToString() });
                
                return Ok();
            }
            catch(NullReferenceException e)
            {
                _logger.LogError(String.Format("Invalid form content using content: \"{0}\". Request was passed from IP: {1}. Error message: {2}", "CoNtEnT", "localhost", e.ToString()));
            }
            return BadRequest();
        }
    }
}
