using GameService.Common.ValueObjects;

namespace GameService.Contract.ResponseModels;

public class PositionModel : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public Position Position  { get; set; }
}