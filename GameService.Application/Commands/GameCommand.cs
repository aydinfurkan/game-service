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
            var player = _game.FindPlayerOrDefault(id);
            if (player == null) return false;
            player.Position = position;
            return true;
        }
    }
}