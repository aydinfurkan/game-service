using GameService.Domain.Entities;
using GameService.Domain.Skills.Results;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Skills
{
    public abstract class SkillBase : ICastSkill
    {
        private readonly Character _user;
        private readonly Character _target;
        private readonly Skill _skill;

        protected SkillBase(Character user, Character target, Skill skill)
        {
            _user = user;
            _target = target;
            _skill = skill;
        }
        public virtual bool HealthChange(out HealthResult result)
        {
            result = null;
            return false;
        }

        public virtual bool ManaChange(out ManaResult result)
        {
            result = null;
            return false;
        }

        public virtual bool StatsChange(out StatsResult result)
        {
            result = null;
            return false;
        }
    }
}