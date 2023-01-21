namespace GameService.Anticorruption.UserService.Configs;

public class UserServiceSettings
{
    public Url Url { get; set; }
    public Credentials Credentials { get; set; }
    public EndPoints EndPoints { get; set; }
}
public class Url
{
    public string Scheme { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
public class Credentials
{
    public string Username { get; set; }
    public string Password { get; set; }
}
public class EndPoints
{
    public string ReplaceCharacter { get; set; }
}