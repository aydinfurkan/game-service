namespace GameService.Contract.ReceiveModels;

public class SelectCharacterCommand : CommandBaseData
{
    public Guid CharacterId { get; set; }
}