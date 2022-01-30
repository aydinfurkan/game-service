using System;
using System.Collections.Generic;
using System.Linq;

namespace GameService.AntiCorruption.User.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public List<Character> CharacterList { get; set; }

        public Domain.Entities.User ToModel()
        {
            return new Domain.Entities.User(Id, Name, Surname, Email, 
                CharacterList.Select(x => x.ToModel()).ToList());
        }        
    }
}