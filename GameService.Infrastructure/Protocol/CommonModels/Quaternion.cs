namespace GameService.Infrastructure.Protocol.CommonModels
{
    public class Quaternion
    {
        public decimal W;
        public decimal X;
        public decimal Y;
        public decimal Z;

        public Quaternion(Domain.ValueObjects.Quaternion quaternion)
        {
            W = quaternion.W;
            X = quaternion.X;
            Y = quaternion.Y;
            Z = quaternion.Z;
        }
    }
}