using GameService.Domain.Entities;
using GameService.Domain.Skills;

namespace GameService.Domain.ValueObjects
{
    public class Skill
    {
        public static readonly Skill BasicRangedAttack = new Skill(0, 0, 10, 1000);

        public readonly int Code;
        public readonly int ManaCost;
        public readonly int Range;
        public readonly double BaseCooldown;
        private Skill(int code, int manaCost, int range, double baseCooldown)
        {
            Code = code;
            ManaCost = manaCost;
            Range = range;
            BaseCooldown = baseCooldown;
        }

        public ICastSkill Cast(Character user, Character target)
        {
            return Code switch
            {
                0 => new CastBasicAttack(user, target, this),
                _ => null
            };
        }
    }
}