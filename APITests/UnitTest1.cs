using System;
using Xunit;
using APP1.Controllers;
using APP1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace APITests
{
    public class UnitTest1 : IDisposable
    {
        private readonly IAuthorization _auth;
        private readonly ISondage _sondage;
        private readonly ISondageReponse _reponse;

        public UnitTest1()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<MockAuthorization> logger_Auth = loggerFactory.CreateLogger<MockAuthorization>();
            ILogger<MockSondage> logger_Sondage = loggerFactory.CreateLogger<MockSondage>();
            ILogger<MockSondageReponse> logger_Reponse = loggerFactory.CreateLogger<MockSondageReponse>();
            _auth = new MockAuthorization(logger_Auth);
            _sondage = new MockSondage(logger_Sondage);
            _reponse = new MockSondageReponse(logger_Reponse);
        }
        public void Dispose()
        {
            
        }

        [Fact]
        public void Test1()
        {
            Assert.Equal(4, 4);
        }

        [Theory]
        [InlineData("patate")]
        [InlineData("fuck you")]
        [InlineData("ta mere")]
        public void Test2(String str)
        {
            Assert.True(str.Length == 6);
        }
    }
}
