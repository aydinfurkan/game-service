using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.RequestModels;

public class PositionModel : RequestModelBase
{
    public Position Position { get; set; }
}