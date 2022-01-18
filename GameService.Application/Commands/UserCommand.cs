using System.Threading.Tasks;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;

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