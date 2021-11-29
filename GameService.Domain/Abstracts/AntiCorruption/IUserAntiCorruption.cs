using System.Threading.Tasks;
using GameService.Domain.Entity;

namespace GameService.Domain.Abstracts.AntiCorruption
{
    public interface IUserAntiCorruption
    {
        public Task<Character> VerifyUser(string pToken);
    }
}