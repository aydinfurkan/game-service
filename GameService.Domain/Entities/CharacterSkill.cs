using System;
using GameService.Domain.Skills;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Entities
{
    public class CharacterSkill
    {
        public Skill Skill;
        public Character User;
        public int Level;
        public DateTime UsedAt;
        public DateTime CanBeUsedAt;

        public CharacterSkill(Skill skill, Character user)
        {
            Skill = skill;
            User = user;
            Level = 1;
            UsedAt = DateTime.UtcNow;
            CanBeUsedAt = DateTime.UtcNow;
        }
        
        public bool TryCast(Character target, out ICastSkill castSkill)
        {
            if (!CanBeCasted(target))
            {
                castSkill = null;
                return false;
            }

            castSkill = Skill.Cast(User, target);
            UsedAt = DateTime.UtcNow;
            CanBeUsedAt = DateTime.UtcNow.AddMilliseconds(CalculateCooldown());
            return true;
        }

        private bool CanBeCasted(Character target)
        {
            return IsUserAlive() && IsTargetAlive(target) && IsManaEnough() && IsRangeEnough(target) && IsNotOnCooldown();
        }

        private double CalculateCooldown()
        {
            return Skill.BaseCooldown * (1 - User.Stats.CdReduction);
        }
        
        private bool IsNotOnCooldown()
        {
            return CanBeUsedAt <= DateTime.UtcNow;
        }
        private bool IsUserAlive()
        {
            return User.Health > 0;
        }
        private bool IsTargetAlive(Character target)
        {
            return target.Health > 0;
        }
        private bool IsManaEnough()
        {
            return User.Mana >= Skill.ManaCost;
        }
        private bool IsRangeEnough(Character target)
        {
            return User.Position.DistanceTo(target.Position) <= Skill.Range;
        }

    }
}