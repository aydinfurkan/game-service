using System;
using System.Collections.Generic;
using System.Linq;
using GameService.Domain.Entities;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Components;

public class ActiveCharacters
{
    private readonly object _lockObj = new ();
    private readonly Dictionary<Guid, Character> _characterList;

    public ActiveCharacters()
    {
        _characterList = new Dictionary<Guid, Character>();
    }

    public bool Add(Character character)
    {
        lock (_lockObj)
        {
            return _characterList.TryAdd(character.Id, character);
        }
    }

    public bool Delete(Character character)
    {
        lock (_lockObj)
        {
            return _characterList.Remove(character.Id);
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
    public bool ChangeMoveState(Guid id, string moveState)
    {
        lock (_lockObj)
        {
            var ok = _characterList.TryGetValue(id, out var character);
            if (character == null) return false;
            character.MoveState = moveState;
            return true;
        }
    }
    public bool ChangeJumpState(Guid id, int jumpState)
    {
        lock (_lockObj)
        {
            var ok = _characterList.TryGetValue(id, out var character);
            if (character == null) return false;
            character.JumpState = jumpState;
            return true;
        }
    }
}