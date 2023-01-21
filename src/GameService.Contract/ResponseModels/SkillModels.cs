namespace GameService.Contract.ResponseModels;

public class ExecuteProjectile
{
    public Guid CasterCharacterId { get; set; }
    
    public Guid TargetCharacterId { get; set; }
    
    public int SkillId { get; set; }
}