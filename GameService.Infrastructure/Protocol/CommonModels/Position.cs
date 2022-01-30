namespace GameService.Infrastructure.Protocol.CommonModels
{
    public class Position
    {
        public decimal X;
        public decimal Y;
        public decimal Z;

        public Position(Domain.ValueObjects.Position position)
        {
            X = position.X;
            Y = position.Y;
            Z = position.Z;
        }
    }
}