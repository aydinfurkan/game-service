namespace GameService.Infrastructure.Protocol.RequestModels;

public class MoveStateModel : RequestModelBase
{
    public string MoveState { get; set; }
}
public class JumpStateModel : RequestModelBase
{
    public int JumpState { get; set; }
}
public class SkillStateModel : RequestModelBase
{
    public int SkillState { get; set; }
}