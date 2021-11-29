using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CoreLib.Extensions;
using GameService.AntiCorruption.UserDomain;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.ValueObject;
using Character = GameService.Domain.Entity.Character;

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
        
        public async Task<Character> VerifyUser(string pToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pToken);
            httpClient.BaseAddress = new Uri("http://3.140.210.21:5000/");
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetAsync("user/internal/verify");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var user = await response.DeserializeAsync<User>();

            return new Character(user.CharacterList[0].CharacterId, new Position(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }

    }
}