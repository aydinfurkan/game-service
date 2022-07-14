
namespace GameService.Contract.Commands;

public class VerificationCommand : CommandBaseData
{
    public string PToken { get; set; }
    
    public Guid CharacterId { get; set; }
}