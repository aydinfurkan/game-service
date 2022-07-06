namespace GameService.Contract.ResponseModels;

public abstract class ResponseModelData
{
}
    
public class ResponseModelBase<T> where T : ResponseModelData
{
    public int Type;
    public T Data;
    public ResponseModelBase(int type, T data)
    {
        Data = data;
        Type = type;
    }
}