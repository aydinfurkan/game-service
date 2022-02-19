using GameService.Domain.Entities;
using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills
{
    public interface ICastSkill
    {
        public bool HealthChange(out HealthResult result);
        public bool ManaChange(out ManaResult result);
        public bool StatsChange(out StatsResult result);
    }
}