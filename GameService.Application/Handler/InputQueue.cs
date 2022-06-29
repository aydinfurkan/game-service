using System;
using System.Collections.Concurrent;
using GameService.Application.Controllers;
using GameService.Infrastructure.Logger;
using GameService.Infrastructure.Protocol.RequestModels;

namespace GameService.Application.Handler;

public class InputQueue
{
    private readonly InputHandler _inputHandler;
    private readonly BlockingCollection<ClientInput> _queue;
    private readonly IPLogger<ServerController> _logger;

    public InputQueue(InputHandler inputHandler, IPLogger<ServerController> logger)
    {
        _inputHandler = inputHandler;
        _logger = logger;
        _queue = new BlockingCollection<ClientInput>();
    }

    public void Push(ClientInput clientInput)
    {
        _queue.Add(clientInput);
    }

    public ClientInput Pop()
    {
        return _queue.Take();
    }

    public void Handle()
    {
        var clientInput = Pop();

        try
        {
            var ok = clientInput.Input switch
            {
                PositionModel o => _inputHandler.HandlePosition(o, clientInput.Client),
                QuaternionModel o => _inputHandler.HandleQuaternion(o, clientInput.Client),
                MoveStateModel o => _inputHandler.HandleMoveState(o, clientInput.Client),
                JumpStateModel o => _inputHandler.HandleJumpState(o, clientInput.Client),
                SelectCharacterModel o => _inputHandler.HandleSelectCharacter(o, clientInput.Client),
                SkillStateModel o => _inputHandler.HandleSkillState(o, clientInput.Client),
                _ => false
            };
        }
        catch(Exception e)
        {
            _logger.LogError(EventId.Handle,$"Error on : {clientInput.Input}", e);
        }
            
            
    }

        
}