using System.Threading.Tasks;
using GameService.Domain.Entities;

namespace GameService.AntiCorruption.User
{
    public interface IUserAntiCorruption
    {
        public Task<Models.User> VerifyUserAsync(string pToken);
        public Task<Models.User> ReplaceCharacterAsync(string pToken, Character character);
    }
}