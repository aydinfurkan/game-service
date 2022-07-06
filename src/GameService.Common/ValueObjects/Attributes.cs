namespace GameService.Common.ValueObjects;

public record Attributes(int Strength, int Vitality, int Dexterity, int Intelligent, int Wisdom, int Defense)
{
    public int Strength = Strength;
    public int Vitality = Vitality;
    public int Dexterity = Dexterity;
    public int Intelligent = Intelligent;
    public int Wisdom = Wisdom;
    public int Defense = Defense;
}