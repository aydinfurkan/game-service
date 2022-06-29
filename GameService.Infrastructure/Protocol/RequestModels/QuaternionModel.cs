using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.RequestModels;

public class QuaternionModel : RequestModelBase
{
    public Quaternion Quaternion { get; set; }
}