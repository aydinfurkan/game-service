using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills;

public abstract class ChangeBase : IChange
{
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