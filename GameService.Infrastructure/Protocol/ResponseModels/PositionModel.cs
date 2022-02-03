using System;
using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.ResponseModels
{
    public class PositionModel : ResponseModelBase
    {
        public Guid CharacterId;
        public Position Position { get; set; }

        public PositionModel(Guid characterId, Position position)
        {
            CharacterId = characterId;
            Position = position;
        }

    }
}