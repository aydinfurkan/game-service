using GameService.Domain.Skills;

namespace GameService.Domain.Entities.CharacterAggregate;

public class LearnedSkill
{
    public readonly Skill Skill;
    public readonly Character User;
    public int Level;
    public DateTime UsedAt;
    public DateTime CanBeUsedAt;
    public DateTime CastingStartedAt;
    public DateTime CastingWillEndAt;

    public LearnedSkill(Skill skill, Character user)
    {
        Skill = skill;
        User = user;
        Level = 1;
        UsedAt = DateTime.UtcNow;
        CanBeUsedAt = DateTime.UtcNow;
        CastingStartedAt = DateTime.UtcNow;
        CastingWillEndAt = DateTime.UtcNow;
    }
        
    public void StartCast()
    {
        CastingStartedAt = DateTime.Now;
        CastingWillEndAt = DateTime.Now.AddMilliseconds(Skill.CastTime);
    }
    
    public bool EndCast(Character? target, out IChange? change)
    {
        if (target == null)
        {
            change = null;
            return false;
        }
        
        change = Skill.Cast(User);
        
        UsedAt = DateTime.UtcNow;
        CanBeUsedAt = DateTime.UtcNow.AddMilliseconds(CalculateCooldown());
        
        change = null;
        return false;
    }

    public bool Execute(Character? target, out IChange? change)
    {
        if (target == null)
        {
            change = null;
            return false;
        }
        
        change = Skill.Execute(User, target);
        return true;
    }

    public bool CanBeCasting(Character? target)
    {
        if (target == null) return false;
            
        return IsUserAlive() && IsTargetValid(target) && IsTargetAlive(target) &&
               IsManaEnough() && IsInRange(target) && IsNotOnCooldown();
    }
    
    public bool CanBeCasted()
    {
        return IsUserAlive() && IsManaEnough() && IsCastEnd();
    }

    public bool CanBeExecuted(Character? target)
    {
        if (target == null) return false;
            
        return IsUserAlive() && IsTargetValid(target) && IsTargetAlive(target) &&
               IsManaEnough() && IsNotOnCooldown();
    }

    private double CalculateCooldown()
    {
        return Skill.BaseCooldown * (1 - User.Stats.CdReduction);
    }
        
    private bool IsNotOnCooldown()
    {
        return CanBeUsedAt <= DateTime.UtcNow;
    }
    private bool IsCastEnd()
    {
        return CastingWillEndAt <= DateTime.UtcNow;
    }
    private bool IsUserAlive()
    {
        return !User.IsDead;
    }
    private bool IsTargetValid(Character target)
    {
        return Skill.SelfCast || target.Id != User.Id;
    }
    private bool IsTargetAlive(Character target)
    {
        return !target.IsDead;
    }
    private bool IsManaEnough()
    {
        return User.Mana >= Skill.ManaCost;
    }
    private bool IsInRange(Character target)
    {
        return User.Position.DistanceTo(target.Position) <= Skill.Range;
    }

}