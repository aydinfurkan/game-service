using System.Net.Sockets;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Infrastructure.Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameService.TcpServer.Infrastructure.Protocol;

public class GameProtocol : WebSocketProtocol, IProtocol
{
    public async Task WriteAsync<T>(TcpClient tcpClient, T obj) where T : ResponseModelData
    {
        var type = obj switch
        {
            ClientCharacter => 0,
            ActiveCharacters => 1,
            AddCharacter => 2,
            DeleteCharacter => 3,
            PositionModel => 16,
            QuaternionModel => 17,
            MoveStateModel => 32,
            JumpStateModel => 33,
            SkillStateModel => 34,
            CharacterHealth => 48,
            CharacterMana => 49,
            CharacterStats => 50,
            CharacterLevel => 51,
            PingModel => 99,
            _ => 3131
        };
        var responseModel = new ResponseModelBase<T>(type, obj);

        var jsonSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        jsonSetting.Converters.Add(new DoubleFormatConverter());
        jsonSetting.Converters.Add(new DateFormatConverter());
        var str = JsonConvert.SerializeObject(responseModel, jsonSetting);
        await base.WriteAsync(tcpClient, str);
    }
        
    public async Task<CommandBaseData?> ReadAsync(TcpClient tcpClient, CancellationToken cancellationToken)
    {
        var str= await base.ReadAsync(tcpClient, cancellationToken);
        var definition = new { Type = 0 };
        
        try
        {
            var type = JsonConvert.DeserializeAnonymousType(str, definition);
            
            CommandBaseData? requestModelData = type?.Type switch
            {
                128 => JsonConvert.DeserializeObject<CommandBase<VerificationCommand>>(str)?.Data,
                144 => JsonConvert.DeserializeObject<CommandBase<ChangePositionCommand>>(str)?.Data,
                145 => JsonConvert.DeserializeObject<CommandBase<ChangeQuaternionCommand>>(str)?.Data,
                160 => JsonConvert.DeserializeObject<CommandBase<ChangeMoveStateCommand>>(str)?.Data,
                161 => JsonConvert.DeserializeObject<CommandBase<ChangeJumpStateCommand>>(str)?.Data,
                162 => JsonConvert.DeserializeObject<CommandBase<ChangeSkillStateCommand>>(str)?.Data,
                176 => JsonConvert.DeserializeObject<CommandBase<SelectCharacterCommand>>(str)?.Data,
                _ => null
            };
            
            return requestModelData;
        }
        catch
        {
            return null;
        }
    }

    public async Task HandShakeAsync(TcpClient client)
    {
        await base.HandShakeAsync(client);
    }
}