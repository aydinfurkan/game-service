
using GameService.Domain.ValueObject;

namespace GameService.Controller.RequestModel
{
    public class RequestModel
    {
        public PositionRequestModel Position { get; set; }
        public QuaternionRequestModel Quaternion { get; set; }
        
    }

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
    public class QuaternionRequestModel
    {
        public decimal W { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
        public Quaternion ToDomainModel()
        {
            return new Quaternion(W, X, Y, Z);
        }
    }
}