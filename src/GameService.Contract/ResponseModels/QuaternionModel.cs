using GameService.Common.ValueObjects;

namespace GameService.Contract.ResponseModels;

public class QuaternionModel : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public Quaternion Quaternion  { get; set; }
}