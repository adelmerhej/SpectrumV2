using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Spectrum.Models.Projects.Serializers
{
    /// <summary>
    /// A tolerant ObjectId-as-string BSON serializer that handles empty/null values
    /// without throwing "not a valid 24 digit hex string".
    ///
    /// Deserialization:
    ///   - ObjectId  → 24-hex string
    ///   - String    → returned as-is (or null if empty)
    ///   - Null/missing → null
    ///
    /// Serialization:
    ///   - null or ""  → BsonNull (field is written as null, not omitted)
    ///   - valid 24-hex string → BsonObjectId
    ///   - any other string → BsonString (tolerant fallback)
    /// </summary>
    public class SafeObjectIdSerializer : SerializerBase<string>
    {
        public static readonly SafeObjectIdSerializer Instance = new SafeObjectIdSerializer();

        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.ObjectId:
                    return context.Reader.ReadObjectId().ToString();

                case BsonType.String:
                    var s = context.Reader.ReadString();
                    return string.IsNullOrWhiteSpace(s) ? null : s;

                case BsonType.Null:
                    context.Reader.ReadNull();
                    return null;

                default:
                    context.Reader.SkipValue();
                    return null;
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                context.Writer.WriteNull();
                return;
            }

            ObjectId objectId;
            if (ObjectId.TryParse(value, out objectId))
                context.Writer.WriteObjectId(objectId);
            else
                context.Writer.WriteString(value); // tolerant fallback for non-ObjectId strings
        }
    }
}
