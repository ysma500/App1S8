using APP1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class FilesystemSondage : ISondage
    {
        private readonly int MAX = 65536;
        public Sondage GetSondageById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sondage> GetSondages()
        {
            FileStream fileStream = new FileStream("Database/Sondages.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
            }

            string[] AllLines = new string[MAX]; //only allocate memory here
            using (StreamReader sr = File.OpenText("Database/Sondages.txt"))
            {
                int x = 0;
                while (!sr.EndOfStream)
                {
                    AllLines[x] = sr.ReadLine();
                    x += 1;
                }
            } //CLOSE THE FILE because we are now DONE with it.

            Parallel.For(0, AllLines.Length, x =>
            {
                //TestReadingAndProcessingLinesFromFile_DoStuff(AllLines[x]);
            });
            throw new NotImplementedException();
        }
    }
}
