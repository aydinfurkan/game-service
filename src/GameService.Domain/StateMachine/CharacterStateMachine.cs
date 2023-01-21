using GameService.Common.Enums;
using GameService.Common.Logger;
using GameService.Contract.Commands;
using GameService.Domain.Entities.CharacterAggregate;
using Microsoft.Extensions.Logging;
using Stateless;

namespace GameService.Domain.StateMachine;

public class CharacterStateMachine
{
    private readonly StateMachine<CharacterState, string> _machine;
    public CharacterState CurrentCharacterState = CharacterState.Idle;
    private int? _currentSkillState;
    private readonly ILogger<CharacterStateMachine> _logger;
    
    public CharacterStateMachine()
    {
        _machine = new StateMachine<CharacterState, string>(() => CurrentCharacterState, c => CurrentCharacterState = c);
        _logger = ApplicationLogging.CreateLogger<CharacterStateMachine>();
        Configure();
    }

    private void Configure()
    {
        this._machine.Configure(CharacterState.Idle)
            .PermitReentry(nameof(ChangePositionCommand))
            .PermitReentry(nameof(ChangeQuaternionCommand))
            .PermitReentry(nameof(SelectCharacterCommand))
            .PermitIf(nameof(ChangeSkillStateCommand), CharacterState.Casting)
            .PermitReentryIf(nameof(ChangeSkillStateCommand), () => !IsSkillCastTimeExist())
            .PermitReentry(nameof(ChangeJumpStateCommand))
            .Permit(nameof(ChangeMoveStateCommand), CharacterState.Moving);

        this._machine.Configure(CharacterState.Moving)
            .PermitReentry(nameof(ChangePositionCommand))
            .PermitReentry(nameof(ChangeQuaternionCommand))
            .PermitReentry(nameof(SelectCharacterCommand))
            .PermitIf(nameof(ChangeSkillStateCommand), CharacterState.Casting, IsSkillCastTimeExist)
            .PermitReentryIf(nameof(ChangeSkillStateCommand), () => !IsSkillCastTimeExist())
            .PermitReentry(nameof(ChangeMoveStateCommand))
            .PermitReentry(nameof(ChangeJumpStateCommand));

        this._machine.Configure(CharacterState.Casting)
            .PermitReentry(nameof(ChangePositionCommand))
            .PermitReentry(nameof(ChangeQuaternionCommand))
            .PermitReentry(nameof(SelectCharacterCommand))
            .Permit(nameof(ChangeMoveStateCommand), CharacterState.Moving)
            .Permit(nameof(ChangeJumpStateCommand), CharacterState.Moving)
            .Permit(nameof(CastSkillCommand), CharacterState.Idle);

        this._machine.Configure(CharacterState.Dead);
    }

    public void Fire(CommandBaseData command)
    {
        try
        {
            this._machine.Fire(command.Name);
        }
        catch (Exception e)
        {
            _logger.LogDebug("{Exception}", e.Message);
        }
    }
    
    public void Fire(CommandBaseData command, int skillState)
    {
        _currentSkillState = skillState;
        try
        {
            this._machine.Fire(command.Name);
        }
        catch (Exception e)
        {
            _logger.LogDebug("{Exception}", e.Message);
        }
    }
    
    private bool IsSkillCastTimeExist()
    {
        return Skill.All.FirstOrDefault(s => s.Code == _currentSkillState)?.CastTime > 0;
    }
    
}