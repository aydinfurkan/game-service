using GameService.Domain.Abstracts;
using GameService.Domain.Entities;
using GameService.Domain.Skills.Results;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Skills;

public class CastBasicAttack : SkillBase
{
    private readonly Character _user;
    private readonly Character _target;
        
    public CastBasicAttack(Character user, Character target, Skill skill) : base(user, target, skill)
    {
        _user = user;
        _target = target;
    }
        
    public override bool HealthChange(out HealthResult result)
    {
        if (GlobalRandom.Rand().Next(100) < _target.Stats.CriticalRate * 100)
        {
            _target.Health -= _user.Stats.PhysicalDamage * (1 - _target.Stats.Armor / 300) * _target.Stats.CriticalDamage;
        }
        else
        {
            _target.Health -= _user.Stats.PhysicalDamage * (1 - _target.Stats.Armor / 300);
        }
            
        result = new HealthResult(_target.Id, _target.Health);
        return true;
    }
}