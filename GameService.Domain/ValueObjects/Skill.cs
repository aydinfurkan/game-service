using GameService.Domain.Entities;
using GameService.Domain.Skills;

namespace GameService.Domain.ValueObjects;

public class Skill
{
    public static readonly Skill WarriorBasicAttack = new Skill(1, 0, 5, 1000, 0, false);
    public static readonly Skill ArcherBasicAttack = new Skill(2, 0, 35, 1000, 0, false);
    public static readonly Skill MageBasicAttack = new Skill(3, 0, 30, 1000, 0, false);
    public static readonly Skill HealerBasicAttack = new Skill(4, 0, 30, 1000, 0, false);
        
    public static readonly Skill Fireball = new Skill(5, 10, 40, 10000, 1000, false);

    public readonly int Code;
    public readonly int ManaCost;
    public readonly int Range;
    public readonly int BaseCooldown;
    public readonly int CastTime;
    public readonly bool SelfCast;
    private Skill(int code, int manaCost, int range, int baseCooldown, int castTime, bool selfCast)
    {
        Code = code;
        ManaCost = manaCost;
        Range = range;
        BaseCooldown = baseCooldown;
        CastTime = castTime;
        SelfCast = selfCast;
    }

    public IChange Cast(Character user, Character target)
    {
        return Code switch
        {
            1 => new CastBasicAttack(user, target, this),
            2 => new CastBasicAttack(user, target, this),
            3 => new CastBasicAttack(user, target, this),
            4 => new CastBasicAttack(user, target, this),
            //5 => new CastBasicAttack(user, target, this), // TODO: implement
            _ => null
        };
    }
}