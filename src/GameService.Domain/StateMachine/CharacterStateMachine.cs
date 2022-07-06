using GameService.Domain.Entities;
using GameService.Domain.State;
using Stateless;

namespace GameService.Domain.StateMachine;

public class CharacterStateMachine
{
    private readonly StateMachine<CharacterState, string> _machine;
    private readonly Character _character;
    private CharacterState _currentState;
    
    public CharacterStateMachine(Character character)
    {
        _character = character;
        _machine = new StateMachine<CharacterState, string>(() => _currentState, c => _currentState = c);
        Configure();
    }

    private void Configure()
    {
        this._machine.Configure(CharacterState.Idle)
            .Permit(nameof(MoveStateModel));
    }
    
}