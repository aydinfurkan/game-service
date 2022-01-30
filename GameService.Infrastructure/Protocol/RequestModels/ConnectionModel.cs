
using System;

namespace GameService.Infrastructure.Protocol.RequestModels
{
    public class VerificationModel : RequestModelBase
    {
        public string PToken { get; set; }
        public Guid CharacterId { get; set; }

    }
}