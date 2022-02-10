using System;
using GameService.Domain.Entities;
using GameService.Domain.ValueObjects;

namespace GameService.AntiCorruption.UserService.Models.Response
{
    public class CharacterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public Position Position { get; set; }
        public Quaternion Quaternion { get; set; }
        public Attributes Attributes { get; set; }
        public int Experience { get; set; }

        public Character ToModel()
        {
            return new Character(Id, Name, Class, Position, Quaternion, Attributes, Experience);
        }
    }
}