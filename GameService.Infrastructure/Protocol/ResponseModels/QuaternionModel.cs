using System;
using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.ResponseModels;

public class QuaternionModel : ResponseModelBase
{
    public Guid CharacterId;
    public Quaternion Quaternion { get; set; }
        
    public QuaternionModel(Guid characterId, Quaternion quaternion)
    {
        CharacterId = characterId;
        Quaternion = quaternion;
    }
        
}