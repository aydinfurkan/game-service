namespace GameService.Common.ValueObjects;

public record Level(int Experience)
{
    public readonly int Value = Experience / 1000;
    public int Experience = Experience;
}