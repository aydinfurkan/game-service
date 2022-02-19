using System;
using GameService.Domain.Skills;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Entities
{
    public class CharacterSkill
    {
        public Skill Skill;
        public int Level;
        public DateTime UsedAt;
        public DateTime CanBeUsedAt;

        public CharacterSkill(Skill skill)
        {
            Skill = skill;
            Level = 1;
            UsedAt = DateTime.UtcNow;
            CanBeUsedAt = DateTime.UtcNow;
        }
        
        public bool TryCast(Character user, Character target, out ICastSkill castSkill)
        {
            if (!CanBeCasted())
            {
                castSkill = null;
                return false;
            }

            UsedAt = DateTime.UtcNow;
            CanBeUsedAt = DateTime.UtcNow.AddSeconds(CalculateCooldown(user));
            castSkill = Skill.Cast(user, target);
            return true;
        }

        private bool CanBeCasted()
        {
            return CanBeUsedAt <= DateTime.UtcNow;
        }

        private double CalculateCooldown(Character user)
        {
            return Skill.BaseCooldown * (1 - user.Stats.CdReduction);
        }
    }
}