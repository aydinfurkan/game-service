using Newtonsoft.Json.Converters;

namespace GameService.TcpServer.Infrastructure.Converter;

public class DateFormatConverter : IsoDateTimeConverter
{
    public DateFormatConverter()
    {
        DateTimeFormat = "HH:mm:ss.fff";
    }
}