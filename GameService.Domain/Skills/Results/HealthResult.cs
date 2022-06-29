using System;

namespace GameService.Domain.Skills.Results;

public class HealthResult
{
    public Guid CharacterId;
    public double Health;

    public HealthResult(Guid characterId, double health)
    {
        CharacterId = characterId;
        Health = health;
    }
}