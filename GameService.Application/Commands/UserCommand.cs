using System.Threading.Tasks;
using GameService.Anticorruption.UserService;
using GameService.Domain.Entities;

namespace GameService.Application.Commands;

public class UserCommand
{
    private readonly IUserAntiCorruption _userAntiCorruption;

    public UserCommand(IUserAntiCorruption userAntiCorruption)
    {
        _userAntiCorruption = userAntiCorruption;
    }

    public async Task UpdateCharacterAsync(string pToken, Character character)
    {
        await _userAntiCorruption.ReplaceCharacterAsync(pToken, character);
    }
        
}