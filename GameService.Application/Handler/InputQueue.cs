using System;
using System.Collections.Concurrent;
using GameService.Infrastructure.Protocol.RequestModels;

namespace GameService.Handler
{
    public class InputQueue
    {
        private readonly InputHandler _inputHandler;
        private readonly BlockingCollection<ClientInput> _queue;

        public InputQueue(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            _queue = new BlockingCollection<ClientInput>();
        }

        public void Push(ClientInput clientInput)
        {
            _queue.Add(clientInput);
        }

        public ClientInput Pop()
        {
            _queue.TryTake(out var clientInput);
            return clientInput;
        }

        public void Handle()
        {
            var clientInput = Pop();
            if (clientInput == null) return;
            
            var ok = clientInput.Input switch
            {
                PositionModel o => _inputHandler.HandlePosition(o, clientInput.Client),
                QuaternionModel o => _inputHandler.HandleQuaternion(o, clientInput.Client),
                MoveStateModel o => _inputHandler.HandleMoveState(o, clientInput.Client),
                JumpStateModel o => _inputHandler.HandleJumpState(o, clientInput.Client),
                SkillStateModel o => _inputHandler.HandleSkillState(o, clientInput.Client),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        
    }
}