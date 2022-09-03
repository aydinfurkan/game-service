namespace GameService.Contract.ResponseModels;

public class MoveStateModel : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public string MoveState { get; set; }
}
public class JumpStateModel : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public int JumpState { get; set; }
}
public class SkillStateModel : ResponseModelData
{
    public Guid CharacterId { get; set; }
    public Guid? TargetCharacterId { get; set; }
    public int SkillState { get; set; }
}