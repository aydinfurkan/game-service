using System;
using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.CommonModels
{
    public class Player
    {
        public Guid Id;
        public string Name;
        public string Class;
        public Position Position;
        public Quaternion Quaternion;
        public Stats Stats;
        public Attributes Attributes;
        public double Health;
        public double Mana;

        public Player(Domain.Entities.Character character)
        {
            Id = character.Id;
            Name = character.Name;
            Class = character.Class;
            Position = character.Position;
            Quaternion = character.Quaternion;
            Stats = character.Stats;
            Attributes = character.Attributes;
            Health = character.Health;
            Mana = character.Mana;
        }
    }
}