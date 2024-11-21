using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Pets;
using Caramel.Pattern.Services.Domain.Validators;
using MongoDB.Bson;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Pets
{
    public class PetService : BaseService, IPetService
    {
        private readonly IPartnerApiService _partnerApiService;
        private readonly IBucketService _bucketService;

        public PetService(IUnitOfWork unitOfWork, IPartnerApiService partnerApiService, IBucketService bucketService) : base(unitOfWork)
        {
            _partnerApiService = partnerApiService;
            _bucketService = bucketService;
        }

        public async Task<Pet> GetSingleOrDefaultByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var pet = await _unitOfWork.Pets.GetSingleAsync(id) ?? 
                throw new BusinessException("Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest);
            
            return pet;
        }

        public async Task<PetStatus> GetPetStatusAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var pet = await _unitOfWork.Pets.GetSingleAsync(id);

            if (pet == null || pet.Info == null)
                throw new BusinessException("Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest);

            return pet.Info.Status;
        }

        public async Task<IEnumerable<Pet>> FetchAsync(string partnerId)
        {
            if (string.IsNullOrEmpty(partnerId))
                throw new BusinessException("O campo Partner ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(partnerId)
                ?? throw new BusinessException("Não foi possível encontrar nenhum Partner com esse ID.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
            
            var pets = await _unitOfWork.Pets.FetchAsync(x => x.PartnerId == partnerId);

            return pets;
        }

        public async Task<IEnumerable<Pet>> FetchAllAsync()
        {
            var pets = await _unitOfWork.Pets.FetchAsync(x => x.Info.Status == PetStatus.Available);

            return pets;
        }

        public async Task<IEnumerable<Pet>> FetchByFilterAsync(string partnerId, PetFilter filter)
        {
            BusinessException.ThrowIfNull(filter, "Pet Filter");

            if (string.IsNullOrEmpty(partnerId))
                throw new BusinessException("O campo Partner ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(partnerId);

            var pets = await _unitOfWork.Pets.FetchAsync(x => x.PartnerId == partnerId);

            var petsFiltered = FilterPets(pets, filter);

            return petsFiltered;
        }

        public async Task<IEnumerable<Pet>> FetchByRangeAsync(List<string> ids)
        {
            BusinessException.ThrowIfNull(ids, "Range de Ids");

            var pets = await _unitOfWork.Pets.FetchAsync(x => ids.Contains(x.Id));

            return pets;
        }

        public async Task<Pet> RegisterAsync(Pet entity, string base64Image)
        {
            BusinessException.ThrowIfNull(entity, "Pet");
            ValidateEntity<PetValidator, Pet>(entity);

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(entity.PartnerId) ??
                throw new BusinessException("Não foi possível encontrar nenhum Partner com esse ID.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            entity.Info.RegisterDate = DateTime.Now;
            entity.Id = ObjectId.GenerateNewId().ToString();

            var key = $"pets/{entity.Id}/profileImage.jpg";
            var imageUrl = await _bucketService.UploadFileAsync(base64Image, key);

            entity.Info.ProfileImageUrl = imageUrl;

            await _unitOfWork.Pets.AddAsync(entity);

            return entity;
        }

        public async Task<Pet> UpdateAsync(Pet entity, string base64Image)
        {
            BusinessException.ThrowIfNull(entity, "Pet");

            if (string.IsNullOrEmpty(entity.Id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            ValidateEntity<PetValidator, Pet>(entity);

            var pet = await GetSingleOrDefaultByIdAsync(entity.Id) ?? 
                throw new BusinessException("Não foi possível encontrar nenhum Partner com esse ID.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var key = $"pets/{pet.Id}/profileImage.jpg";
            var imageUrl = await _bucketService.UploadFileAsync(base64Image, key);

            entity.Info.ProfileImageUrl = imageUrl;

            var registerDate = pet.Info.RegisterDate;
            pet.PartnerId = entity.PartnerId;
            pet.Info = entity.Info;
            pet.Healthy = entity.Healthy;
            pet.Caracteristics = entity.Caracteristics;
            pet.Info.RegisterDate = registerDate;

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(pet.PartnerId);

            if (partner == null)
                throw new BusinessException("Não foi possível encontrar nenhum Partner com esse ID.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            _unitOfWork.Pets.Update(pet);

            return pet;
        }

        public async Task<PetStatus> UpdateStatusAsync(string id, PetStatus status)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var entity = await GetSingleOrDefaultByIdAsync(id);

            if (entity.Info == null)
                throw new BusinessException("Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.UnprocessableEntity);

            entity.Info.Status = status;

            if (status == PetStatus.AdoptApp || status == PetStatus.AdoptOng)
                entity.Info.AdoptionDate = DateTime.Now;
            else
                entity.Info.AdoptionDate = default;

            _unitOfWork.Pets.Update(entity);

            return entity.Info.Status;
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var entity = await GetSingleOrDefaultByIdAsync(id);

            var key = $"pets/{id}/profileImage.jpg";
            await _bucketService.DeleteFileAsync(key);

            _unitOfWork.Pets.Delete(entity);
        }

        public async Task<string> GetImageBase64(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var key = $"pets/{id}/profileImage.jpg";

            if (!await _bucketService.ImageExistsAsync(key))
                return string.Empty;

            return await _bucketService.GetImageAsBase64Async(key);
        }

        private IEnumerable<Pet> FilterPets(IEnumerable<Pet> pets, PetFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
                pets = pets.Where(x => x.Info.Name.ToUpper().Contains(filter.Name.ToUpper()));

            if (filter.Castrated != CastratedStatus.Unknown)
                pets = pets.Where(x => x.Healthy.Castrated == filter.Castrated);

            if (filter.Vaccinated != VaccinatedStatus.Unknown)
                pets = pets.Where(x => x.Healthy.Vaccinated == filter.Vaccinated);

            if (filter.Age != AgeType.Unknown)
            {
                pets = pets.Where(x =>
                {
                    return FilterByAgeType(filter.Age, x.Info.BirthDate);
                });
            }

            if (filter.Status != PetStatus.Unknown)
                pets = pets.Where(x => x.Info.Status == filter.Status);

            if (filter.Sex != PetSex.Unknown)
                pets = pets.Where(x => x.Caracteristics.Sex == filter.Sex);

            if (filter.RegisterDate != default)
                pets = pets.Where(x => x.Info.RegisterDate.ToShortDateString() == filter.RegisterDate.ToShortDateString());

            return pets;
        }

        private static bool FilterByAgeType(AgeType ageType, DateTime birthDate)
        {
            DateTime now = DateTime.Now;

            int age = now.Year - birthDate.Year;

            if (birthDate.Date > now.AddYears(-age)) age--;

            return ageType switch
            {
                AgeType.Puppy => age == 0,
                AgeType.Young => age >= 1 && age <= 3,
                AgeType.Adult => age > 3 && age <= 7,
                AgeType.Senior => age > 7,
                _ => false
            };
        }
    }
}
