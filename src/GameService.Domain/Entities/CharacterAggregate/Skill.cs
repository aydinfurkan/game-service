using GameService.Domain.Skills;

namespace GameService.Domain.Entities.CharacterAggregate;

public class Skill
{
    public static readonly List<Skill> All = new List<Skill>();
    
    public static readonly Skill WarriorBasicAttack = new Skill(1, 0, 5, 1000, 0, false, null);
    public static readonly Skill ArcherBasicAttack = new Skill(2, 0, 35, 1000, 0, false, null);
    public static readonly Skill MageBasicAttack = new Skill(3, 0, 30, 1000, 1000, false, null);
    public static readonly Skill HealerBasicAttack = new Skill(4, 0, 30, 1000, 0, false, null);
    
    public static readonly Skill Fireball = new Skill(5, 10, 40, 10000, 1000, false, Effect.Burn);

    public readonly int Code;
    public readonly int ManaCost;
    public readonly int Range;
    public readonly int BaseCooldown;
    public readonly int CastTime;
    public readonly bool SelfCast;
    public readonly Effect? Effect;
    private Skill(int code, int manaCost, int range, int baseCooldown, int castTime, bool selfCast, Effect? effect)
    {
        Code = code;
        ManaCost = manaCost;
        Range = range;
        BaseCooldown = baseCooldown;
        CastTime = castTime;
        SelfCast = selfCast;
        Effect = effect;

        Skill.All.Add(this);
    }

    public IChange? Cast(Character user)
    {
        return Code switch
        {
            //5 => new CastBasicAttack(user, target, this), // TODO: cast sonrasi degisiklikler mana azaltma gibi
            _ => null
        };
    }
    
    public IChange? Execute(Character user, Character target)
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