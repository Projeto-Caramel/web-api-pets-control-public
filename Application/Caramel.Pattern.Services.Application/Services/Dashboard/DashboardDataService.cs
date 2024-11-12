using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Domain.Entities.Models.Dashboard;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services.Dashboard;
using Caramel.Pattern.Services.Domain.Services.Pets;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Dashboard
{
    public class DashboardDataService : BaseService, IDashboardService
    {
        private readonly IPetService _petService;
        private readonly IPartnerApiService _partnerApiService;

        public DashboardDataService(IUnitOfWork unitOfWork, IPetService petService, IPartnerApiService partnerApiService) : base(unitOfWork)
        {
            _petService = petService;
            _partnerApiService = partnerApiService;
        }

        public async Task<DashboardData> GetSingleOrDefaultByIdAsync(string partnerId)
        {
            if (string.IsNullOrEmpty(partnerId))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(partnerId) ?? 
                throw new BusinessException("Não foi possível encontrar nenhum Partner com esse Id.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
            var pets = await _petService.FetchAsync(partnerId);

            var availablePets = pets.Where(x => x.Info.Status == PetStatus.Available);
            if (availablePets.Count() == 0)
                availablePets = pets;

            var dashboard = new DashboardData();

            if (!pets.Any())
            {
                dashboard.Pets = pets.ToList();
                dashboard.CapacityData = new CapacityData(partner.MaxCapacity, 0, 0);
                dashboard.VaccinatedData = new VaccinatedData(0, 0);
                dashboard.CastratedData = new CastratedData(0, 0);
                dashboard.AdoptedData = new AdoptedData(0, 0);
                dashboard.DadosUltimoAno = LastYearDataReturn(pets);
            }
            else
            {
                dashboard.Pets = PetsReturn(availablePets);
                dashboard.CapacityData = CapacityReturn(partner, availablePets);
                dashboard.VaccinatedData = VaccinatedReturn(availablePets);
                dashboard.CastratedData = CastratedReturn(availablePets);
                dashboard.AdoptedData = AdoptedReturn(pets);
                dashboard.DadosUltimoAno = LastYearDataReturn(pets);
            }

            return dashboard;
        }

        private List<Pet> PetsReturn(IEnumerable<Pet> availablePets)
        {
            return availablePets.Where(x => 
                x.Info.Status == PetStatus.Available && 
                (x.Healthy.Vaccinated == VaccinatedStatus.NotVaccinated ||
                x.Healthy.Castrated == CastratedStatus.NotCastrated))
                .Take(10)
                .ToList();
        }

        private CapacityData CapacityReturn(Partner partner, IEnumerable<Pet> pets)
        {
            int maxCapacity = partner.MaxCapacity;
            int petsAvailableAmount = pets.Count(x => x.Info.Status == PetStatus.Available);
            double capacityPercentage = 100 * ((double)petsAvailableAmount / maxCapacity);

            var dashboard = new DashboardData();
            dashboard.CapacityData = new CapacityData(maxCapacity, petsAvailableAmount, capacityPercentage);

            return dashboard.CapacityData;
        }

        private VaccinatedData VaccinatedReturn(IEnumerable<Pet> pets)
        {
            int vaccinatedAmount = pets.Count(x => x.Healthy.Vaccinated == VaccinatedStatus.Vaccinated);
            double vaccinatedPercentage = 100 * ((double)vaccinatedAmount) / pets.Count();

            var dashboard = new DashboardData();
            dashboard.VaccinatedData = new VaccinatedData(vaccinatedAmount, vaccinatedPercentage);

            return dashboard.VaccinatedData;
        }

        private CastratedData CastratedReturn(IEnumerable<Pet> pets)
        {
            int castratedAmount = pets.Count(x => x.Healthy.Castrated == CastratedStatus.Castrated);
            double castratedPercentage = 100 * ((double)castratedAmount) / pets.Count();

            var dashboard = new DashboardData();
            dashboard.CastratedData = new CastratedData(castratedAmount, castratedPercentage);

            return dashboard.CastratedData;
        }

        private AdoptedData AdoptedReturn(IEnumerable<Pet> pets)
        {
            int adoptedAmount = pets.Count(x => x.Info.AdoptionDate.Date.Month == DateTime.Now.Month
                                           && x.Info.AdoptionDate.Date.Year == DateTime.Now.Year
                                           && (x.Info.Status == PetStatus.AdoptApp || x.Info.Status == PetStatus.AdoptOng));

            double adoptedPercentage = 100 * ((double)adoptedAmount) / pets.Count();

            var dashboard = new DashboardData();
            dashboard.AdoptedData = new AdoptedData(adoptedAmount, adoptedAmount);

            return dashboard.AdoptedData;
        }

        private List<DadosDoMes> LastYearDataReturn(IEnumerable<Pet> pets)
        {
            List<DadosDoMes> year = new List<DadosDoMes>();
            string[] months = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"};

            for (int i = 0; i < 12; i++)
            {
                int adoptedByApp = 0, adoptedByOng = 0;

                DateTime targetDate = DateTime.Now.AddMonths(-i);
                int targetMonth = targetDate.Month;
                int targetYear = targetDate.Year;

                foreach (Pet pet in pets)
                {
                    if (pet.Info.AdoptionDate.Month == targetMonth
                        && pet.Info.AdoptionDate.Year == targetYear)
                    {
                        if (pet.Info.Status == PetStatus.AdoptApp)
                            adoptedByApp++;
                        else if (pet.Info.Status == PetStatus.AdoptOng)
                            adoptedByOng++;
                    }
                }

                DadosDoMes monthData = new DadosDoMes(months[targetMonth - 1], adoptedByApp, adoptedByOng);
                year.Add(monthData);
            }

            return year;
        }
    }

}

