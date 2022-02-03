using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.AntiCorruption.User;
using GameService.Commands;
using GameService.Controllers;
using GameService.Infrastructure.Protocol.CommonModels;
using GameService.Queries;
using RequestModels = GameService.Infrastructure.Protocol.RequestModels;
using ResponseModels = GameService.Infrastructure.Protocol.ResponseModels;

namespace GameService.Handler
{
    public class InputHandler
    {
        private readonly GameServer _gameServer;
        private readonly GameCommand _gameCommand;
        private readonly GameQuery _gameQuery;
        private readonly IUserAntiCorruption _userAntiCorruption;

        public InputHandler(GameServer gameServer, GameCommand gameCommand, GameQuery gameQuery, IUserAntiCorruption userAntiCorruption)
        {
            _gameServer = gameServer;
            _gameCommand = gameCommand;
            _gameQuery = gameQuery;
            _userAntiCorruption = userAntiCorruption;
        }
        public async Task<GameClient> HandleConnectionAsync(RequestModels.VerificationModel requestModel, TcpClient tcpClient)
        {
            var user = (await _userAntiCorruption.VerifyUserAsync(requestModel.PToken)).ToModel();
            _gameServer.CancelFormerConnection(user);
            var character = user.CharacterList.FirstOrDefault(x => x.Id == requestModel.CharacterId);
            var gameClient = new GameClient(tcpClient, requestModel.PToken, user, character);
            _gameServer.AddClient(gameClient);

            var userCharacter = new Character(character);
            var userCharacterResponseModel = new ResponseModels.UserCharacter(userCharacter);
            _gameServer.PushGameQueues(userCharacterResponseModel, x => x.Character.Id == gameClient.Character.Id);
            
            var activeCharacters = _gameQuery.GetAllActiveCharacters().Select(x => new Character(x)).ToList(); 
            var activeCharactersResponseModel = new ResponseModels.ActiveCharacters(activeCharacters);
            _gameServer.PushGameQueues(activeCharactersResponseModel, x => x.Character.Id == gameClient.Character.Id);

            var addCharacterResponseModel = new ResponseModels.AddCharacter(userCharacter);
            _gameServer.PushGameQueues(addCharacterResponseModel, x => x.Character.Id != gameClient.Character.Id);
            
            _gameCommand.SetCharacterActive(gameClient.Character);
            
            return gameClient;
        }

        public bool HandleDisconnect(GameClient gameClient)
        {
            var deleteCharacterResponseModel = new ResponseModels.DeleteCharacter(gameClient.Character.Id);
            _gameServer.PushGameQueues(deleteCharacterResponseModel, x => x.Character.Id != gameClient.Character.Id);
            return true;
        }
        public bool HandlePosition(RequestModels.PositionModel requestModel, GameClient gameClient)
        {
            gameClient.Character.Position = requestModel.Position;
            var responseModel = new ResponseModels.PositionModel(gameClient.Character.Id, requestModel.Position);
            _gameServer.PushGameQueues(responseModel, x => x.Character.Id != gameClient.Character.Id);
            return true;
        }
        public bool HandleQuaternion(RequestModels.QuaternionModel requestModel, GameClient gameClient)
        {
            gameClient.Character.Quaternion = requestModel.Quaternion;
            var responseModel = new ResponseModels.QuaternionModel(gameClient.Character.Id, requestModel.Quaternion);
            _gameServer.PushGameQueues(responseModel, x => x.Character.Id != gameClient.Character.Id);
            return true;
        }
        public bool HandleMoveState(RequestModels.MoveStateModel requestModel, GameClient gameClient)
        {
            var responseModel = new ResponseModels.MoveStateModel(gameClient.Character.Id, requestModel.MoveState);
            _gameServer.PushGameQueues(responseModel, x => x.Character.Id != gameClient.Character.Id);
            return true;
        }
        public bool HandleJumpState(RequestModels.JumpStateModel requestModel, GameClient gameClient)
        {
            var responseModel = new ResponseModels.JumpStateModel(gameClient.Character.Id, requestModel.JumpState);
            _gameServer.PushGameQueues(responseModel, x => x.Character.Id != gameClient.Character.Id);
            return true;
        }
        public bool HandleSkillState(RequestModels.SkillStateModel requestModel, GameClient gameClient)
        {
            var responseModel = new ResponseModels.SkillStateModel(gameClient.Character.Id, requestModel.SkillState);
            _gameServer.PushGameQueues(responseModel, x => x.Character.Id != gameClient.Character.Id);
            return true;
        }
    }
}