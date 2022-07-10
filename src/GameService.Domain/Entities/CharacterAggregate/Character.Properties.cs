using GameService.Common.ValueObjects;
using GameService.Contract.Enums;
using GameService.Domain.Components;

namespace GameService.Domain.Entities.CharacterAggregate;

public partial class Character
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
}