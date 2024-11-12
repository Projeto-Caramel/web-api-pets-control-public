using Caramel.Pattern.Services.Domain.Entities.Models.Dashboard;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class DashData
    {
        public static Dictionary<string, DashboardData> Data = new Dictionary<string, DashboardData>
        {
            {
                "Basic", new DashboardData()
                {
                    Pets = new List<Pet> {PetData.Data["NotVaccinated"]},
                    AdoptedData = new AdoptedData(0, 0),
                    CapacityData = new CapacityData(100, 1, 1),
                    CastratedData = new CastratedData(1, 100),
                    VaccinatedData = new VaccinatedData(0, 0),
                    DadosUltimoAno = new List<DadosDoMes>()
                    {
                        new DadosDoMes("Janeiro", 0, 0),
                        new DadosDoMes("Fevereiro", 0, 0),
                        new DadosDoMes("Março", 0, 0),
                        new DadosDoMes("Abril", 0, 0),
                        new DadosDoMes("Maio", 0, 0),
                        new DadosDoMes("Junho", 0, 0),
                        new DadosDoMes("Julho", 0, 0),
                        new DadosDoMes("Agosto", 0, 0),
                        new DadosDoMes("Setembro", 0, 0),
                        new DadosDoMes("Outubro", 0, 0),
                        new DadosDoMes("Novembro", 0, 0),
                        new DadosDoMes("Dezembro", 0, 0),
                    }                    
                }
            },
            { "Empty", new DashboardData() { }
},
            { "Null", null },
        };
    }
}
