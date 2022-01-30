namespace GameService.Domain.ValueObjects
{
    public class Position
    {
        public decimal X;
        public decimal Y;
        public decimal Z;

        public Position(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Set(decimal x, decimal y, decimal z)
        {
            (X,Y,Z) = (x,y,z);
        }
        
        public (decimal, decimal, decimal) Get()
        {
            return (X,Y,Z);
        }
    }
}