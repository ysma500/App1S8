using APP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public interface ISondageReponse
    {
        void PostSondageReponse(Reponse reponse, string token);
    }
}
