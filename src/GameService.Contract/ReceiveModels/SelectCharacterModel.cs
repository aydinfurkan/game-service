namespace GameService.Contract.ReceiveModels;

public class SelectCharacterModel : ReceiveModelData
{
    public Guid CharacterId { get; set; }
}