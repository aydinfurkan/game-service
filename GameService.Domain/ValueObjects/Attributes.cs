namespace GameService.Domain.ValueObjects
{
    public class Attributes
    {
        public int Strength;
        public int Vitality;
        public int Dexterity;
        public int Intelligent;
        public int Wisdom;
        public int Defense;
        
        public Attributes(int strength, int vitality, int dexterity, int intelligent, int wisdom, int defense)
        {
            Strength = strength;
            Vitality = vitality;
            Dexterity = dexterity;
            Intelligent = intelligent;
            Wisdom = wisdom;
            Defense = defense;
        }

    }
}