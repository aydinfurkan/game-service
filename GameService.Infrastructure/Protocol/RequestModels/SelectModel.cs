using System;

namespace GameService.Infrastructure.Protocol.RequestModels
{
    public class SelectCharacterModel : RequestModelBase
    {
        public Guid Character { get; set; }
    }
}