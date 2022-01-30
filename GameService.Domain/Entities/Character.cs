using System;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Entities
{
    public class Character
    {
        public Guid Id;
        public string Name;
        public string Class;
        public decimal MaxHealth;
        public decimal MaxMana;
        
        public Position Position;
        public Quaternion Quaternion;
        public Character Target;
        public string MoveState;
        public int JumpState;
        public decimal Health;
        public decimal Mana;

        public Character(Guid id, string characterName, string characterClass, Position position, Quaternion quaternion,
            decimal maxHealth, decimal health, decimal maxMana, decimal mana)
        {
            Id = id;
            Name = characterName;
            Class = characterClass;
            Position = position;
            Quaternion = quaternion;
            MaxHealth = maxHealth;
            Health = health;
            MaxMana = maxMana;
            Mana = mana;
        }
    }
}