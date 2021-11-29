using System;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Entity
{
    public class Character
    {
        public Guid Id;
        public Position Position;
        public Quaternion Quaternion;

        public Character(Guid id, Position position, Quaternion quaternion)
        {
            Id = id;
            Position = position;
            Quaternion = quaternion;
        }
    }
}