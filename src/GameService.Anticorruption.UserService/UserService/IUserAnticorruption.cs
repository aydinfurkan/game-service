using System.Threading.Tasks;
using GameService.Anticorruption.UserService.Models.Response;
using GameService.Domain.Entities;

namespace GameService.Anticorruption.UserService;

public interface IUserAntiCorruption
{
    public Task<UserDto> VerifyUserAsync(string pToken);
    public Task<UserDto> ReplaceCharacterAsync(Character character);
}