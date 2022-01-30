using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameService.AntiCorruption.User.Models;
using Newtonsoft.Json;
using Character = GameService.Domain.Entities.Character;

namespace GameService.AntiCorruption.User
{
    public class UserAntiCorruption : IUserAntiCorruption
    {
        // private readonly HttpClient _httpClient;
        //
        // public UserAntiCorruption(HttpClient httpClient)
        // {
        //     _httpClient = httpClient; // TODO fix transient
        // }
        
        public async Task<Models.User> VerifyUserAsync(string pToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pToken);
            httpClient.BaseAddress = new Uri("http://3.140.210.21:5000/");
            
            var response = await httpClient.GetAsync("user/internal/verify");
            
            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<Models.User>(data);

            return user;
        }
        public async Task<Models.User> ReplaceCharacterAsync(string pToken, Character character)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pToken);
            httpClient.BaseAddress = new Uri("http://3.140.210.21:5000/");

            var requestModel = new ReplaceCharacterRequestModel
            {
                CharacterId = character.Id,
                Position = new Position
                {
                    X = character.Position.X, Y = character.Position.Y, Z = character.Position.Z
                },
                Quaternion = new Quaternion
                {
                    X = character.Quaternion.X, Y = character.Quaternion.Y, Z = character.Quaternion.Z, W = character.Quaternion.W
                },
                MaxHealth = character.MaxHealth,
                Health = character.Health,
                MaxMana = character.MaxMana,
                Mana = character.Mana,
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");  
            var response = await httpClient.PutAsync("user/character", httpContent);
            
            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<Models.User>(data);

            return user;
        }

    }
}