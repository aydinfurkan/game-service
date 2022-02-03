using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.ResponseModels
{
    public class PositionModel : ResponseModelBase
    {
        public Position Position { get; set; }

        public PositionModel(Position position)
        {
            Position = position;
        }

    }
}