using GameService.Common.ValueObjects;

namespace GameService.Contract.CommonModels;

public class UserCharacterDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
    public Position Position { get; set; }
    public Quaternion Quaternion { get; set; }
    public Stats Stats { get; set; }
    public Attributes Attributes { get; set; }
    public double Health { get; set; }
    public double Mana { get; set; }
}