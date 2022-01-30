
namespace GameService.Infrastructure.Protocol.RequestModels
{
    public abstract class RequestModelBase{}
    public class RequestModel<T> where T : RequestModelBase 
    {
        public int Type { get; set; }
        public T Data { get; set; }
    }
}