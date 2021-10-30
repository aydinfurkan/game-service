using GameService.Domain.ValueObject;

namespace GameService.Controller.RequestModel
{
    public class PositionRequestModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Position ToDomainModel()
        {
            return new Position(X, Y, Z);
        }
    }
}