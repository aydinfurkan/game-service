
namespace GameService.Contract.ReceiveModels;

public class VerificationModel : ReceiveModelData
{
    public string PToken { get; set; }
    
    public Guid CharacterId { get; set; }
}