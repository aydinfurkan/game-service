using System;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Entity
{
    public class Character
    {
        public Guid Id;
        public string Name;
        public string Class;
        public Position Position;
        public Quaternion Quaternion;
        public decimal Health;
        public string MoveState;
        public int JumpState;

        public Character(Guid id, string characterName, string characterClass, Position position, Quaternion quaternion, decimal health)
        {
            Id = id;
            Name = characterName;
            Class = characterClass;
            Position = position;
            Quaternion = quaternion;
            Health = health;
            MoveState = "";
            JumpState = 0;
        }
    }
}