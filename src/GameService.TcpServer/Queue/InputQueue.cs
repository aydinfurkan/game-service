// using System.Collections.Concurrent;
// using GameService.Application.Handlers;
// using GameService.Infrastructure.Logger;
// using GameService.Infrastructure.Protocol.ReceiveModels;
// using GameService.Infrastructure.Protocol.RequestModels;
// using GameService.TcpServer.Abstractions;
// using GameService.TcpServer.Models;
//
// namespace GameService.TcpServer.Queue;
//
// public class InputQueue
// {
//     private readonly BlockingCollection<ClientInput> _queue;
//     private readonly IPLogger<InputQueue> _logger;
//     private readonly ICharacterController _characterController;
//
//     public InputQueue(IPLogger<InputQueue> logger, ICharacterController characterController)
//     {
//         _logger = logger;
//         _characterController = characterController;
//         _queue = new BlockingCollection<ClientInput>();
//     }
//
//     public void Push(ClientInput clientInput)
//     {
//         _queue.Add(clientInput);
//     }
//
//     public ClientInput Pop()
//     {
//         return _queue.Take();
//     }
//
//     // public void Handle()
//     // {
//     //     var clientInput = Pop();
//     //
//     //     try
//     //     {
//     //         var ok = clientInput.Input switch
//     //         {
//     //             ChangePositionCommand o => _inputHandler.HandlePosition(o, clientInput.Client),
//     //             ChangeQuaternionCommand o => _inputHandler.HandleQuaternion(o, clientInput.Client),
//     //             ChangeMoveStateCommand o => _inputHandler.HandleMoveState(o, clientInput.Client),
//     //             ChangeJumpStateCommand o => _inputHandler.HandleJumpState(o, clientInput.Client),
//     //             SelectCharacterCommand o => _inputHandler.HandleSelectCharacter(o, clientInput.Client),
//     //             CastSkillCommand o => _inputHandler.HandleSkillState(o, clientInput.Client),
//     //             _ => false
//     //         };
//     //     }
//     //     catch(Exception e)
//     //     {
//     //         _logger.LogError(EventId.Handle,$"Error on : {clientInput.Input}", e);
//     //     }
//     // }
//
//     public void Handle()
//     {
//         var clientInput = Pop();
//
//         try
//         {
//             if (clientInput.Input is CommandBaseData receiveModelData)
//             {
//                 _characterController.Send(clientInput.Client, receiveModelData);
//             }
//         }
//         catch(Exception e)
//         {
//             _logger.LogError(EventId.Handle,$"Error on : {clientInput.Input}", e);
//         }
//     }
//
//         
// }