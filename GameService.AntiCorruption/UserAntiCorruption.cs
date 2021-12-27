using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GameService.Domain.Abstracts.AntiCorruption;
using Newtonsoft.Json;
using User = GameService.Domain.Entity.User;

namespace GameService.AntiCorruption
{
    public class UserAntiCorruption : IUserAntiCorruption
    {
        // private readonly HttpClient _httpClient;
        //
        // public UserAntiCorruption(HttpClient httpClient)
        // {
        //     _httpClient = httpClient; // TODO fix transient
        // }
        
        public async Task<User> VerifyUser(string pToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pToken);
            httpClient.BaseAddress = new Uri("http://3.140.210.21:5000/");
            
            var response = await httpClient.GetAsync("user/internal/verify");
            
            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<User>(data);

            return user;
        }

    }
}