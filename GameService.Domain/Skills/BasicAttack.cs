using System.ComponentModel.DataAnnotations;
using GameService.Domain.Entities;
using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills
{
    public class CastBasicAttack : ICastSkill
    {
        private readonly Character _user;
        private readonly Character _target;
        
        public CastBasicAttack(Character user, Character target)
        {
            _user = user;
            _target = target;
            target.Health -= 100;
        }
        
        public bool HealthChange(out HealthResult result)
        {
            result = new HealthResult(_target.Id, _target.Health);
            return true;
        }

        public bool ManaChange(out ManaResult result)
        {
            result = null;
            return false;
        }

        public bool StatsChange(out StatsResult result)
        {
            result = null;
            return false;
        }
    }
}