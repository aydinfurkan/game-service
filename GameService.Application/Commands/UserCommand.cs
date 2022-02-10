using System.Threading.Tasks;
using GameService.AntiCorruption.UserService;
using GameService.Domain.Entities;

namespace GameService.Commands
{
    public class UserCommand
    {
        private readonly IUserAntiCorruption _userAntiCorruption;

        public UserCommand(IUserAntiCorruption userAntiCorruption)
        {
            _userAntiCorruption = userAntiCorruption;
        }

        public async Task UpdateCharacter(string pToken, Character character)
        {
            await _userAntiCorruption.ReplaceCharacterAsync(pToken, character);
        }
        
    }
}