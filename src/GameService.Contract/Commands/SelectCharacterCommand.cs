namespace GameService.Contract.Commands;

public class SelectCharacterCommand : CommandBaseData
{
    public Guid CharacterId { get; set; }
}