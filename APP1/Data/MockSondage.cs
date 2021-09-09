using APP1.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class MockSondage : ISondage
    {
        private readonly ILogger _logger;
        public MockSondage(ILogger<MockSondage> logger)
        {
            _logger = logger;
        }

        public Sondage GetSondageById(int id)
        {
            var var_sondage = this.GetSondages();
            try
            {
                List<Sondage> lst_sondage = var_sondage.ToList();
                int lst_length = lst_sondage.Count();
                if ((lst_length > id) && (id >= 0))
                {
                    return lst_sondage.ElementAt(id);
                }
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(String.Format("Encoutered ArgumentNull error in GetSondageById with index: \"{0}\". Error message: {1}", id, e.ToString()));
                return null;
            }
            catch (OverflowException e)
            {
                _logger.LogError(String.Format("Encoutered Overflow error in GetSondageById with index: \"{0}\". Error message: {1}", id, e.ToString()));
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(String.Format("Encoutered UnknownError error in GetSondageById with index: \"{0}\". Error message: {1}", id, e.ToString()));
                return null;
            }
            throw new IndexOutOfRangeException(String.Format("Invalid form index GetSondageById using index: \"{0}\".", id));
        }

        public IEnumerable<Sondage> GetSondages()
        {
            var lstSondage = new List<Sondage>
            {
                new Sondage { JsonString = "This is a string1" },
                new Sondage { JsonString = "This is a string2" },
                new Sondage { JsonString = "This is a string3" }
            };

            return lstSondage;
        }
    }
}
