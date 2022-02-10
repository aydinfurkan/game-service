using System;
using GameService.Domain.ValueObjects;

namespace GameService.AntiCorruption.UserService.Models.Request
{
    public class ReplaceCharacterDto
    {
        public Guid CharacterId { get; set; }
        public Position Position { get; set; }
        public Quaternion Quaternion { get; set; }
        public Attributes Attributes { get; set; }
        public double Experience { get; set; }
    }
}