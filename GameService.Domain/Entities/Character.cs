using System;
using System.Linq;
using GameService.Domain.Skills;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Entities
{
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
        
        public Character Target;
        public string MoveState;
        public int JumpState;
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
        }
        
        public ICastSkill CastSkill(int skillCode)
        {
            var skill = Skill.All.FirstOrDefault(x => x.Code == skillCode);

            ICastSkill castSkill = skillCode switch
            {
                0 => new CastBasicAttack(this, Target),
                _ => null
            };
            return castSkill;
        }
    }
}