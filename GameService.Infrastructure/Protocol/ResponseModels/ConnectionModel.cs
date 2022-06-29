using System;
using System.Collections.Generic;
using GameService.Infrastructure.Protocol.CommonModels;

namespace GameService.Infrastructure.Protocol.ResponseModels;

public class ClientCharacter : ResponseModelBase
{
    public UserCharacter UserCharacter;

    public ClientCharacter(UserCharacter userCharacter)
    {
        UserCharacter = userCharacter;
    }
}
    
public class ActiveCharacters : ResponseModelBase
{
    public List<Character> Characters;

    public ActiveCharacters(List<Character> characters)
    {
        Characters = characters;
    }
}
    
public class AddCharacter : ResponseModelBase
{
    public Character Character;

    public AddCharacter(Character character)
    {
        Character = character;
    }
}
    
public class DeleteCharacter : ResponseModelBase
{
    public Guid CharacterId;

    public DeleteCharacter(Guid characterId)
    {
        CharacterId = characterId;
    }
}