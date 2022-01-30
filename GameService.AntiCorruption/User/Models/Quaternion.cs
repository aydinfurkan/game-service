namespace GameService.AntiCorruption.User.Models
{
    public class Quaternion
    {
        public decimal W { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }


        public Domain.ValueObjects.Quaternion ToModel()
        {
            return new Domain.ValueObjects.Quaternion(W, X, Y, Z);
        }
    }
}