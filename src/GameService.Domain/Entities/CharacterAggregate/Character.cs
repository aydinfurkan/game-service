using GameService.Common.ValueObjects;
using GameService.Contract.Enums;
using GameService.Domain.Components;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.Domain.Skills;
using GameService.Domain.State;

namespace GameService.Domain.Entities;

public class Character
{
    public Guid Id;
    public string Name;
    public string Class;
        
    public Position Position;
    public Quaternion Quaternion;

    public Attributes Attributes;
    public Level Level;
    public Stats Stats;
    public double Health;
    public double Mana;
    public bool IsDead;
    public SkillStatus SkillStatus = new ();
        
    public Character Target;
    public List<CharacterSkill> CharacterSkills;

    public CharacterState State = CharacterState.Idle;
    public string MoveState;
    public int JumpState;
    
    public DateTime LastTick;
    public Character(Guid id, string name, string @class, Position position, Quaternion quaternion, Attributes attributes, int experience)
    {
        Id = id;
        Name = name;
        Class = @class;
        Position = position;
        Quaternion = quaternion;
        Attributes = attributes;
        Level = new Level(experience);
        Stats = new Stats(attributes, Level);
        Health = Stats.MaxHealth;
        Mana = Stats.MaxMana;
        CharacterSkills = GetCharacterSkills();
        LastTick = DateTime.Now;
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