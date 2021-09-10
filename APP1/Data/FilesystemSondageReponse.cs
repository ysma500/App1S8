using APP1.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class FilesystemSondageReponse : ISondageReponse
    {
        private readonly ILogger _logger;
        public FilesystemSondageReponse(ILogger<FilesystemSondageReponse> logger)
        {
            _logger = logger;
        }
        public void PostSondageReponse(Reponse reponse, string token)
        {
            string path = "Database/Reponses.txt";
            if (!File.Exists(path))
            {
                //throw UnauthorizedException
                _logger.LogError("Response database dosen't exist (Database/Reponses.txt)");
                throw new NullReferenceException("File dosen't exist");
            }
            //Read answer file
            string jsonStr = String.Join("", File.ReadAllLines(path));
            var lst_reponses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reponse>>(jsonStr);
            foreach(Reponse rep in lst_reponses)
            {
                if(String.CompareOrdinal(rep.ClientId,token)==0)
                {
                    _logger.LogInformation("Token identique: {0}", token);
                    if (rep.SondageId == reponse.SondageId)
                    {
                        _logger.LogError("Sondage was already answered by user: {0}", token);
                        throw new UnauthorizedAccessException("Sondage was already answered by user");
                    }
                }
            }
            //if (not in use)
            lst_reponses.Add(reponse);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(lst_reponses);
            //open file to write
            File.WriteAllText(path, json);
        }
    }
}
