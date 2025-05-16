using System;
using System.Net;

namespace TP_M2I_DOTNET.Services
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode statusCode, string message) 
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
