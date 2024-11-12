using Caramel.Pattern.Services.Domain.Enums.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Pets
{
    [ExcludeFromCodeCoverage]
    public class PetInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BirthDate { get; set; }
        public PetStatus Status { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime AdoptionDate { get; set; }
    }
}
