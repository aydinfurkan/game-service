using System;

namespace GameService.Infrastructure.Protocol.RequestModels
{
    public class SelectCharacterModel : RequestModelBase
    {
        public Guid CharacterId { get; set; }
    }
}