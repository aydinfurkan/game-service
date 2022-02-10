using System.Collections.Generic;

namespace GameService.Domain.ValueObjects
{
    public class Skill
    {
        public static Skill BasicRangedAttack = new Skill(0, 0, 10);
        
        public static List<Skill> All = new ()
        {
            BasicRangedAttack
        };

        public int Code;
        public int ManaCost;
        public int Range;
        public Skill(int code, int manaCost, int range)
        {
            Code = code;
            ManaCost = manaCost;
            Range = range;
        }
    }
}