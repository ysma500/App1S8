using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class FilesystemAuthorization : IAuthorization
    {
        private readonly ILogger _logger;
        public FilesystemAuthorization(ILogger<FilesystemAuthorization> logger)
        {
            _logger = logger;
        }

        public bool ValidateToken(string token)
        {
            string path = "Database/Tokens.txt";
            if (!File.Exists(path))
            {
                //throw UnauthorizedException
                _logger.LogError("Response database dosen't exist (Database/Reponses.txt)");
                throw new NullReferenceException("File dosen't exist");
            }
            var tokens = File.ReadAllLines(path);
            foreach(string tok in tokens)
            {
                if(String.CompareOrdinal(tok, token) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
