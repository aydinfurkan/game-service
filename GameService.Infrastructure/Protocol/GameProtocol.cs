using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RequestModels = GameService.Infrastructure.Protocol.RequestModels;
using ResponseModels = GameService.Infrastructure.Protocol.ResponseModels;

namespace GameService.Infrastructure.Protocol
{
    public class GameProtocol : WebSocketProtocol, IProtocol
    {
        public bool Write<T>(TcpClient client, T obj) where T : ResponseModels.ResponseModelBase
        {
            var type = obj switch
            {
                ResponseModels.ClientCharacter => 0,
                ResponseModels.ActiveCharacters => 1,
                ResponseModels.AddCharacter => 2,
                ResponseModels.DeleteCharacter => 3,
                ResponseModels.PositionModel => 16,
                ResponseModels.QuaternionModel => 17,
                ResponseModels.MoveStateModel => 32,
                ResponseModels.JumpStateModel => 33,
                ResponseModels.SkillStateModel => 34,
                ResponseModels.CharacterHealth => 48,
                ResponseModels.CharacterMana => 49,
                ResponseModels.CharacterStats => 50,
                ResponseModels.CharacterLevel => 51,
                _ => 3131
            };
            var responseModel = new ResponseModels.ResponseModel<T>(type, obj);

            var jsonSetting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsonSetting.Converters.Add(new DecimalFormatConverter());
            var str = JsonConvert.SerializeObject(responseModel, jsonSetting);
            return base.Write(client, str);
        }
        
        public new object Read(TcpClient client)
        {
            var str= base.Read(client);
            var definition = new { Type = 0 };
            var type = JsonConvert.DeserializeAnonymousType(str, definition);
            object requestModelData = type?.Type switch
            {
                128 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.VerificationModel>>(str)?.Data,
                144 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.PositionModel>>(str)?.Data,
                145 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.QuaternionModel>>(str)?.Data,
                160 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.MoveStateModel>>(str)?.Data,
                161 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.JumpStateModel>>(str)?.Data,
                162 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.SkillStateModel>>(str)?.Data,
                176 => JsonConvert.DeserializeObject<RequestModels.RequestModel<RequestModels.SelectCharacterModel>>(str)?.Data,
                _ => null
            };
            
            return requestModelData;
        }
        
        public new void HandShake(TcpClient client)
        {
            base.HandShake(client);
        }
    }
}