using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public interface IAuthorization
    {
        bool ValidateToken(string token);
    }
}
