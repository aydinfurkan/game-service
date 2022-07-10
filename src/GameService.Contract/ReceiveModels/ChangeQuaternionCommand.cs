using GameService.Common.ValueObjects;

namespace GameService.Contract.ReceiveModels;

public class ChangeQuaternionCommand : CommandBaseData
{
    public Quaternion Quaternion { get; set; }
}