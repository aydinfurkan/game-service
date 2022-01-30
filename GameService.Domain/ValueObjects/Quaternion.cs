namespace GameService.Domain.ValueObjects
{
    public class Quaternion
    {
        public decimal W;
        public decimal X;
        public decimal Y;
        public decimal Z;

        public Quaternion(decimal w, decimal x, decimal y, decimal z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        public void Set(decimal w, decimal x, decimal y, decimal z)
        {
            (W,X,Y,Z) = (w,x,y,z);
        }
        
        public (decimal, decimal, decimal, decimal) Get()
        {
            return (W,X,Y,Z);
        }
    }
}