using System;
using GameService.Domain.Entities;
using GameService.Domain.Skills;

namespace GameService.Domain.Components;

public enum Status
{
    Ready,
    Casting,
}
    
public class SkillStatus
{
    public CharacterSkill CharacterSkill;
    public DateTime CastingStartedAt;
    public Status Status;
    public Character Target;
        
    public SkillStatus()
    {
        Status = Status.Ready;
    }

    public bool StartCasting(Character target, CharacterSkill characterSkill, out IChange change)
    {
        change = null;
        if (Status == Status.Casting)
        {
            return false;
        }
        Target = target;
        CharacterSkill = characterSkill;
        Status = Status.Casting;
        CastingStartedAt = DateTime.Now;
        return true;
    }

    public bool Check(out IChange change)
    {
        if (Status == Status.Casting)
        {
            var castingTime = (DateTime.Now - CastingStartedAt).Milliseconds;

            if(castingTime >= CharacterSkill.Skill.CastTime)
            {
                var ok = CharacterSkill.TryCast(Target, out change);
            }
        }
            
        change = null;
        return false;
    }
}