using APP1.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class FilesystemSondage : ISondage
    {
        private readonly ILogger _logger;
        public FilesystemSondage(ILogger<FilesystemSondage> logger)
        {
            _logger = logger;
        }
        public Sondage GetSondageById(int id)
        {
            List<Sondage> AllSondages = this.GetSondages();
            try
            {
                int lst_length = AllSondages.Count();
                if ((lst_length >= id) && (id > 0))
                {
                    return AllSondages.ElementAt(id-1);
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

        public List<Sondage> GetSondages()
        {
            List<Sondage> SondageList = new List<Sondage>();
            List<string> SondageLines = new List<string>();

            try
            {
                using (FileStream fs = File.Open("Database/Sondages.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s != string.Empty)
                        {
                            SondageLines.Add(s);
                        }
                        else
                        {
                            SondageList.Add(new Sondage { JsonString = String.Join("\r\n", SondageLines) });
                            SondageLines = new List<string>();
                        }
                    }
                    SondageList.Add(new Sondage { JsonString = String.Join("\r\n", SondageLines) });
                }

                return SondageList;
            }
            catch(OutOfMemoryException e)
            {
                _logger.LogError(String.Format("Encoutered OutOfMemoryException error in GetSondages. Error message: {0}", e.ToString()));
            }
            catch(IOException e)
            {
                _logger.LogError(String.Format("Encoutered IOException error in GetSondages. Error message: {0}", e.ToString()));
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(String.Format("Encoutered ArgumentNullException error in GetSondages. Error message: {0}", e.ToString()));
            }
            catch (ArgumentException e)
            {
                _logger.LogError(String.Format("Encoutered ArgumentException error in GetSondages. Error message: {0}", e.ToString()));
            }
            catch(Exception e)
            {
                _logger.LogError(String.Format("Encoutered UnknownError error in GetSondages. Error message: {0}", e.ToString()));
            }
            return null;
        }
    }
}
