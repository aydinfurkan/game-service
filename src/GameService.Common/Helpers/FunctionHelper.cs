namespace GameService.Common.Helpers;

public static class FunctionHelper
{
    public static double GetArmorReduction(double x)
    {
        const int maxReduction = 60;
        const int sharpness = 100;
        var value = maxReduction * Math.Tanh(x/sharpness);
        return value / 100;
    }
}