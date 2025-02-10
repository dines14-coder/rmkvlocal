using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Demo.ApiClients
{
    public class ApiClientException: Exception
    {
        public ApiClientException(HttpResponseMessage response) : base(GenerateMessage(response))
        {
            Response = response;
        }

        private static string GenerateMessage(HttpResponseMessage response) =>
            $@"Request: {response.RequestMessage.Method} {response.RequestMessage.RequestUri}
            Response: {response.StatusCode}
            Content: {response.Content.ReadAsStringAsync().Result}";

        public HttpResponseMessage Response { get; }
    }
}
