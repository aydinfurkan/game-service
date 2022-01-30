namespace GameService.AntiCorruption.User.Models
{
    public class Position
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public Domain.ValueObjects.Position ToModel()
        {
            return new Domain.ValueObjects.Position(X, Y, Z);
        }
    }
}