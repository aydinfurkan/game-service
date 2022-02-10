using System;

namespace GameService.Infrastructure.Protocol.ResponseModels
{
    public class CharacterHealth : ResponseModelBase
    {
        public Guid CharacterId;
        public double Health;
        
        public CharacterHealth(Guid characterId, double health)
        {
            CharacterId = characterId;
            Health = health;
        }
    }
    public class CharacterMana : ResponseModelBase
    {
        public Guid CharacterId;
        public double Mana;
        
        public CharacterMana(Guid characterId, double mana)
        {
            CharacterId = characterId;
            Mana = mana;
        }
    }
    public class CharacterStats : ResponseModelBase
    {
        public Guid CharacterId;
        public double MaxHealth;
        public double MaxMana;

        public CharacterStats(Guid characterId, double maxHealth)
        {
            CharacterId = characterId;
            MaxHealth = maxHealth;
        }
    }
    public class CharacterLevel : ResponseModelBase
    {
        public Guid CharacterId;
        public int Level;
        
        public CharacterLevel(Guid characterId, int level)
        {
            CharacterId = characterId;
            Level = level;
        }
    }
}