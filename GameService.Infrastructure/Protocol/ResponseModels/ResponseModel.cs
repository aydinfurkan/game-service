using System;

namespace GameService.Infrastructure.Protocol.ResponseModels
{
    public abstract class ResponseModelBase
    {
    }
    
    public class ResponseModel<T> where T : ResponseModelBase
    {
        public int Type;
        public T Data;
        public ResponseModel(int type, T data)
        {
            Data = data;
            Type = type;
        }
    }
}