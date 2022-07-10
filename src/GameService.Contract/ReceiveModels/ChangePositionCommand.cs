using GameService.Common.ValueObjects;

namespace GameService.Contract.ReceiveModels;

public class ChangePositionCommand : CommandBaseData
{
    public Position Position { get; set; }
}