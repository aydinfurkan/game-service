using System;

namespace GameService.Infrastructure.Protocol.ResponseModels;

public class MoveStateModel : ResponseModelBase
{
    public Guid CharacterId;
    public string MoveState;

    public MoveStateModel(Guid characterId, string moveState)
    {
        CharacterId = characterId;
        MoveState = moveState;
    }
}
public class JumpStateModel : ResponseModelBase
{
    public Guid CharacterId;
    public int JumpState;

    public JumpStateModel(Guid characterId, int jumpState)
    {
        CharacterId = characterId;
        JumpState = jumpState;
    }
}
public class SkillStateModel : ResponseModelBase
{
    public Guid CharacterId;
    public Guid TargetCharacterId;
    public int SkillState;

    public SkillStateModel(Guid characterId, Guid targetCharacterId, int skillState)
    {
        CharacterId = characterId;
        TargetCharacterId = targetCharacterId;
        SkillState = skillState;
    }
}