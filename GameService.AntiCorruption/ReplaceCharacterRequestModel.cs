using System;

namespace GameService.AntiCorruption
{
    public class ReplaceCharacterRequestModel
    {
        public Guid CharacterId { get; set; }
        public Position Position { get; set; }
        public decimal Health { get; set; }
    }

    public class Position
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
    }
}