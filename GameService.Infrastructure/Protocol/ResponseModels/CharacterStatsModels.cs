using System;

namespace GameService.Infrastructure.Protocol.ResponseModels
{
    public class CharacterHealth : ResponseModelBase
    {
        public Guid CharacterId;
        public decimal Health;
        
        public CharacterHealth(Guid characterId, decimal health)
        {
            CharacterId = characterId;
            Health = health;
        }
    }
    public class CharacterMana : ResponseModelBase
    {
        public Guid CharacterId;
        public decimal Mana;
        
        public CharacterMana(Guid characterId, decimal mana)
        {
            CharacterId = characterId;
            Mana = mana;
        }
    }
    public class CharacterStats : ResponseModelBase
    {
        public Guid CharacterId;
        public decimal MaxHealth;
        public decimal MaxMana;

        public CharacterStats(Guid characterId, decimal maxHealth)
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