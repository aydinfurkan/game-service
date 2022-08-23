using GameService.Common.Enums;
using GameService.Contract.Commands;
using GameService.Contract.CommonModels;
using GameService.Domain.Skills;

namespace GameService.Domain.Entities.CharacterAggregate;

public partial class Character
{
    public UserCharacter ToUserCharacter()
    {
        return new UserCharacter
        {
            Id = Id,
            Name = Name,
            Class = Class,
            Position = Position,
            Quaternion = Quaternion,
            Stats = Stats,
            Attributes = Attributes,
            Health = Health,
            Mana = Mana
        };
    }
    
    public Contract.CommonModels.Character ToCharacter()
    {
        return new Contract.CommonModels.Character
        {
            Id = Id,
            Name = Name,
            Class = Class,
            Position = Position,
            Quaternion = Quaternion,
            MaxHealth = Stats.MaxHealth,
            MaxMana = Stats.MaxMana,
            Health = Health,
            Mana = Mana
        };
    }

    public void ChangePosition(ChangePositionCommand command)
    {
        CharacterStateMachine.Fire(command);
        
        Position = command.Position;
    }
    
    public void ChangeQuaternion(ChangeQuaternionCommand command)
    {
        CharacterStateMachine.Fire(command);
        
        Quaternion = command.Quaternion;
    }

    public void ChangeJumpState(ChangeJumpStateCommand command)
    {
        CharacterStateMachine.Fire(command);
        
        JumpState = command.JumpState;
    }
    
    public void ChangeMoveState(ChangeMoveStateCommand command)
    {
        CharacterStateMachine.Fire(command);
        
        MoveState = command.MoveState;
    }

    public void SelectCharacter(SelectCharacterCommand command, Game game)
    {
        CharacterStateMachine.Fire(command);
        
        Target = game.GetAllActiveCharacters().FirstOrDefault(x => x.Id == command.CharacterId);
    }
    
    public bool TryTick(DateTime signalTime, out List<IChange> changeList)
    {
        var delta = (signalTime - LastTick).TotalMilliseconds / 1000.0;
        LastTick = signalTime;

        changeList = new List<IChange>();
        
        if (Health < 10e-2)
        {
            IsDead = true;
            return false;
        }
        
        if (CharacterStateMachine.CurrentCharacterState == CharacterState.Casting)
        {
            var ok = TryExecuteSkill(out var change);
            
            if (ok && change != null)
            {
                changeList.Add(change);
            }
        }
            
        var passive = new Passive(this, delta);
        changeList.Add(passive);
        return true;
    }
        
    public bool TryCastSkill(CastSkillCommand command, out IChange? change)
    {
        CharacterStateMachine.Fire(command, command.SkillState);
        
        SkillState = command.SkillState;
        
        CurrentCastingSkill = LearnedSkills.FirstOrDefault(x => x.Skill.Code == SkillState);

        if (CurrentCastingSkill == null)
        {
            change = null;
            return false;
        }

        return CurrentCastingSkill.TryCast(Target, out change);
    }

    private bool TryExecuteSkill(out IChange? change)
    {
        if (CurrentCastingSkill == null)
        {
            change = null;
            return false;
        }
        
        var ok = CurrentCastingSkill.TryExecute(Target, out change);
        if (ok)
        {
            var command = new ExecuteCurrentCastingSkillCommand();
            CharacterStateMachine.Fire(command);
        }
        return ok;
    }

    private List<LearnedSkill> GetLearnedSkills()
    {
        return Class switch
        {
            "warrior" => new List<LearnedSkill>
            {
                new(Skill.WarriorBasicAttack, this)
            },
            "archer" => new List<LearnedSkill>
            {
                new(Skill.ArcherBasicAttack, this)
            },
            "mage" => new List<LearnedSkill>
            {
                new(Skill.MageBasicAttack, this), 
                new(Skill.Fireball, this)
            },
            "healer" => new List<LearnedSkill>
            {
                new(Skill.HealerBasicAttack, this)
            },
            _ => new List<LearnedSkill>
            {
                new(Skill.WarriorBasicAttack, this)
            }
        };
    }
}