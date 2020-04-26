using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal class FrameConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(AnimationFrame[]);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // deserialize as object
        var roles = serializer.Deserialize<JObject>(reader);
        var result = new List<AnimationFrame>();

        // create an array out of the properties
        foreach (var property in roles.Properties())
        {
            var role = property.Value.ToObject<AnimationFrame>();
            role.id = property.Name;
            result.Add(role);
        }

        return result.ToArray();
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}