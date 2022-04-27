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
        public bool IsDead;
        
        public Character Target;
        public List<CharacterSkill> CharacterSkills;
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

            if (characterSkill != null) return characterSkill.TryCast(Target, out change);
            
            change = null;
            return false;
        }

        private List<CharacterSkill> GetCharacterSkills()
        {
            switch (Class)
            {
                case "warrior":
                    return new List<CharacterSkill>
                    {
                        new (Skill.WarriorBasicAttack, this)
                    };
                case "archer":
                    return new List<CharacterSkill>
                    {
                        new (Skill.ArcherBasicAttack, this)
                    };
                case "mage":
                    return new List<CharacterSkill>
                    {
                        new (Skill.MageBasicAttack, this)
                    };
                case "healer":
                    return new List<CharacterSkill>
                    {
                        new (Skill.HealerBasicAttack, this)
                    };
                default:
                    return new List<CharacterSkill>
                    {
                        new(Skill.WarriorBasicAttack, this)
                    };
            }
        }

    }
}