using System.Threading.Tasks;
using GameService.Domain.Entity;

namespace GameService.Domain.Abstracts.AntiCorruption
{
    public interface IUserAntiCorruption
    {
        public Task<User> VerifyUserAsync(string pToken);
        public Task<User> ReplaceCharacterAsync(string pToken, Character character);
    }
}