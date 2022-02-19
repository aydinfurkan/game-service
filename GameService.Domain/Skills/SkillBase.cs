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
        protected bool CanBeCasted;
        
        protected SkillBase(Character user, Skill skill)
        {
            _user = user;
            _skill = skill;
            if (IsUserAlive() && IsManaEnough()) CanBeCasted = true;
        }
        
        protected SkillBase(Character user, Character target, Skill skill)
        {
            _user = user;
            _target = target;
            _skill = skill;
            if (IsUserAlive() && IsTargetAlive() && IsManaEnough() && IsRangeEnough()) CanBeCasted = true;
        }

        private bool IsUserAlive()
        {
            return _user.Health > 0;
        }
        private bool IsTargetAlive()
        {
            return _target.Health > 0;
        }
        private bool IsManaEnough()
        {
            return _user.Mana >= _skill.ManaCost;
        }
        private bool IsRangeEnough()
        {
            return _user.Position.DistanceTo(_target.Position) <= _skill.Range;
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