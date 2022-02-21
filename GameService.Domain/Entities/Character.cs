using System;
using System.Collections.Generic;
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
        public List<CharacterSkill> CharacterSkills;
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
            CharacterSkills = new List<CharacterSkill>
            {
                new CharacterSkill(Skill.BasicRangedAttack, this)
            };
        }
        
        public bool TryCastSkill(int skillCode, out ICastSkill castSkill)
        {
            var characterSkill = CharacterSkills.FirstOrDefault(x => x.Skill.Code == skillCode);

            if (characterSkill != null) return characterSkill.TryCast(Target, out castSkill);
            
            castSkill = null;
            return false;

        }
    }
}