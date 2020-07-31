
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("Hello World!");
        }

        // !!!DANGEROUS!!!
        // [HttpGet("jwt")]
        // public async Task<IActionResult> GetJwtAsync()
        // {
        //     string url = "https://cloud-run-auth-c2htxv5iwq-ew.a.run.app";
        //     if (url is null) return BadRequest("Could not get environment variable!");

        //     HttpClient client = _clientFactory.CreateClient();
        //     var request = new HttpRequestMessage
        //     {
        //         RequestUri = new System.Uri($"http://metadata/computeMetadata/v1/instance/service-accounts/default/identity?audience={url}"),
        //     };
        //     request.Headers.Add("Metadata-Flavor", "Google");
        //     HttpResponseMessage response = await client.SendAsync(request);
        //     string jwt = await response.Content.ReadAsStringAsync();
        //     return Ok(jwt);
        // }

        // [HttpGet("access")]
        // public async Task<IActionResult> GetAccessAsync()
        // {
        //     HttpClient client = _clientFactory.CreateClient();
        //     var request = new HttpRequestMessage
        //     {
        //         RequestUri = new System.Uri($"http://metadata.google.internal/computeMetadata/v1/instance/service-accounts/default/token"),
        //     };
        //     request.Headers.Add("Metadata-Flavor", "Google");
        //     HttpResponseMessage response = await client.SendAsync(request);
        //     string jwt = await response.Content.ReadAsStringAsync();
        //     return Ok(jwt);
        // }

        [HttpGet("request")]
        public async Task<IActionResult> GetRequestAsync()
        {
            HttpClient client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new System.Uri($"http://metadata.google.internal/computeMetadata/v1/instance/service-accounts/default/token?scopes=https://www.googleapis.com/auth/cloud-platform"),
            };
            request.Headers.Add("Metadata-Flavor", "Google");
            HttpResponseMessage response = await client.SendAsync(request);
            using Stream tokenStream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(tokenStream);
            string accessToken = json.RootElement.GetProperty("access_token").GetString();

            var apiRequestMessage = new HttpRequestMessage
            {
                RequestUri = new System.Uri("https://content-datastore.googleapis.com/v1/projects/helical-bonito-234716/operations"),
            };
            apiRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage apiResponse = await client.SendAsync(apiRequestMessage);
            string apiResponseContent = await apiResponse.Content.ReadAsStringAsync();

            return Ok(apiResponseContent);
        }

    } 
}
