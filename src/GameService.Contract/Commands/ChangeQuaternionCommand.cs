using GameService.Common.ValueObjects;

namespace GameService.Contract.Commands;

public class ChangeQuaternionCommand : CommandBaseData
{
    public Quaternion Quaternion { get; set; }
}