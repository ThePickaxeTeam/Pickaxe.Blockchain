using Newtonsoft.Json;
using Pickaxe.Blockchain.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Clients
{
    public class NodeClient : INodeClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public NodeClient(string baseUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = new TimeSpan(0, 0, 3);
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<Response<MiningJob>> GetMiningJob(string minerAddress)
        {
            // Create a GET request to Node
            var requestMessage = BuildRequestMessage(HttpMethod.Get, $"api/miningjobs/{minerAddress}");
            return await SendRequest<MiningJob>(requestMessage).ConfigureAwait(false);
        }

        public async Task<Response<Block>> SubmitMiningJob(MiningJobResult result, string minerAddress)
        {
            // Create a POST request to Node
            var requestMessage = BuildRequestMessage(HttpMethod.Post, $"api/miningjobs/{minerAddress}");
            requestMessage.Content = SerializeRequestAsJson(result);
            return await SendRequest<Block>(requestMessage).ConfigureAwait(false);
        }

        private static StringContent SerializeRequestAsJson<T>(T request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }

        private HttpRequestMessage BuildRequestMessage(HttpMethod httpMethod, string location, string queryString = "")
        {
            var requestMessage = new HttpRequestMessage(httpMethod, string.Format("{0}/{1}{2}", _baseUrl, location, queryString));
            requestMessage.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return requestMessage;
        }

        private async Task<Response<TPayload>> SendRequest<TPayload>(HttpRequestMessage request)
        {
            try
            {
                HttpResponseMessage responseMessage =
                    await _httpClient.SendAsync(request).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    TPayload payload;
                    if (responseMessage.Content == null)
                    {
                        payload = default(TPayload);
                    }
                    else
                    {
                        string responseContent =
                            await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                        payload = JsonConvert.DeserializeObject<TPayload>(responseContent);
                    }

                    var response = new Response<TPayload>
                    {
                        Status = Status.Success,
                        Payload = payload
                    };

                    return response;
                }

                return new Response<TPayload>
                {
                    Status = Status.Failed,
                    Errors = new[]
                    {
                        $"An error occurred while sending {request.Method} request to {request.RequestUri}."
                    }
                };
            }
            catch (Exception ex)
            {
                IList<string> errors = new List<string>
                {
                    $"An error occurred while accessing Node API at {request.RequestUri}: {ex.Message}"
                };

                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    errors.Add(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                return new Response<TPayload>
                {
                    Status = Status.Failed,
                    Errors = errors
                };
            }
        }
    }
}
