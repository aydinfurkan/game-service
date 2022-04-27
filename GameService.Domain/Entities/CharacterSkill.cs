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
        
        public bool TryCast(Character target, out IChange change)
        {
            if (!CanBeCasted(target))
            {
                change = null;
                return false;
            }

            change = Skill.Cast(User, target);
            UsedAt = DateTime.UtcNow;
            CanBeUsedAt = DateTime.UtcNow.AddMilliseconds(CalculateCooldown());
            return true;
        }

        private bool CanBeCasted(Character target)
        {
            if (target == null) return false;
            
            return IsUserAlive() && IsTargetValid(target) && IsTargetAlive(target) &&
                   IsManaEnough() && IsInRange(target) && 
                   IsNotOnCooldown();
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
            return !User.IsDead;
        }
        private bool IsTargetValid(Character target)
        {
            return Skill.SelfCast || target.Id != User.Id;
        }
        private bool IsTargetAlive(Character target)
        {
            return !target.IsDead;
        }
        private bool IsManaEnough()
        {
            return User.Mana >= Skill.ManaCost;
        }
        private bool IsInRange(Character target)
        {
            return User.Position.DistanceTo(target.Position) <= Skill.Range;
        }

    }
}