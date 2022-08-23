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
        
    public bool TryCast(Character? target, out IChange? change)
    {
        if (target == null || !CanBeCasted(target))
        {
            change = null;
            return false;
        }
        
        if (Skill.CastTime > 0)
        {
            return Cast(out change);
        }

        return Execute(target, out change);
    }

    public bool TryExecute(Character? target, out IChange? change)
    {
        var isCastEnd = DateTime.Now >= CastingWillEndAt; // TODO: use isExecuted or maybe stateMachine

        if (isCastEnd)
        {
            Execute(target, out change);
            return true;
        }

        change = null;
        return false;
    }
    
    private bool Execute(Character? target, out IChange? change)
    {
        if (target == null || !CanBeExecuted(target))
        {
            change = null;
            return false;
        }

        change = Skill.Cast(User, target);
        UsedAt = DateTime.UtcNow;
        CanBeUsedAt = DateTime.UtcNow.AddMilliseconds(CalculateCooldown());
        return true;
    }

    private bool Cast(out IChange? change)
    {
        CastingStartedAt = DateTime.Now;
        CastingWillEndAt = DateTime.Now.AddMilliseconds(Skill.CastTime);
        change = null;
        return true;
    }

    private bool CanBeCasted(Character? target)
    {
        if (target == null) return false;
            
        return IsUserAlive() && IsTargetValid(target) && IsTargetAlive(target) &&
               IsManaEnough() && IsInRange(target) && IsNotOnCooldown();
    }

    private bool CanBeExecuted(Character? target)
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