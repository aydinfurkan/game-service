using GameService.Application.Controllers;

namespace GameService.Application.Handler;

public class ClientInput
{
    public object Input;
    public GameClient Client;

    public ClientInput(GameClient client, object input)
    {
        Client = client;
        Input = input;
    }
}