using System.Net.Sockets;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Infrastructure.Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameService.TcpServer.Infrastructure.Protocol;

public class GameProtocol : WebSocketProtocol, IProtocol
{
    public bool Write<T>(TcpClient client, T obj) where T : ResponseModelData
    {
        var type = obj switch
        {
            ClientCharacter => 0,
            ActiveCharacters => 1,
            AddCharacter => 2,
            DeleteCharacter => 3,
            Contract.ResponseModels.PositionModel => 16,
            Contract.ResponseModels.QuaternionModel => 17,
            Contract.ResponseModels.MoveStateModel => 32,
            Contract.ResponseModels.JumpStateModel => 33,
            Contract.ResponseModels.SkillStateModel => 34,
            CharacterHealth => 48,
            CharacterMana => 49,
            CharacterStats => 50,
            CharacterLevel => 51,
            _ => 3131
        };
        var responseModel = new ResponseModelBase<T>(type, obj);

        var jsonSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        jsonSetting.Converters.Add(new DoubleFormatConverter());
        var str = JsonConvert.SerializeObject(responseModel, jsonSetting);
        return base.Write(client, str);
    }
        
    public CommandBaseData? Read(TcpClient client)
    {
        var str= base.Read(client);
        var definition = new { Type = 0 };
        var type = JsonConvert.DeserializeAnonymousType(str, definition);
        CommandBaseData? requestModelData = type?.Type switch
        {
            128 => JsonConvert.DeserializeObject<CommandBase<VerificationCommand>>(str)?.Data,
            144 => JsonConvert.DeserializeObject<CommandBase<ChangePositionCommand>>(str)?.Data,
            145 => JsonConvert.DeserializeObject<CommandBase<ChangeQuaternionCommand>>(str)?.Data,
            160 => JsonConvert.DeserializeObject<CommandBase<ChangeMoveStateCommand>>(str)?.Data,
            161 => JsonConvert.DeserializeObject<CommandBase<ChangeJumpStateCommand>>(str)?.Data,
            162 => JsonConvert.DeserializeObject<CommandBase<CastSkillCommand>>(str)?.Data,
            176 => JsonConvert.DeserializeObject<CommandBase<SelectCharacterCommand>>(str)?.Data,
            _ => null
        };
            
        return requestModelData;
    }
        
    public new void HandShake(TcpClient client)
    {
        base.HandShake(client);
    }
}