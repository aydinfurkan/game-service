using GameService.Contract.CommonModels;

namespace GameService.Contract.ResponseModels;

public class ClientCharacter : ResponseModelData
{
    public UserCharacterDto UserCharacterDto { get; set; }
}
    
public class ActiveCharacters : ResponseModelData
{
    public List<CharacterDto> Characters { get; set; }
}
    
public class AddCharacter : ResponseModelData
{
    public CharacterDto CharacterDto { get; set; }
}
    
public class DeleteCharacter : ResponseModelData
{
    public Guid CharacterId { get; set; }
}