using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills
{
    public interface IChange
    {
        public bool HealthChange(out HealthResult result);
        public bool ManaChange(out ManaResult result);
        public bool StatsChange(out StatsResult result);
    }
}