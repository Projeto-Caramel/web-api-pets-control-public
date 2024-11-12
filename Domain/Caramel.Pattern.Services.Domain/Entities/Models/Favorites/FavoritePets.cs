using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Favorites
{
    [ExcludeFromCodeCoverage]
    public class FavoritePets : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public List<Pet> Pets { get; set; } = new();
    }
}
