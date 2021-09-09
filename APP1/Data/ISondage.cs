using APP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public interface ISondage
    {
        IEnumerable<Sondage> GetSondages();
        Sondage GetSondageById(int id);
    }
}
