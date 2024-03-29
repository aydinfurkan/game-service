using GameService.Domain.Entities.CharacterAggregate;

namespace GameService.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<Character> CharacterList { get; set; }
        
    public User(Guid id, string name, string surname, string email, List<Character> characterList)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        CharacterList = characterList;
    }

}