using System;
using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.CommonModels;

public class Character
{
    public Guid Id;
    public string Name;
    public string Class;
    public Position Position;
    public Quaternion Quaternion;
    public double MaxHealth;
    public double Health;
    public double MaxMana;
    public double Mana;

    public Character(Domain.Entities.Character character)
    {
        Id = character.Id;
        Name = character.Name;
        Class = character.Class;
        Position = character.Position;
        Quaternion = character.Quaternion;
        MaxHealth = character.Stats.MaxHealth;
        Health = character.Health;
        MaxMana = character.Stats.MaxMana;
        Mana = character.Mana;
    }
}