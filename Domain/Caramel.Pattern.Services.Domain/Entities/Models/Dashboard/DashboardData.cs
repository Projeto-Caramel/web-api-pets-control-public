using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Dashboard
{
    [ExcludeFromCodeCoverage]
    public class DashboardData
    {
        public List<Pet> Pets { get; set; }
        public CapacityData CapacityData { get; set; }
        public VaccinatedData VaccinatedData { get; set; }
        public CastratedData CastratedData { get; set; }
        public AdoptedData AdoptedData { get; set; }
        public List<DadosDoMes> DadosUltimoAno { get; set; }
    }
    public class CapacityData(int maxCapacity, int petsAmount, double usedCapacityPercentage)
    {
        public int MaxCapacity { get; private set; } = maxCapacity;
        public int PetsAmount { get; private set; } = petsAmount;
        public double UsedCapacityPercentage { get; private set; } = usedCapacityPercentage;
        public double UnusedCapacityPercentage { get; private set; } = 100 - usedCapacityPercentage;
    }
    public class VaccinatedData(int vaccinatedAmount, double vaccinatedPercentage)
    {
        public int VaccinatedAmount { get; private set; } = vaccinatedAmount;
        public double VaccinatedPercentage { get; private set; } = vaccinatedPercentage;
        public double NotVaccinatedPercentage { get; private set; } = 100 - vaccinatedPercentage;
    }
    public class CastratedData(int castratedAmount, double castratedPercentage)
    {
        public int CastratedAmount { get; private set; } = castratedAmount;
        public double CastratedPercentage { get; private set; } = castratedPercentage;
        public double NotCastratedPercentage { get; private set; } = 100 - castratedPercentage;
    }
    public class AdoptedData(int adoptedAmount, double adoptedPercentage)
    {
        public int AdoptedAmount { get; private set; } = adoptedAmount;
        public double AdoptedPercentage { get; private set; } = adoptedPercentage;
        public double NotAdoptedPercentage { get; private set; } = 100 - adoptedPercentage;
    }

    public class DadosDoMes
    {
        public DadosDoMes(string mes, int adotadosOng, int adotadosApp)
        {
            Mes = mes;
            AdotadosOng = adotadosOng;
            AdotadosApp = adotadosApp;
        }

        public string Mes { get; set; }
        public int AdotadosOng { get; set; }
        public int AdotadosApp { get; set; }        
    }
}
