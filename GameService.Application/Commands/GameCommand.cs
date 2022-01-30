using System;
using GameService.Domain.Entities;
using GameService.Domain.ValueObjects;

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
        public bool ChangeMoveState(Guid id, string moveState)
        {
            return _game.ChangeMoveState(id, moveState);
        }
        public bool ChangeJumpState(Guid id, int jumpState)
        {
            return _game.ChangeJumpState(id, jumpState);
        }
    }
}