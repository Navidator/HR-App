using Newtonsoft.Json;
using System;
using System.Text;

namespace JobInterviewProject.MVC.Services
{
    public class HttpClientService : IHttpClientService 
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void AuthorizationHeader(string authorizationToken = null)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(authorizationToken))
            {
                authorizationToken = authorizationToken.Replace("Bearer", "").Trim();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationToken);
            }
        }

        public async Task<T> GetAsync<T>(string url, string authorizationToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            AuthorizationHeader(authorizationToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);

            return result;
        }

        public async Task<T> PostAsync<T>(string url, object data, string authorizationToken = null)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            AuthorizationHeader(authorizationToken);

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(responseContent);

            return result;
        }

        public async Task<T> PutAsync<T>(string url, object data, string authorizationToken = null)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            AuthorizationHeader(authorizationToken);

            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(responseContent);

            return result;
        }

        public async Task DeleteAsync(string url, string authorizationToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            AuthorizationHeader(authorizationToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
