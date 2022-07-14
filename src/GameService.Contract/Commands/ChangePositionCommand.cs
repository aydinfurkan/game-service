using GameService.Common.ValueObjects;

namespace GameService.Contract.Commands;

public class ChangePositionCommand : CommandBaseData
{
    public Position Position { get; set; }
}