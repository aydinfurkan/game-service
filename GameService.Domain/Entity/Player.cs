using System;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Entity
{
    public class Player
    {
        public Guid Id;
        public Position Position;

        public Player(Guid id, Position position)
        {
            Id = id;
            Position = position;
        }
    }
}