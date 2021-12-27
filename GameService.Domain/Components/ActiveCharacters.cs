using System;
using System.Collections.Generic;
using System.Linq;
using GameService.Domain.Entity;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Components
{
    public class ActiveCharacters
    {
        private static object _lockObj = new ();
        private readonly Dictionary<Guid, Character> _characterList;

        public ActiveCharacters()
        {
            _characterList = new Dictionary<Guid, Character>();
        }

        public bool Add(Character character)
        {
            lock (_lockObj)
            {
                return _characterList.TryAdd(character.CharacterId, character);
            }
        }

        public bool Delete(Character character)
        {
            lock (_lockObj)
            {
                return _characterList.Remove(character.CharacterId);
            }
        }

        public Character FindOrDefault(Guid id)
        {
            lock (_lockObj)
            {
                var ok = _characterList.TryGetValue(id, out var character);
                return character;
            }
        }
        
        public List<Character> GetAllActiveCharacters()
        {
            lock (_lockObj)
            {
                return _characterList.Select(x => x.Value).ToList();
            }
        }

        public bool ChangeCharacterPosition(Guid id, Position position)
        {
            lock (_lockObj)
            {
                var ok = _characterList.TryGetValue(id, out var character);
                if (character == null) return false;
                character.Position = position;
                return true;
            }
        }

        public bool ChangeCharacterQuaternion(Guid id, Quaternion quaternion)
        {
            lock (_lockObj)
            {
                var ok = _characterList.TryGetValue(id, out var character);
                if (character == null) return false;
                character.Quaternion = quaternion;
                return true;
            }
        }
    }
}