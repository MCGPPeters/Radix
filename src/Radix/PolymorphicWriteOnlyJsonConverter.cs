﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Radix;

public class PolymorphicWriteOnlyJsonConverter<T> : JsonConverter<T> where T : notnull
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotImplementedException($"Deserializing not supported. Type={typeToConvert}.");

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
