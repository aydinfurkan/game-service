using System;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Entity
{
    public class Character
    {
        public Guid CharacterId;
        public string CharacterName;
        public string CharacterClass;
        public Position Position;
        public Quaternion Quaternion;

        public Character(Guid characterId, string characterName, string characterClass, Position position, Quaternion quaternion)
        {
            CharacterId = characterId;
            CharacterName = characterName;
            CharacterClass = characterClass;
            Position = position;
            Quaternion = quaternion;
        }
    }
}