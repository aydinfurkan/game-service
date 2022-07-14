using GameService.Common.Helpers;
using GameService.Domain.Abstracts;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills;

public class CastBasicAttack : SkillBase
{
    public CastBasicAttack(Character user, Character target, Skill skill) : base(user, target, skill)
    {
    }
        
    public override bool HealthChange(out HealthResult? result)
    {
        var armorReduction = FunctionHelper.GetArmorReduction(Target.Stats.Armor);
        
        if (GlobalRandom.Rand().Next(100) < Target.Stats.CriticalRate * 100)
        {
            Target.Health -= User.Stats.PhysicalDamage * armorReduction * Target.Stats.CriticalDamage;
        }
        else
        {
            Target.Health -= User.Stats.PhysicalDamage * armorReduction;
        }
            
        result = new HealthResult(Target.Id, Target.Health);
        return true;
    }
    
    
}