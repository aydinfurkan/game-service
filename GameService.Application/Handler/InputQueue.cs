using System;
using System.Collections.Concurrent;
using GameService.Infrastructure.Protocol.RequestModels;

namespace GameService.Handler
{
    public class InputQueue
    {
        private readonly InputHandler _inputHandler;
        private readonly ConcurrentQueue<ClientInput> _queue;

        public InputQueue(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            _queue = new ConcurrentQueue<ClientInput>();
        }

        public void Push(ClientInput clientInput)
        {
            _queue.Enqueue(clientInput);
        }

        public ClientInput Pop()
        {
            _queue.TryDequeue(out var clientInput);
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