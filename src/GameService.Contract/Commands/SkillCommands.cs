namespace GameService.Contract.Commands;

public class CastSkillCommand : CommandBaseData
{
    public int SkillId { get; set; }
}

public class ExecuteSkillEffectCommand : CommandBaseData
{
    public int SkillId { get; set; }
}