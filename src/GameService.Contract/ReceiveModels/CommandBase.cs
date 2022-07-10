
namespace GameService.Contract.ReceiveModels;

public abstract class CommandBaseData{}
public class CommandBase<T> where T : CommandBaseData 
{
    public int Type { get; set; }
    public T Data { get; set; }
}