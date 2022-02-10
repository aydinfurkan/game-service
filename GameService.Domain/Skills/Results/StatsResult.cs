using System;
using GameService.Domain.ValueObjects;

namespace GameService.Domain.Skills.Results
{
    public class StatsResult
    {
        public Guid CharacterId;
        public Stats Stats;

        public StatsResult(Guid characterId, Stats stats)
        {
            CharacterId = characterId;
            Stats = stats;
        }
    }
}