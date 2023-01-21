namespace GameService.Contract.Commands;

public class PingCommand : CommandBaseData
{
    public DateTime SentTime { get; set; } 
}