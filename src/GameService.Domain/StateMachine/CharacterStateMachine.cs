using GameService.Contract.Commands;
using GameService.Contract.Enums;
using GameService.Domain.Entities.CharacterAggregate;
using Stateless;

namespace GameService.Domain.StateMachine;

public class CharacterStateMachine
{
    private readonly StateMachine<CharacterState, string> _machine;
    public CharacterState CurrentCharacterState = CharacterState.Idle;
    private CharacterState _lastCharacterState = CharacterState.Idle;
    private int? _currentSkillState;
    
    public CharacterStateMachine()
    {
        _machine = new StateMachine<CharacterState, string>(() => CurrentCharacterState, c => CurrentCharacterState = c);
        Configure();
    }

    private void Configure()
    {
        this._machine.Configure(CharacterState.Idle)
            .PermitReentry(nameof(ChangePositionCommand))
            .PermitReentry(nameof(ChangeQuaternionCommand))
            .PermitReentry(nameof(SelectCharacterCommand))
            .PermitIf(nameof(CastSkillCommand), CharacterState.Casting, IsSkillCastTimeExist)
            .PermitReentryIf(nameof(CastSkillCommand), () => !IsSkillCastTimeExist())
            .PermitReentry(nameof(ChangeJumpStateCommand))
            .Permit(nameof(ChangeMoveStateCommand), CharacterState.Moving);

        this._machine.Configure(CharacterState.Moving)
            .PermitReentry(nameof(ChangePositionCommand))
            .PermitReentry(nameof(ChangeQuaternionCommand))
            .PermitReentry(nameof(SelectCharacterCommand))
            .PermitIf(nameof(CastSkillCommand), CharacterState.Casting, IsSkillCastTimeExist)
            .PermitReentryIf(nameof(CastSkillCommand), () => !IsSkillCastTimeExist())
            .PermitReentry(nameof(ChangeMoveStateCommand))
            .PermitReentry(nameof(ChangeJumpStateCommand));

        this._machine.Configure(CharacterState.Casting)
            .PermitReentry(nameof(ChangePositionCommand))
            .PermitReentry(nameof(ChangeQuaternionCommand))
            .PermitReentry(nameof(SelectCharacterCommand))
            .Permit(nameof(ChangeMoveStateCommand), CharacterState.Moving)
            .Permit(nameof(ChangeJumpStateCommand), CharacterState.Moving)
            .Permit(nameof(ExecuteCurrentCastingSkillCommand), CharacterState.Moving);

        this._machine.Configure(CharacterState.Dead);
    }

    public void Fire(CommandBaseData command)
    {
        if (CurrentCharacterState != _lastCharacterState)
        {
            Console.WriteLine(_lastCharacterState + " -> " + CurrentCharacterState);
            _lastCharacterState = CurrentCharacterState;
        }
        this._machine.Fire(command.Name);
    }
    
    public void Fire(CommandBaseData command, int skillState)
    {
        _currentSkillState = skillState;
        this._machine.Fire(command.Name);
    }
    
    private bool IsSkillCastTimeExist()
    {
        return Skill.All.FirstOrDefault(s => s.Code == _currentSkillState)?.CastTime > 0;
    }
    
}