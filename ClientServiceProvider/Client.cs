using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace ClientServiceProvider
{
    public class Client : IClient
    {
        private readonly HttpClient _HttpClient;
        private readonly string _Token = string.Empty;

        public Client(string ControllerBaseAddress)
        {
            _HttpClient = new HttpClient();
            _HttpClient.BaseAddress = new Uri(ControllerBaseAddress);
        }

        public Client(string ControllerBaseAddress, string Token)
        {
            _HttpClient = new HttpClient();
            _HttpClient.BaseAddress = new Uri(ControllerBaseAddress);
            _Token = Token;
        }

        public async Task<U> GetAsync<U>(string ApiPath)
        {
            return await GetCoreAsync<U>(ApiPath);
        }

        public async Task<U> GetAsync<T, U>(string ApiPath, T Param)
        {
            var strParams = "";
            if (Param.GetType().GetProperties().Count() > 0 && Param.GetType() != typeof(string))
            {
                strParams = "?";
                foreach (var property in Param.GetType().GetProperties())
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(Param);
                    strParams += $"{propertyName}={propertyValue}&";
                }
                if (strParams.EndsWith("&") && strParams.Length > 0) //remove & from end of string
                    strParams = strParams.Substring(0, strParams.Length - 1);
            }
            return await GetCoreAsync<U>(ApiPath, strParams);
        }

        private async Task<U> GetCoreAsync<U>(string ApiPath, string Params = "")
        {
            if (ApiPath.EndsWith("/") && ApiPath.Length > 0)//remove / from end of string
                ApiPath = ApiPath.Substring(0, ApiPath.Length - 1);
            _HttpClient.DefaultRequestHeaders.Accept.Clear();
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token());
            HttpResponseMessage response = await _HttpClient.GetAsync($"{ApiPath}{Params}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var retVal = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<U>(retVal);
            }
            return default;
        }

        public async Task<U> PostAsync<T, U>(string ApiPath, T Param)
        {
            _HttpClient.DefaultRequestHeaders.Accept.Clear();
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token());
            HttpResponseMessage response = await _HttpClient.PostAsJsonAsync(ApiPath, Param);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var retVal = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<U>(retVal);
            }
            return default;
        }

        public async Task<U> PostFromBody<T, U>(string ApiPath, T Param)
        {
            _HttpClient.DefaultRequestHeaders.Accept.Clear();
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token());
            _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(Param);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _HttpClient.PostAsync(ApiPath, data);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var retVal = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<U>(retVal);
            }
            return default;
        }

        protected virtual string Token()
        {
            return _Token;
        }





    }
}
