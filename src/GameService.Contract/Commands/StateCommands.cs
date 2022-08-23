namespace GameService.Contract.Commands;

public class ChangeMoveStateCommand : CommandBaseData
{
    public string MoveState { get; set; }
}
public class ChangeJumpStateCommand : CommandBaseData
{
    public int JumpState { get; set; }
}