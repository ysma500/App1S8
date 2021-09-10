using System;
using Xunit;
using APP1.Controllers;
using APP1.Data;
using APP1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;

namespace APITests
{
    public class UnitTest1 : IDisposable
    {
        private readonly IAuthorization _auth;
        private readonly ISondage _sondage;
        private readonly ISondageReponse _reponse;
        private readonly ReponseController _reponseCont;

        public UnitTest1()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<MockAuthorization> logger_Auth = loggerFactory.CreateLogger<MockAuthorization>();
            ILogger<MockSondage> logger_Sondage = loggerFactory.CreateLogger<MockSondage>();
            ILogger<MockSondageReponse> logger_Reponse = loggerFactory.CreateLogger<MockSondageReponse>();
            ILogger<ReponseController> logger_ReponseCont = loggerFactory.CreateLogger<ReponseController>();
            _auth = new MockAuthorization(logger_Auth);
            _sondage = new MockSondage(logger_Sondage);
            _reponse = new MockSondageReponse(logger_Reponse);
            _reponseCont = new ReponseController(_reponse,logger_ReponseCont,_auth);
        }
        public void Dispose()
        {
            
        }

        [Fact]
        public void TestGetIndex()
        {
            // Vérifier si envoyer l'index du songage fonctionne correctement
            var sondage1 = _sondage.GetSondageById(1);
            var sondage2 = _sondage.GetSondageById(2);
            var badSondage = _sondage.GetSondageById(0);

            Assert.IsType<OkObjectResult>(sondage1);
            Assert.IsType<OkObjectResult>(sondage2);
            Assert.IsType<BadRequestResult>(badSondage);
        }

        [Fact]
        public void TestGetContent()
        {
            // Vérifier si le contenu des songages est bon
            var sondage1 = _sondage.GetSondageById(1).JsonString;
            var sondage2 = _sondage.GetSondageById(2).JsonString;

            var sondageJson1 = "Sondage 1:\r\n1. À quelle tranche d'âge appartenez-vous? a:0-25 ans, b:25-50 ans, c:50-75 ans, d:75 ans et plus\r\n2. Êtes-vous une femme ou un homme? a:Femme, b:Homme, c:Je ne veux pas répondre\r\n3. Quel journal lisez-vous à la maison? a:La Presse, b:Le Journal de Montréal, c:The Gazette, d:Le Devoir\r\n4. Combien de temps accordez-vous à la lecture de votre journal quotidiennement? a:Moins de 10 minutes; b:Entre 10 et 30 minutes, c:Entre 30 et 60 minutes, d:60 minutes ou plus";
            var sondageJson2 = "Sondage 2:\r\n1. À quelle tranche d'âge appartenez-vous? a:0-25 ans, b:25-50 ans, c:50-75 ans, d:75 ans et +\r\n2. Êtes-vous une femme ou un homme? a:Femme, b:Homme, c:Je ne veux pas répondre\r\n3. Combien de tasses de café buvez-vous chaque jour? a:Je ne bois pas de café, b:Entre 1 et 5 tasses, c:Entre 6 et 10 tasses, d:10 tasses ou plus\r\n4. Combien de consommations alcoolisées buvez-vous chaque jour? a:0, b:1, c:2 ou 3, d:3 ou plus";

            Assert.Equal(sondage1, sondageJson1);
            Assert.Equal(sondage2, sondageJson2);
        }

        [Fact]
        public void TestPostAuth()
        {
            // Vérifier si l'authentification fonctionne
            QuestionReponse reponsesQ = new QuestionReponse { question = "q1", reponse = "a" };
            List<QuestionReponse> reponseList = new List<QuestionReponse>();
            reponseList.Add(reponsesQ);
            Reponse reponseValide = new Reponse { SondageId = 1, ClientId = "0", reponses = reponseList };
            Reponse reponseInvalide = new Reponse { SondageId = 1, ClientId = "1", reponses = reponseList};

            var reponseTokenValide = _reponseCont.PostReponse("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IkplYW4tQ2hyaXN0b3BoZSBCbGFpcy1DcmVwZWF1IiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.uBTKFtC62sAlnVLg1ATZaFrzwgSPWA9DIWlepRkYD02XFCH7GE6fTd1CfjcKCbXrm56zMudVeg4c3Y77W5Xy3Q", reponseValide) ;
            var reponseTokenInvalide = _reponseCont.PostReponse("ey", reponseValide);
            var reponseTokenSecond = _reponseCont.PostReponse("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IkplYW4tQ2hyaXN0b3BoZSBCbGFpcy1DcmVwZWF1IiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.uBTKFtC62sAlnVLg1ATZaFrzwgSPWA9DIWlepRkYD02XFCH7GE6fTd1CfjcKCbXrm56zMudVeg4c3Y77W5Xy3Q", reponseInvalide) ;

            Assert.IsType<OkObjectResult>(reponseTokenValide);
            Assert.IsType<OkObjectResult>(reponseTokenInvalide);
            Assert.IsType<BadRequestResult>(reponseTokenSecond);
        }

        [Fact]
        public void TestPostContent()
        {
            // Vérifier si la validation du contenu fonctionne
            Reponse reponseInvalide = null;

            var reponseContentValide = _reponseCont.PostReponse("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IkplYW4tQ2hyaXN0b3BoZSBCbGFpcy1DcmVwZWF1IiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.uBTKFtC62sAlnVLg1ATZaFrzwgSPWA9DIWlepRkYD02XFCH7GE6fTd1CfjcKCbXrm56zMudVeg4c3Y77W5Xy3Q", reponseInvalide);
            var reponseContentInvalide = _reponseCont.PostReponse("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IkplYW4tQ2hyaXN0b3BoZSBCbGFpcy1DcmVwZWF1IiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.uBTKFtC62sAlnVLg1ATZaFrzwgSPWA9DIWlepRkYD02XFCH7GE6fTd1CfjcKCbXrm56zMudVeg4c3Y77W5Xy3Q", reponseInvalide);

            Assert.IsType<OkObjectResult>(reponseContentValide);
            Assert.IsType<OkObjectResult>(reponseContentInvalide);
        }

        [Theory]
        [InlineData("patate")]
        [InlineData("rutabaga")]
        [InlineData("patate douce")]
        public void Test2(String str)
        {
            Assert.True(str.Length == 6);
        }
    }
}
