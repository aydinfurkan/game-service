using System.Threading.Tasks;
using GameService.AntiCorruption.UserService.Models.Response;
using GameService.Domain.Entities;

namespace GameService.AntiCorruption.UserService
{
    public interface IUserAntiCorruption
    {
        public Task<UserDto> VerifyUserAsync(string pToken);
        public Task<UserDto> ReplaceCharacterAsync(string pToken, Character character);
    }
}