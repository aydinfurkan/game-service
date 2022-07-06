using GameService.Common.ValueObjects;

namespace GameService.Contract.ReceiveModels;

public class QuaternionModel : ReceiveModelData
{
    public Quaternion Quaternion { get; set; }
}