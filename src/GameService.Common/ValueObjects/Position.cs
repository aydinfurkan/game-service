namespace GameService.Common.ValueObjects;

public record Position(double X, double Y, double Z)
{
    public readonly double X = X;
    public readonly double Y = Y;
    public readonly double Z = Z;

    public double DistanceTo(Position target)
    {
        return Math.Sqrt(Math.Pow(target.X - X, 2) + 
                         Math.Pow(target.Y - Y, 2) +
                         Math.Pow(target.Z - Z, 2));
    }
}