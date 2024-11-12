using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Pets
{
    [ExcludeFromCodeCoverage]
    public class Pet : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public PetInfo Info { get; set; }
        public PetHealthy Healthy { get; set; }
        public PetCharacteristics Caracteristics { get; set; }
    }
}
