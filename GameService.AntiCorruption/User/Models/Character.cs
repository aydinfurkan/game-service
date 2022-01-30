using System;

namespace GameService.AntiCorruption.User.Models
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public Position Position { get; set; }
        public Quaternion Quaternion { get; set; }
        public decimal MaxHealth { get; set; }
        public decimal Health { get; set; }
        public decimal MaxMana { get; set; }
        public decimal Mana { get; set; }

        public Domain.Entities.Character ToModel()
        {
            return new Domain.Entities.Character(Id, Name, Class, Position.ToModel(), Quaternion.ToModel(),
            MaxHealth, Health, MaxMana, Mana);
        }
    }
}