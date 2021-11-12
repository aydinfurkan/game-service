using System;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Entity
{
    public class Player
    {
        public Guid Id;
        public Position Position;
        public Quaternion Quaternion;

        public Player(Guid id, Position position, Quaternion quaternion)
        {
            Id = id;
            Position = position;
            Quaternion = quaternion;
        }
    }
}