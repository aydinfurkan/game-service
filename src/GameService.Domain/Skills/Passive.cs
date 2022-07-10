using GameService.Domain.Entities;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.Domain.Skills.Results;

namespace GameService.Domain.Skills;

public class Passive : IChange
{
    private readonly Character _user;
    private readonly double _delta;

    public Passive(Character user, double delta)
    {
        _user = user;
        _delta = delta;
    }
    public bool HealthChange(out HealthResult result)
    {
        if (Math.Abs(_user.Health - _user.Stats.MaxHealth) < 10e-2)
        {
            result = null;
            return false;
        }
            
        _user.Health += _user.Stats.HealthRegen * _delta;
        if (_user.Health > _user.Stats.MaxHealth) _user.Health = _user.Stats.MaxHealth;
            
        result = new HealthResult(_user.Id, _user.Health);
        return true;
    }

    public bool ManaChange(out ManaResult result)
    {
        if (Math.Abs(_user.Mana - _user.Stats.MaxMana) < 10e-2)
        {
            result = null;
            return false;
        }
        _user.Mana += _user.Stats.ManaRegen * _delta;
        if (_user.Mana > _user.Stats.MaxMana) _user.Mana = _user.Stats.MaxMana;   
            
        result = new ManaResult(_user.Id, _user.Mana);
        return true;
    }

    public bool StatsChange(out StatsResult result)
    {
        result = null;
        return false;
    }
}