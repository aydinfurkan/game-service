namespace GameService.Contract.Commands;

public class ChangeMoveStateCommand : CommandBaseData
{
    public string MoveState { get; set; }
}
public class ChangeJumpStateCommand : CommandBaseData
{
    public int JumpState { get; set; }
}

public class ChangeSkillStateCommand : CommandBaseData
{
    public int SkillState { get; set; }
}