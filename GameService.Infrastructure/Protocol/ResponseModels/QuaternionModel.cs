using GameService.Domain.ValueObjects;

namespace GameService.Infrastructure.Protocol.ResponseModels
{
    public class QuaternionModel : ResponseModelBase
    {
        public Quaternion Quaternion { get; set; }
        
        public QuaternionModel(Quaternion quaternion)
        {
            Quaternion = quaternion;
        }
        
    }
}