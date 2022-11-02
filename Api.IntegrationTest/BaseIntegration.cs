using Api.CrossCutting.DependencyInjection;
using Api.Data.Context;
using Api.Domain.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Api.IntegrationTest
{
    public abstract class BaseIntegration : IDisposable
    {
        public MyContext myContext { get; private set; }
        public HttpClient client { get; private set; }
        public IMapper mapper { get; set; }
        public string hostApi { get; set; }
        public HttpResponseMessage response { get; set; }

        public BaseIntegration()
        {
            hostApi = "http://localhost:44349/api";
            var builder = new WebHostBuilder().UseEnvironment("Testing").UseStartup<Startup>();
            var server = new TestServer(builder);

            myContext = server.Host.Services.GetService(typeof(MyContext)) as MyContext;

            mapper = ConfigureAutoMapper.ConfigureMapper();

            client = server.CreateClient();
        }

        public async Task AddToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "adm@hotmail.com"
            };

            var result = await PostJsonAsync(loginDto, $"{hostApi}/login", client);
            var jsonLogin = await result.Content.ReadAsStringAsync();
            var loginDTO = JsonConvert.DeserializeObject<LoginResponseDto>(jsonLogin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.acessToken);
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(object data, string url, HttpClient client)
        {
            return await client.PostAsync(url,
                new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json"));
        }

        public void Dispose()
        {
            myContext.Dispose();
            client.Dispose();
        }
    }
}
