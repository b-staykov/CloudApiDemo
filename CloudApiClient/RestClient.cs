using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CloudApiClient
{
    /*
     * <summary>
     * Simple Restful API client
     * Send async requests to REST API endpoint with json serialzed parameter
     * </summary>
     */
    public class RestClient<T>
    {        
        private static readonly HttpClient _client = new HttpClient();
        private readonly string _endPointUri;

        public RestClient(string endPointUri)
        {
            _endPointUri = endPointUri;
        }

        public async Task<HttpResponseMessage> Publish(T parameter)
        {
            var result = await _client.PostAsync(_endPointUri, new JsonContent(parameter));

            return result;
        }
    }
}
