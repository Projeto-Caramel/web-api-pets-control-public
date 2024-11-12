using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class PetData
    {
        public static Dictionary<string, Pet> Data = new Dictionary<string, Pet>
    {
        {
            "Basic", new Pet()
            {
                Id = "t35t3",
                PartnerId = "t35t3",
                Info = new PetInfo
                {
                    Name = "Basic",
                    Description = "A basic pet",
                    BirthDate = DateTime.Now,
                    Status = PetStatus.Available,
                    ProfileImageUrl = "Teste",
                    RegisterDate = DateTime.Now,
                    AdoptionDate = default,
                },
                Healthy = new PetHealthy
                {
                    Castrated = CastratedStatus.Castrated,
                    Vaccinated = VaccinatedStatus.Vaccinated
                },
                Caracteristics = new PetCharacteristics
                {
                    Sex = PetSex.Male,
                    Coat = CoatType.Short,
                    EnergyLevel = EnergyLevelType.Medium,
                    Size = SizeType.Medium,
                    StimulusLevel = StimulusLevelType.High,
                    Temperament = TemperamentType.NotFriendly,
                    ChildLove = ChildLoveType.Loves,
                    AnimalsSocialization = AnimalsSocializationType.Friendly,
                    SpecialNeeds = SpecialNeedsType.Dietary,
                    Shedding = SheddingType.ShedsSeasonally
                }
            }
        },
        {
            "BasicUpdate", new Pet()
            {
                Id = "t35t5",
                PartnerId = "t35t3",
                Info = new PetInfo
                {
                    Name = "Basic Update",
                    Description = "A basic pet updated",
                    BirthDate = DateTime.Now,
                    Status = PetStatus.Available,
                    ProfileImageUrl = "Teste"
                },
                Healthy = new PetHealthy
                {
                    Castrated = CastratedStatus.Castrated,
                    Vaccinated = VaccinatedStatus.Vaccinated
                },
                Caracteristics = new PetCharacteristics
                {
                     Sex = PetSex.Female,
                    Coat = CoatType.Long,
                    EnergyLevel = EnergyLevelType.High,
                    Size = SizeType.Small,
                    StimulusLevel = StimulusLevelType.High,
                    Temperament = TemperamentType.VeryFriendly,
                    ChildLove = ChildLoveType.Loves,
                    AnimalsSocialization = AnimalsSocializationType.Friendly,
                    SpecialNeeds = SpecialNeedsType.Dietary,
                    Shedding = SheddingType.ShedsLittle
                }
            }
        },
        {
            "NotVaccinated", new Pet()
            {
                Id = "t35t3",
                PartnerId = "t35t3",
                Info = new PetInfo
                {
                    Name = "Basic",
                    Description = "A basic pet",
                    BirthDate = DateTime.Now,
                    Status = PetStatus.Available,
                    ProfileImageUrl = "Teste",
                    RegisterDate = DateTime.Now,
                    AdoptionDate = default,
                },
                Healthy = new PetHealthy
                {
                    Castrated = CastratedStatus.Castrated,
                    Vaccinated = VaccinatedStatus.NotVaccinated
                },
                Caracteristics = new PetCharacteristics
                {
                    Sex = PetSex.Male,
                    Coat = CoatType.Short,
                    EnergyLevel = EnergyLevelType.Medium,
                    Size = SizeType.Medium,
                    StimulusLevel = StimulusLevelType.High,
                    Temperament = TemperamentType.NotFriendly,
                    ChildLove = ChildLoveType.Loves,
                    AnimalsSocialization = AnimalsSocializationType.Friendly,
                    SpecialNeeds = SpecialNeedsType.Dietary,
                    Shedding = SheddingType.ShedsSeasonally
                }
            }
        },
        {
            "UpdateException", new Pet()
            {
                Id = "t35t3-3rr0r",
                Info = new PetInfo
                {
                    Name = "Update Exception",
                    Description = "A pet that causes an update exception",
                    BirthDate = DateTime.Now,
                    Status = PetStatus.Available,
                    ProfileImageUrl = ""
                },
                Healthy = new PetHealthy
                {
                    Castrated = CastratedStatus.Castrated,
                    Vaccinated = VaccinatedStatus.Vaccinated
                },
                Caracteristics = new PetCharacteristics
                {
                     Sex = PetSex.Female,
                    Coat = CoatType.Long,
                    EnergyLevel = EnergyLevelType.High,
                    Size = SizeType.Small,
                    StimulusLevel = StimulusLevelType.High,
                    Temperament = TemperamentType.VeryFriendly,
                    ChildLove = ChildLoveType.Loves,
                    AnimalsSocialization = AnimalsSocializationType.Friendly,
                    SpecialNeeds = SpecialNeedsType.Dietary,
                    Shedding = SheddingType.ShedsLittle
                }
            }
        },
        {
            "Empty", new Pet()
            {
                Id = "t35t3"
            }
        },
        {
            "WithoutId", new Pet()
        },
        {
            "Null", null
        },
    };
    }
}
