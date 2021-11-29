using System;
using CoreLib.HttpClients.Interfaces;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;
using GameService.Domain.ValueObject;

namespace GameService.Commands
{
    public class GameCommand
    {
        private readonly Game _game;

        public GameCommand(Game game)
        {
            _game = game;
        }

        public bool SetCharacterActive(Character character)
        {
            return _game.AddCharacter(character);
        }
        
        public bool SetCharacterDeactivated(Character character)
        {
            return _game.DeleteCharacter(character);
        }
        
        public bool ChangeCharacterPosition(Guid id, Position position)
        {
            return _game.ChangeCharacterPosition(id, position);
        }
        public bool ChangeCharacterQuaternion(Guid id, Quaternion quaternion)
        {
            return _game.ChangeCharacterQuaternion(id, quaternion);
        }
    }
}