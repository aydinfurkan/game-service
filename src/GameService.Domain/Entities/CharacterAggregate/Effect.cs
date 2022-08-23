using GameService.Common.Changes;

namespace GameService.Domain.Entities.CharacterAggregate;

public class Effect
{
    public static readonly Effect Burn = new Effect(1, 5000, 50, 0, null);
    public static readonly Effect Restore = new Effect(2, 10000, 0, 100, null);
    public static readonly Effect Crack = new Effect(3, 3000, 0, 0, new StatsChange
    {
        MoveSpeed = -50,
    });
    
    public readonly int Code;
    public readonly int Duration;
    public readonly double BaseDamage;
    public readonly double BaseHeal;
    public readonly StatsChange? StatsChange;
    
    private Effect(int code, int duration, double baseDamage, double baseHeal, StatsChange? statsChange)
    {
        Code = code;
        Duration = duration;
        BaseDamage = baseDamage;
        BaseHeal = baseHeal;
        StatsChange = statsChange;
    }
}