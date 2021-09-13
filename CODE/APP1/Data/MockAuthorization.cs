using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP1.Data
{
    public class MockAuthorization : IAuthorization
    {
        private readonly ILogger _logger;
        List<string> tokens = new List<string>();
        public MockAuthorization(ILogger<MockAuthorization> logger)
        {
            _logger = logger;
            //Header: {"alg": "HS512", "typ": "JWT" } Payload: { "sub": "App1", "name": "Jean-Christophe Blais-Crepeau", "admin": true, "iat": 1516239022 } Signature: MACSHA512(base64UrlEncode(header) + "." +base64UrlEncode(payload),your-512-bit-secret)
            tokens.Add("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IkplYW4tQ2hyaXN0b3BoZSBCbGFpcy1DcmVwZWF1IiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.uBTKFtC62sAlnVLg1ATZaFrzwgSPWA9DIWlepRkYD02XFCH7GE6fTd1CfjcKCbXrm56zMudVeg4c3Y77W5Xy3Q");
            //Header: {"alg": "HS512", "typ": "JWT" } Payload: { "sub": "App1", "name": "Ysmael Fortier", "admin": true, "iat": 1516239022 } Signature: MACSHA512(base64UrlEncode(header) + "." +base64UrlEncode(payload),your-512-bit-secret)
            tokens.Add("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBcHAxIiwibmFtZSI6IllzbWFlbCBGb3J0aWVyIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.aoXbEjNLcO5sJTC_-nVd-dyxeh9vZey_tjUGjQ3Fw64y9qkz8ujsrdZCLbeVgxoY-W_Dib-zaKqE-qAvk1xGVg");
        }

        public bool ValidateToken(string token)
        {
            foreach(string s in tokens)
            {
                if (String.Compare(token, s, StringComparison.Ordinal) == 0)
                {
                    _logger.LogInformation(String.Format("Valid token: {0}", token));
                    return true;
                }
            }
            _logger.LogInformation(String.Format("Invalid token: {0}", token));
            return false;
        }
    }
}
