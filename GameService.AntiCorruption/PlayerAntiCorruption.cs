using System;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;
using GameService.Domain.ValueObject;

namespace GameService.AntiCorruption
{
    public class PlayerAntiCorruption : IPlayerAntiCorruption
    {
        public Player GetPlayer(Guid id)
        {
            return new Player(id, new Position(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }
    }
}