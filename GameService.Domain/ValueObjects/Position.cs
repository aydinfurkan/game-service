namespace GameService.Domain.ValueObjects
{
    public class Position
    {
        public double X;
        public double Y;
        public double Z;

        public Position(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}