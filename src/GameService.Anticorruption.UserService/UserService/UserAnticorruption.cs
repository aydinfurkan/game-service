using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameService.Anticorruption.Configs;
using GameService.Anticorruption.UserService.Models.Request;
using GameService.Anticorruption.UserService.Models.Response;
using GameService.Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameService.Anticorruption.UserService;

public class UserAntiCorruption : IUserAntiCorruption
{
    private readonly HttpClient _httpClient;
    private readonly UserServiceSettings _userServiceSettings;

    public UserAntiCorruption(HttpClient httpClient, IOptions<UserServiceSettings> userServiceSettings)
    {
        _httpClient = httpClient;
        _userServiceSettings = userServiceSettings.Value;
            
        _httpClient.BaseAddress = new UriBuilder(
            _userServiceSettings.Url.Scheme,
            _userServiceSettings.Url.Host,
            _userServiceSettings.Url.Port).Uri;
    }
        
    public async Task<UserDto> VerifyUserAsync(string pToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pToken);
            
        var response = await _httpClient.GetAsync("user");
            
        var data = await response.Content.ReadAsStringAsync();

        var user = JsonConvert.DeserializeObject<UserDto>(data);

        return user;
    }
    public async Task<UserDto> ReplaceCharacterAsync(Character character)
    {
            
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{_userServiceSettings.Credentials.Username}:{_userServiceSettings.Credentials.Password}")));
            
        var requestModel = new ReplaceCharacterDto
        {
            CharacterId = character.Id,
            Position = character.Position,
            Quaternion = character.Quaternion,
            Attributes = character.Attributes,
            Experience = character.Level.Experience
        };

        var httpContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");  
        var response = await _httpClient.PutAsync("user/character", httpContent);
            
        var data = await response.Content.ReadAsStringAsync();

        var user = JsonConvert.DeserializeObject<UserDto>(data);

        return user;
    }

}