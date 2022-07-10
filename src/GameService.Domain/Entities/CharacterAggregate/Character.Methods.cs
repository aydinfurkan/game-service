using GameService.Contract.CommonModels;
using GameService.Domain.Skills;

namespace GameService.Domain.Entities.CharacterAggregate;

public partial class Character
{
    public UserCharacterDto ToUserCharacterDto()
    {
        return new UserCharacterDto
        {
            Id = Id,
            Name = Name,
            Class = Class,
            Position = Position,
            Quaternion = Quaternion,
            Stats = Stats,
            Attributes = Attributes,
            Health = Health,
            Mana = Mana
        };
    }
    
    public CharacterDto ToCharacterDto()
    {
        return new CharacterDto
        {
            Id = Id,
            Name = Name,
            Class = Class,
            Position = Position,
            Quaternion = Quaternion,
            MaxHealth = Stats.MaxHealth,
            MaxMana = Stats.MaxMana,
            Health = Health,
            Mana = Mana
        };
    }

    public bool Tick(DateTime signalTime, out IChange change)
    {
        var delta = (signalTime - LastTick).Milliseconds / 1000.0;
        LastTick = signalTime;

        if(Health < 10e-2)
        {
            IsDead = true;
            change = null;
            return false;
        }
            
        change = new Passive(this, delta);
        return true;
    }
        
    public bool TryCastSkill(int skillCode, out IChange change)
    {
        var characterSkill = CharacterSkills.FirstOrDefault(x => x.Skill.Code == skillCode);

        if (characterSkill != null)
        {
            if (characterSkill.Skill.CastTime > 0)
            {
                return SkillStatus.StartCasting(Target, characterSkill, out change);
            }
            return characterSkill.TryCast(Target, out change);
        }
            
        change = null;
        return false;
    }

    private List<CharacterSkill> GetCharacterSkills()
    {
        return Class switch
        {
            "warrior" => new List<CharacterSkill>
            {
                new(Skill.WarriorBasicAttack, this)
            },
            "archer" => new List<CharacterSkill>
            {
                new(Skill.ArcherBasicAttack, this)
            },
            "mage" => new List<CharacterSkill>
            {
                new(Skill.MageBasicAttack, this), 
                new(Skill.Fireball, this)
            },
            "healer" => new List<CharacterSkill>
            {
                new(Skill.HealerBasicAttack, this)
            },
            _ => new List<CharacterSkill>
            {
                new(Skill.WarriorBasicAttack, this)
            }
        };
    }
}