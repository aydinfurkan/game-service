using System;

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

        public double DistanceTo(Position target)
        {
            return Math.Sqrt(Math.Pow(target.X - X, 2) + 
                             Math.Pow(target.Y - Y, 2) +
                             Math.Pow(target.Z - Z, 2));
        }
    }
}