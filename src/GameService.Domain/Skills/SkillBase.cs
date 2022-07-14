using GameService.Domain.Entities.CharacterAggregate;
using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills;

public abstract class SkillBase : IChange
{
    protected readonly Character User;
    protected readonly Character Target;
    protected readonly Skill Skill;

    protected SkillBase(Character user, Character target, Skill skill)
    {
        User = user;
        Target = target;
        Skill = skill;
    }
    public virtual bool HealthChange(out HealthResult? result)
    {
        result = null;
        return false;
    }

    public virtual bool ManaChange(out ManaResult? result)
    {
        result = null;
        return false;
    }

    public virtual bool StatsChange(out StatsResult? result)
    {
        result = null;
        return false;
    }
}