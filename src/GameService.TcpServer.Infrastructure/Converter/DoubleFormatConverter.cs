using Newtonsoft.Json;

namespace GameService.TcpServer.Infrastructure.Converter;

public class DoubleFormatConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(double);
    }

    public override void WriteJson(JsonWriter writer, object value, 
        JsonSerializer serializer)
    {
        if (value is double d)
        {
            writer.WriteValue(Math.Round(d, 2));
        }
    }

    public override bool CanRead => false;

    public override object ReadJson(JsonReader reader, Type objectType,
        object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();    
    }
}