using System;
using GameService.Domain.Entity;

namespace GameService.Domain.Abstracts.AntiCorruption
{
    public interface IPlayerAntiCorruption
    {
        public Player GetPlayer(Guid id);
    }
}