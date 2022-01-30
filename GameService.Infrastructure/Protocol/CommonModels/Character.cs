using System;

namespace GameService.Infrastructure.Protocol.CommonModels
{
    public class Character
    {
        public Guid Id;
        public string Name;
        public string Class;
        public Position Position;
        public Quaternion Quaternion;
        public decimal MaxHealth;
        public decimal Health;
        public decimal MaxMana;
        public decimal Mana;

        public Character(Domain.Entities.Character character)
        {
            Id = character.Id;
            Name = character.Name;
            Class = character.Class;
            Position = new Position(character.Position);
            Quaternion = new Quaternion(character.Quaternion);
            MaxHealth = character.MaxHealth;
            Health = character.Health;
            MaxMana = character.MaxMana;
            Mana = character.Mana;
        }
    }
}