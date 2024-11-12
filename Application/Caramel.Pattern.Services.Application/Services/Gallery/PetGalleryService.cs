using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Gallery;
using Caramel.Pattern.Services.Domain.Validators;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Pets
{
    public class PetGalleyService(IUnitOfWork unitOfWork, IBucketService bucketService) : BaseService(unitOfWork), IPetGalleryService
    {
        private readonly IBucketService _bucketService = bucketService;

        public async Task<IEnumerable<PetGalleryImage>> GetGalleryByPetIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var images = await _unitOfWork.PetImages.FetchAsync(x => x.PetId == id);

            return EnsureSixPositions(images);
        }

        public async Task<PetGalleryImage> RegisterAsync(PetGalleryImage entity, string base64Image)
        {
            BusinessException.ThrowIfNull(entity, "Pet");
            ValidateEntity<PetGalleryImageValidator, PetGalleryImage>(entity);

            if (string.IsNullOrEmpty(base64Image))
                throw new BusinessException("A Imagem é obrigatória.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            if (entity == null)
                throw new BusinessException("Não foi possível encontrar nenhum Pet com esse ID.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var gallery = await GetGalleryByPetIdAsync(entity.PetId);
            var count = gallery.Where(x => x is not null).Count();

            string key = $"pets/{entity.PetId}/gallery/gallery-{count + 1}.jpg";
            var url = await _bucketService.UploadFileAsync(base64Image, key);

            entity.ImageUrl = url;

            await _unitOfWork.PetImages.AddAsync(entity);

            gallery = await GetGalleryByPetIdAsync(entity.PetId);

            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var entity = await _unitOfWork.PetImages.GetSingleAsync(id);
            var gallery = await GetGalleryByPetIdAsync(entity.PetId);

            var index = gallery.ToList().FindIndex(x => x.Id == id);

            string key = $"pets/{entity.PetId}/gallery/gallery-{index + 1}.jpg";
            await _bucketService.DeleteFileAsync(key);

            _unitOfWork.PetImages.Delete(entity);
        }

        private static IEnumerable<PetGalleryImage> EnsureSixPositions(IEnumerable<PetGalleryImage> source)
        {
            var list = source.ToList();

            if (list.Count > 6)
                list = list.Skip(list.Count - 6).ToList();

            while (list.Count < 6)
                list.Add(null);

            return list;
        }
    }
}
