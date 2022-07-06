namespace GameService.Contract.ReceiveModels;

public class MoveStateModel : ReceiveModelData
{
    public string MoveState { get; set; }
}
public class JumpStateModel : ReceiveModelData
{
    public int JumpState { get; set; }
}
public class SkillStateModel : ReceiveModelData
{
    public int SkillState { get; set; }
}