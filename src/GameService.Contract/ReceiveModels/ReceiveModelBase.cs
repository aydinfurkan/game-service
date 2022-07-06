
namespace GameService.Contract.ReceiveModels;

public abstract class ReceiveModelData{}
public class ReceiveModelBase<T> where T : ReceiveModelData 
{
    public int Type { get; set; }
    public T Data { get; set; }
}