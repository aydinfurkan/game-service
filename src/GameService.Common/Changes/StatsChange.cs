namespace GameService.Common.Changes;

public record StatsChange
{
    public double MaxHealth { init; get; }
    public double HealthRegen { init; get; }  
    public double MaxMana { init; get; } 
    public double ManaRegen { init; get; }  
    public double PhysicalDamage { init; get; }  
    public double MagicDamage { init; get; }
    public double Armor { init; get; }
    public double MagicResist { init; get; } 
    public double CriticalRate { init; get; } 
    public double CriticalDamage { init; get; } 
    public double BlockRate { init; get; }
    public double CdReduction { init; get; } 
    public double MoveSpeed { init; get; } 
    public double DropRate { init; get; }
}