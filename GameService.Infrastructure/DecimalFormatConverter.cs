using System;
using Newtonsoft.Json;

namespace GameService.Infrastructure
{
    public class DecimalFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal));
        }

        public override void WriteJson(JsonWriter writer, object value, 
            JsonSerializer serializer)
        {
            writer.WriteValue($"{value:N2}");
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();    
        }
    }   
}