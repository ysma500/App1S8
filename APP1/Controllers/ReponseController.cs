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
        private readonly IAuthorization _auth;

        public ReponseController(ISondageReponse repo, ILogger<ReponseController> logger, IAuthorization auth)
        {
            _repo = repo;
            _logger = logger;
            _auth = auth;
        }
        
        //POST api/reponse
        [HttpPost()]
        public ActionResult PostReponse([FromHeader(Name = "Authorization-Token")] string token, Reponse reponses)
        {
            string form = Newtonsoft.Json.JsonConvert.SerializeObject(reponses);
            if ((_auth.ValidateToken(token)) && (String.CompareOrdinal(token,reponses.ClientId)==0))
            {
                try
                {
                    _repo.PostSondageReponse(reponses, token);

                    return Ok();
                }
                catch (NullReferenceException e)
                {
                    _logger.LogError(String.Format("Invalid form content using content: \"{0}\". Request was passed from IP: {1}. Error message: {2}", form, "localhost", e.ToString()));
                }
                catch(UnauthorizedAccessException e)
                {
                    _logger.LogError(String.Format("Person has already answered, invalid form content using content: \"{0}\". Request was passed from IP: {1}. Error message: {2}", form, "localhost", e.ToString()));
                }
                catch(Exception e)
                {
                    _logger.LogError(String.Format("Unkonwn error using content: \"{0}\". Request was passed from IP: {1}. Error message: {2}", form, "localhost", e.ToString()));
                }
            }
            else
            {
                _logger.LogError(String.Format("Bad Token or bad token matching in header and content using content: \"{0}\". Request was passed from IP: {1}.", form, "localhost"));
            }
            return BadRequest();
        }
    }
}
