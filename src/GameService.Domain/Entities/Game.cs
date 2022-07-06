using GameService.Common.ValueObjects;
using GameService.Domain.Components;

namespace GameService.Domain.Entities;

public class Game
{
    private Guid _id;
    private readonly ActiveCharacters _activeCharacters;
    public Game()
    {
        _id = Guid.NewGuid();
        _activeCharacters = new ActiveCharacters();
    }

    public bool AddCharacter(Character character)
    {
        return _activeCharacters.Add(character);
    }
        
    public bool DeleteCharacter(Character character)
    {
        return _activeCharacters.Delete(character);
    }
        
    public Character FindCharacterOrDefault(Guid id)
    {
        return _activeCharacters.FindOrDefault(id);
    }
        
    public List<Character> GetAllActiveCharacters()
    {
        return _activeCharacters.GetAllActiveCharacters();
    }

    public bool ChangeCharacterPosition(Guid id, Position position)
    {
        return _activeCharacters.ChangeCharacterPosition(id, position);
    }

    public bool ChangeCharacterQuaternion(Guid id, Quaternion quaternion)
    {
        return _activeCharacters.ChangeCharacterQuaternion(id, quaternion);
    }
    public bool ChangeMoveState(Guid id, string moveState)
    {
        return _activeCharacters.ChangeMoveState(id, moveState);
    }
    public bool ChangeJumpState(Guid id, int jumpState)
    {
        return _activeCharacters.ChangeJumpState(id, jumpState);
    }
}