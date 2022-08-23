using GameService.Contract.CommonModels;

namespace GameService.Contract.ResponseModels;

public class ClientCharacter : ResponseModelData
{
    public UserCharacter UserCharacter { get; set; }
}
    
public class ActiveCharacters : ResponseModelData
{
    public List<Character> Characters { get; set; }
}
    
public class AddCharacter : ResponseModelData
{
    public Character Character { get; set; }
}
    
public class DeleteCharacter : ResponseModelData
{
    public Guid CharacterId { get; set; }
}