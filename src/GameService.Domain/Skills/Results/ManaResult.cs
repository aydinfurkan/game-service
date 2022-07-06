namespace GameService.Domain.Skills.Results;

public class ManaResult
{
    public Guid CharacterId;
    public double Mana;

    public ManaResult(Guid characterId, double mana)
    {
        CharacterId = characterId;
        Mana = mana;
    }
}