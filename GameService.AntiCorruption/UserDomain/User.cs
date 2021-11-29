using System.Collections.Generic;

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