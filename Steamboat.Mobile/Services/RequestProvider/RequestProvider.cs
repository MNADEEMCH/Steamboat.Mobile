using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Steamboat.Mobile.Exceptions;
using Steamboat.Mobile.Models;

namespace Steamboat.Mobile.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            HttpClient httpClient = CreateHttpClient("");
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> PostAsync<TResult,TData>(string uri, TData data, string sessionID = "")
        {
            HttpClient httpClient = CreateHttpClient(uri, sessionID, "");

            if (!string.IsNullOrEmpty(sessionID))
            {
                AddSessionID(httpClient, sessionID);
            }

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json,Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string sessionID = "")
        {
            HttpClient httpClient = CreateHttpClient(uri, sessionID, "");

            if (!string.IsNullOrEmpty(sessionID))
            {
                AddSessionID(httpClient, sessionID);
            }

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string sessionID = "")
        {
            HttpClient httpClient = CreateHttpClient(uri, sessionID, "");

            if (!string.IsNullOrEmpty(sessionID))
            {
                AddSessionID(httpClient, sessionID);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uri, null);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string sessionID = "")
        {
            HttpClient httpClient = CreateHttpClient(uri, sessionID, "");

            if (!string.IsNullOrEmpty(sessionID))
            {
                AddSessionID(httpClient, sessionID);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(uri, "", "");
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient CreateHttpClient(string uri, string sessionID = null, string token = "")
        {
            HttpClient httpClient;

            if (uri.Contains("login"))
            {
                httpClient = new HttpClient();
            }
            else
            {
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                cookies.Add(new Uri(uri),new Cookie("ASP.NET_SessionId", sessionID, "/", "momentumhealth.co"));
                handler.CookieContainer = cookies;
                httpClient = new HttpClient(handler);
            }

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Momentum-Api","true");
            httpClient.DefaultRequestHeaders.Add("Momentum-Api-Environment", "F5752008-E484-4691-B58A-3338A90F80AA");

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private void AddSessionID(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add("Momentum-Api-Session", parameter);
            //var cookieParameter = string.Format("ASP.NET_SessionId={0};",parameter);
            //httpClient.DefaultRequestHeaders.Add("Cookie", cookieParameter);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if(!content.Contains("<html>")){
                    var result = await Task.Run(() => JsonConvert.DeserializeObject<Models.Error.ErrorInfo>(content, _serializerSettings));
                    throw new ServiceException(result.Error.Message);
                }
                else
                    throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
    }
}
