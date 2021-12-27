using System;

namespace GameService.Controller.RequestModel
{
    public class VerifyUserRequestModel
    {
        public string Token { get; set; }
        public Guid CharacterId { get; set; }
        public string CharacterName { get; set; }
        public string CharacterClass { get; set; }
    }
}