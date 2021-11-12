using System;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;
using GameService.Domain.ValueObject;

namespace GameService.Commands
{
    public class GameCommand
    {
        private Game _game;
        private readonly IPlayerAntiCorruption _playerAntiCorruption;

        public GameCommand(Game game, IPlayerAntiCorruption playerAntiCorruption)
        {
            _game = game;
            _playerAntiCorruption = playerAntiCorruption;
        }

        public bool SetPlayerActive(Guid id)
        {
            var player = _playerAntiCorruption.GetPlayer(id);
            return _game.AddPlayer(player);
        }
        
        public bool SetPlayerDeactivated(Guid id)
        {
            var player = _playerAntiCorruption.GetPlayer(id);
            return _game.DeletePlayer(player);
        }
        
        public bool ChangePlayerPosition(Guid id, Position position)
        {
            return _game.ChangePlayerPosition(id, position);
        }
        public bool ChangePlayerQuaternion(Guid id, Quaternion quaternion)
        {
            return _game.ChangePlayerQuaternion(id, quaternion);
        }
    }
}