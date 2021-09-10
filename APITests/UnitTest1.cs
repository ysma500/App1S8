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
        private readonly SondagesController _sondageCont;
        private readonly ReponseController _reponseCont;
        private readonly string validToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IkplYW4tQ2hyaXN0b3BoZSBCbGFpcy1DcmVwZWF1IiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.uBTKFtC62sAlnVLg1ATZaFrzwgSPWA9DIWlepRkYD02XFCH7GE6fTd1CfjcKCbXrm56zMudVeg4c3Y77W5Xy3Q";
        private readonly string invalidToken = "ey";

        public UnitTest1()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<MockAuthorization> logger_Auth = loggerFactory.CreateLogger<MockAuthorization>();
            ILogger<MockSondage> logger_Sondage = loggerFactory.CreateLogger<MockSondage>();
            ILogger<MockSondageReponse> logger_Reponse = loggerFactory.CreateLogger<MockSondageReponse>();
            ILogger<SondagesController> logger_SondageCont = loggerFactory.CreateLogger<SondagesController>();
            ILogger<ReponseController> logger_ReponseCont = loggerFactory.CreateLogger<ReponseController>();
            _auth = new MockAuthorization(logger_Auth);
            _sondage = new MockSondage(logger_Sondage);
            _reponse = new MockSondageReponse(logger_Reponse);
            _sondageCont = new SondagesController(_sondage,logger_SondageCont,_auth);
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
            Assert.Throws<IndexOutOfRangeException>(() => _sondage.GetSondageById(0));

            Assert.Equal("This is a string1", sondage1.JsonString);
            Assert.Equal("This is a string2", sondage2.JsonString);
        }

        [Fact]
        public void TestGetContent()
        {
            // Vérifier si le contenu des songages est bon
            var sondage1 = _sondage.GetSondageById(1).JsonString;
            var sondage2 = _sondage.GetSondageById(2).JsonString;

            var sondageJson1 = "This is a string1";
            var sondageJson2 = "This is a string2";

            Assert.Equal(sondage1, sondageJson1);
            Assert.Equal(sondage2, sondageJson2);
        }

        [Fact]
        public void TestGetSondageAuth()
        {
            int validId = 1;
            var sondageTokenValide = _sondageCont.GetSondageById(validToken, validId);
            var sondageTokenInvalide = _sondageCont.GetSondageById(invalidToken, validId);

            Assert.IsType<OkObjectResult>(sondageTokenValide);
            Assert.IsType<BadRequestResult>(sondageTokenInvalide);
        }

        [Fact]
        public void TestGetSondageContent()
        {
            int validId = 1;
            int invalidId = -1;

            var sondageIdValide = _sondageCont.GetSondageById(validToken, validId);
            var sondageIdInvalide = _sondageCont.GetSondageById(validToken, invalidId);

            Assert.IsType<OkObjectResult>(sondageIdValide);
            Assert.IsType<BadRequestResult>(sondageIdInvalide);
        }

        [Fact]
        public void TestPostReponseAuth()
        {
            // Vérifier si l'authentification fonctionne
            QuestionReponse reponsesQ = new QuestionReponse { question = "q1", reponse = "a" };
            List<QuestionReponse> reponseList = new List<QuestionReponse>();
            reponseList.Add(reponsesQ);
            Reponse reponseValide = new Reponse { SondageId = 1, ClientId = validToken, reponses = reponseList };
            Reponse reponseValideTokenInvalide = new Reponse { SondageId = 1, ClientId = invalidToken, reponses = reponseList };
            Reponse reponseInvalide = new Reponse { SondageId = 1, ClientId = invalidToken, reponses = reponseList};

            var reponseTokenValide = _reponseCont.PostReponse(validToken, reponseValide) ;
            var reponseTokenInvalide = _reponseCont.PostReponse(invalidToken, reponseValideTokenInvalide);
            var reponseTokenSecond = _reponseCont.PostReponse(validToken, reponseInvalide) ;

            Assert.IsType<OkResult>(reponseTokenValide);
            Assert.IsType<BadRequestResult>(reponseTokenInvalide);
            Assert.IsType<BadRequestResult>(reponseTokenSecond);
        }
    }
}
