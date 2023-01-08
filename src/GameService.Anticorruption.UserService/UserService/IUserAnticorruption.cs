using GameService.Anticorruption.UserService.UserService.Models.Response;
using GameService.Domain.Entities.CharacterAggregate;

namespace GameService.Anticorruption.UserService.UserService;

public interface IUserAntiCorruption
{
    public Task<UserDto?> VerifyUserAsync(string pToken);
    public Task<UserDto> ReplaceCharacterAsync(Character character);
}