using AutoMapper;
using LibApp.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibApp.Controllers.Base
{
    public class BaseController : Controller
    {

        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected async Task<T> MakeGetRequest<T>(string endpoint)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:44352/api/" + endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.Write(responseBody);

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        protected async Task<T> MakePostRequest<T>(string endpoint, T data)
        {
            HttpClient client = new HttpClient();
            HttpContent queryString = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:44352/api/" + endpoint, queryString);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        protected async Task<T> MakePutRequest<T>(string endpoint, T data)
        {
            HttpClient client = new HttpClient();
            HttpContent queryString = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:44352/api/" + endpoint, queryString);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
