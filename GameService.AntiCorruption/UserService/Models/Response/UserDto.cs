using System;
using System.Collections.Generic;
using System.Linq;
using GameService.Domain.Entities;
using Character = GameService.Infrastructure.Protocol.CommonModels.Character;

namespace GameService.AntiCorruption.UserService.Models.Response
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public List<CharacterDto> CharacterList { get; set; }

        public User ToModel()
        {
            return new User(Id, Name, Surname, Email, 
                CharacterList.Select(x => x.ToModel()).ToList());
        }        
    }
}