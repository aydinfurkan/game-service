namespace GameService.Domain.ValueObjects;

public class Level
{
    public int Value;
    public int Experience;

    public Level(int experience)
    {
        Value = experience / 1000;
        Experience = experience;
    }
}