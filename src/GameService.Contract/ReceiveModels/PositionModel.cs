using GameService.Common.ValueObjects;

namespace GameService.Contract.ReceiveModels;

public class PositionModel : ReceiveModelData
{
    public Position Position { get; set; }
}