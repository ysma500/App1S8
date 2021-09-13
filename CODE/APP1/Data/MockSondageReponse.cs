using APP1.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class MockSondageReponse : ISondageReponse
    {
        private readonly ILogger _logger;
        public MockSondageReponse(ILogger<MockSondageReponse> logger)
        {
            _logger = logger;
        }

        public void PostSondageReponse(Reponse reponse, string token)
        {
            if (reponse != null)
            {
                _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(reponse));
            }
            else
            {
                throw new NullReferenceException(String.Format("Le sondage est vide"));
            }
            
        }
    }
}
