using Amazon.Runtime.Internal.Transform;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class PetFilterData
    {
        public static Dictionary<string, PetFilter> Data = new Dictionary<string, PetFilter>
        {
            {
                "Basic", new PetFilter()
                {
                    Name = "Basic",
                    Castrated = CastratedStatus.Castrated,
                    Vaccinated = VaccinatedStatus.Vaccinated,
                    Age = AgeType.Puppy,
                    Status = PetStatus.Available,
                    Sex = PetSex.Male,
                    RegisterDate = DateTime.Now
                }
            },
            {
                "Empty", new PetFilter()
            },
            {
                "Null", null
            }
        };
    }
}
