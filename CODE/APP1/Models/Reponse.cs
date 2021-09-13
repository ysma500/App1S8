using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Models
{
    public class Reponse
    {
        public int SondageId { get; set; }
        public string ClientId { get; set; }
        public List<QuestionReponse> reponses { get; set; }
    }

    public class QuestionReponse
    {
        public string question { get; set; }
        public string reponse { get; set; }
    }
}

