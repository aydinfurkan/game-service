using GameService.TcpServer.Controllers;

namespace GameService.TcpServer.Models;

public class ClientInput
{
    public readonly object Input;
    public readonly Client Client;

    public ClientInput(Client client, object input)
    {
        Client = client;
        Input = input;
    }
}