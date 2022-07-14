
namespace GameService.Contract.Commands;

public class CommandBase<T> where T : CommandBaseData 
{
    public int Type { get; set; }
    public T Data { get; set; }
}

public abstract class CommandBaseData
{
    public string Name => GetType().Name; 
}