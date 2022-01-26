using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gateways.NET.SDK
{
    /// <summary>
    /// Simple extensible backend client
    /// </summary>
    public class ExtensibleBackendClient
    {
        private ApiConfiguration _config;
        private HttpClient _client;
        private readonly ILogger<ExtensibleBackendClient> _logger;

        public Uri BaseAddress { get; private set; }

        public ExtensibleBackendClient(ApiConfiguration config, ILogger<ExtensibleBackendClient> logger)
        {
            BaseAddress = new Uri(config.ApiUrl);
            _config = config;
            _logger = logger;
        }

        public ApiConfiguration Configuration => _config;

        #region [ Protected implementation ]

        protected virtual void LogError(Exception ex, string message)
        {
            if (_logger != null)
                _logger.LogError(message, ex);
        }

        protected virtual HttpClient GetClient()
        {
            return _client ?? (_client = new HttpClient());
        }

        protected virtual JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        protected virtual async Task<T> Request<T>(Method method, string endpoint, object data)
        {
            var client = GetClient();
            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var url = new Uri(BaseAddress, endpoint);

            HttpResponseMessage responseMessage;
            switch (method)
            {
                case Method.POST:
                    responseMessage = await client.PostAsync(url, content);
                    break;
                case Method.PUT:
                    responseMessage = await client.PutAsync(url, content);
                    break;
                case Method.PATCH:
                    responseMessage = await client.PatchAsync(url, content);
                    break;
                default:
                    throw new Exception("Unsupported Http method for this request");
            }

            return await GetContent<T>(responseMessage);
        }        

        protected virtual async Task<T> Request<T>(Method method, string endpoint)
        {
            var client = GetClient();
            var url = new Uri(BaseAddress, endpoint);

            HttpResponseMessage responseMessage = null;

            switch (method)
            {
                case Method.GET:
                    responseMessage = await client.GetAsync(url);
                    break;
                case Method.DELETE:
                    responseMessage = await client.DeleteAsync(url);
                    break;
                default:
                    throw new Exception("Unsupported Http method for this request");
            }

            return await GetContent<T>(responseMessage);
        }

        protected virtual async Task<T> GetContent<T>(HttpResponseMessage message)
        {
            var json = await message.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
            return response;
        } 

        #endregion

        #region [ Supported HTTP methods ]

        public virtual async Task<T> Post<T>(string endpoint, object data)
        {
            return await Request<T>(Method.POST, endpoint, data);
        }

        public virtual async Task<T> Put<T>(string endpoint, object data)
        {
            return await Request<T>(Method.PUT, endpoint, data);
        }

        public virtual async Task<T> Patch<T>(string endpoint, object data)
        {
            return await Request<T>(Method.PATCH, endpoint, data);
        }

        public virtual async Task<T> Get<T>(string endpoint)
        {
            return await Request<T>(Method.GET, endpoint);
        }

        public virtual async Task<T> Delete<T>(string endpoint)
        {
            return await Request<T>(Method.DELETE, endpoint);
        } 

        #endregion
    }

    public enum Method
    {
        POST,
        PUT,
        PATCH,
        GET,
        DELETE
    }
}
