using System.Collections.Generic;
using CoreLib.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace GameService.AntiCorruption.UserDomain
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public List<Character> CharacterList { get; set; }
    }
}