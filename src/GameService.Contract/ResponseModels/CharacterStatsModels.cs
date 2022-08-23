namespace GameService.Contract.ResponseModels;

public class CharacterHealth : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public double Health { get; set; }
}
public class CharacterMana : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public double Mana { get; set; }
}
public class CharacterStats : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public double MaxHealth { get; set; }
    public double MaxMana { get; set; }
}
public class CharacterLevel : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public int Level { get; set; }
}