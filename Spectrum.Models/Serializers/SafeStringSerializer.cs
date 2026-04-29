using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Spectrum.Models.Serializers
{
    public class SafeStringSerializer : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.String:
                    return context.Reader.ReadString();
                case BsonType.Int32:
                    return context.Reader.ReadInt32().ToString();
                case BsonType.Int64:
                    return context.Reader.ReadInt64().ToString();
                case BsonType.Double:
                    return context.Reader.ReadDouble().ToString(System.Globalization.CultureInfo.InvariantCulture);
                case BsonType.Boolean:
                    return context.Reader.ReadBoolean().ToString();
                case BsonType.Null:
                    context.Reader.ReadNull();
                    return string.Empty;
                default:
                    throw new BsonSerializationException($"Cannot deserialize BsonType.{bsonType} to string.");
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
                return;
            }

            context.Writer.WriteString(value);
        }
    }
}