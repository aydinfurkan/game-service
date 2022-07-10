
namespace GameService.Contract.ReceiveModels;

public class VerificationCommand : CommandBaseData
{
    public string PToken { get; set; }
    
    public Guid CharacterId { get; set; }
}