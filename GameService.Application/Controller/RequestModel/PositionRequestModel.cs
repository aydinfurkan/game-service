using GameService.Domain.ValueObject;

namespace GameService.Controller.RequestModel
{
    public class PositionRequestModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public Position ToDomainModel()
        {
            return new Position(X, Y, Z);
        }
    }
}