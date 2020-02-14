using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;


namespace GW_Api.Bindings
{
    [ApiController]
    public class GW_api: ControllerBase
    {
        private IHttpClientFactory _clientFactory;

        public static string AuthDomain = null;
        public static string AuthUrl = null;


        public GW_api(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Route("{permission}/{service}/{url}")]
        [HttpGet]
        public void Get(string permission, string service, string url)
        {
            string answer;
            if (CkeckPermission(permission).Result)
            {
                answer = "good";
            }
            else
            {
                answer = "bad";
            }
            Response.WriteAsync(answer);
        }

        private async Task<bool> CkeckPermission(string permission)
        {
            StringValues token;
            if (Request.Headers.TryGetValue("Authorization", out token))
            {
                string url = AuthDomain + "/" + AuthUrl + "/" + permission;

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token.ToString());

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        private async Task<string> RedirectRequest(string method, string permission, string service, string url)
        {
            string domain = "domain"; //need to create config file where all services domains are registered, save it as map and retrieve here by service
            string fullUrl = domain + url;
            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            foreach (var header in Request.Headers)
            {
                if (header.Key != "Authorization")
                {
                    request.Headers.Add(header.Key, header.Value.ToString());
                }
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // tmp, need to return actual data received from service.
            return "Redirection succeeded";
        }
    }
}
