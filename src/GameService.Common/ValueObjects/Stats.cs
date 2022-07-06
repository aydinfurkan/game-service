namespace GameService.Common.ValueObjects;

public record Stats
{
    public double MaxHealth;
    public double HealthRegen;  
    public double MaxMana; 
    public double ManaRegen;  
    public double PhysicalDamage;  
    public double MagicDamage; 
    public double Armor; 
    public double MagicResist; 
    public double CriticalRate; 
    public double CriticalDamage; 
    public double BlockRate;
    public double CdReduction; 
    public double MoveSpeed; 
    public double DropRate;

    public Stats(Attributes attributes, Level level)
    {
        MaxHealth = 500 + attributes.Vitality * 10 + level.Value * 20;
        HealthRegen = 1 + level.Value * 0.5;
        MaxMana = 50 + (attributes.Wisdom + attributes.Intelligent) * 10 + level.Value * 10;
        ManaRegen = 0.5 + level.Value * 0.3;
        PhysicalDamage = 10 + (attributes.Strength + attributes.Dexterity * 0.6) * 10 + level.Value * 5;
        MagicDamage = 10 + attributes.Intelligent * 10 + level.Value * 5;
        Armor = 1000 + attributes.Defense * 10 + level.Value * 2;
        MagicResist = 10 + attributes.Defense * 10 + level.Value * 5;
        CriticalRate = 0.05;
        CriticalDamage = 1.5 + attributes.Dexterity * 0.005;
        BlockRate = 0.01;
        CdReduction = 0 + attributes.Wisdom / 100;
        MoveSpeed = 100;
        DropRate = 1;
    }


}