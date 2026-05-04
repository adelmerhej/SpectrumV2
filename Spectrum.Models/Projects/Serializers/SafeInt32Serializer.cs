using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace SpectrumV1.Models.Projects.Serializers
{
    /// <summary>
    /// A tolerant <see cref="int"/> BSON serializer that gracefully handles
    /// documents where the value was stored as a string, double, or Int64
    /// instead of Int32.  Non-numeric or empty strings deserialize as 0.
    /// </summary>
    public class SafeInt32Serializer : SerializerBase<int>
    {
        public override int Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.Int32:
                    return context.Reader.ReadInt32();
                case BsonType.Int64:
                    return (int)context.Reader.ReadInt64();
                case BsonType.Double:
                    return (int)context.Reader.ReadDouble();
                case BsonType.String:
                    var str = context.Reader.ReadString();
                    return int.TryParse(str, out var result) ? result : 0;
                case BsonType.Null:
                    context.Reader.ReadNull();
                    return 0;
                default:
                    throw new BsonSerializationException(
                        $"Cannot deserialize BsonType.{bsonType} to Int32.");
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, int value)
        {
            context.Writer.WriteInt32(value);
        }
    }
}
