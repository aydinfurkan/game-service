namespace GameService.Domain.ValueObjects;

public class Quaternion
{
    public double W;
    public double X;
    public double Y;
    public double Z;

    public Quaternion(double w, double x, double y, double z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }
}