using GameService.Common.Helpers;
using GameService.Domain.Abstracts;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills;

public class CastBasicAttack : ChangeBase
{
    private readonly Character _user;
    private readonly Character _target;
    private readonly Skill _skill;
    
    public CastBasicAttack(Character user, Character target, Skill skill)
    {
        _user = user;
        _target = target;
        _skill = skill;
    }
        
    public override bool HealthChange(out HealthResult? result)
    {
        var armorReduction = FunctionHelper.GetArmorReduction(_target.Stats.Armor);
        
        if (GlobalRandom.Rand().Next(100) < _target.Stats.CriticalRate * 100)
        {
            _target.Health -= _user.Stats.PhysicalDamage * armorReduction * _target.Stats.CriticalDamage;
        }
        else
        {
            _target.Health -= _user.Stats.PhysicalDamage * armorReduction;
        }
            
        result = new HealthResult(_target.Id, _target.Health);
        return true;
    }
}