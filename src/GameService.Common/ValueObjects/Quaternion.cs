namespace GameService.Common.ValueObjects;

public record Quaternion(double W, double X, double Y, double Z)
{
    public double W = W;
    public double X = X;
    public double Y = Y;
    public double Z = Z;
}