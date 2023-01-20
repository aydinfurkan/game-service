using GameService.Common.Enums;
using GameService.Common.Logger;
using GameService.Contract.Commands;
using GameService.Domain.Entities.CharacterAggregate;
using Microsoft.Extensions.Logging;
using Stateless;

namespace GameService.Domain.StateMachine;

public class SkillStateMachine
{
    private readonly StateMachine<SkillState, string> _machine;
    public SkillState CurrentSkillState = SkillState.Ready;
    private LearnedSkill? _currentLearnedSkill;
    private Character? _currentTarget;
    private readonly ILogger<SkillStateMachine> _logger;
    
    public SkillStateMachine()
    {
        _machine = new StateMachine<SkillState, string>(() => CurrentSkillState, c => CurrentSkillState = c);
        _logger = ApplicationLogging.CreateLogger<SkillStateMachine>();
        Configure();
    }

    private void Configure()
    {
        this._machine.Configure(SkillState.Ready)
            .PermitIf(nameof(ChangeSkillStateCommand), SkillState.Casting, CanBeCasting); 

        this._machine.Configure(SkillState.Casting)
            .PermitReentryIf(nameof(ChangeSkillStateCommand), CanBeCasting)
            .PermitIf(nameof(CastSkillCommand), SkillState.Casted, CanBeCasted);

        this._machine.Configure(SkillState.Casted)
            .PermitIf(nameof(ChangeSkillStateCommand), SkillState.Casting, CanBeCasting)
            .PermitIf(nameof(ExecuteSkillEffectCommand), SkillState.Executed, CanBeExecuted);

        this._machine.Configure(SkillState.Executed)
            .PermitIf(nameof(ChangeSkillStateCommand), SkillState.Casting, CanBeCasting);
    }

    public void Fire(CommandBaseData command, LearnedSkill? learnedSkill, Character? target)
    {
        _currentLearnedSkill = learnedSkill;
        _currentTarget = target;

        try
        {
            this._machine.Fire(command.Name);
        }
        catch (Exception e)
        {
            _logger.LogDebug("{Exception}", e.Message);
        }
    }
    
    private bool CanBeCasting()
    {
        return _currentLearnedSkill?.CanBeCasting(_currentTarget) ?? false;
    }
    
    private bool CanBeCasted()
    {
        return _currentLearnedSkill?.CanBeCasted() ?? false;
    }
    
    private bool CanBeExecuted()
    {
        return _currentLearnedSkill?.CanBeExecuted(_currentTarget) ?? false;
    }
}