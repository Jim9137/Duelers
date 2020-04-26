using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AnimationsConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        // deserialize as object
        var roles = serializer.Deserialize<JObject>(reader);
        var result = new List<Animation>();

        // create an array out of the properties
        foreach (var property in roles.Properties())
        {
            var role = property.Value.ToObject<string[]>();
            result.Add(new Animation {id = property.Name, frames = role});
        }

        return result.ToArray();
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(Animation[]);
}