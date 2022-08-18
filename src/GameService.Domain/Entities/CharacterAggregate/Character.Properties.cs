using GameService.Common.ValueObjects;
using GameService.Domain.StateMachine;

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
        
    public Character? Target;
    public List<LearnedSkill> LearnedSkills;
    public List<ActiveEffect> ActiveEffects;
    public LearnedSkill? CurrentCastingSkill = null;

    public string MoveState = "";
    public int JumpState;
    public int SkillState;
    
    public CharacterStateMachine CharacterStateMachine;
    
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
        LearnedSkills = GetLearnedSkills();
        LastTick = DateTime.Now;
        CharacterStateMachine = new CharacterStateMachine();
    }
}