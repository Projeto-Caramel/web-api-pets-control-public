using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Services.Gallery;
using Microsoft.AspNetCore.Mvc;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    public class GalleryController : BaseController
    {
        private readonly ILogger<PetController> _logger;
        private readonly IPetGalleryService _service;
        private readonly IMapper _mapper;

        public GalleryController(
            ILogger<PetController> logger,
            IPetGalleryService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera uma lista de seis posições de Imagens do Pet específico.
        /// </summary>
        /// <param name="petId">O ID do parceiro.</param>
        /// <returns>Lista de Imagens, Status do Processo e Descrição</returns>
        [HttpGet("/pets-control/pets/gallery")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPetsAsync(string petId)
        {
            var pets = await _service.GetGalleryByPetIdAsync(petId);

            var response = new CustomPetListResponse<IEnumerable<PetGalleryImage>>(pets, StatusProcess.Success, pets.Count());

            return Ok(response);
        }


        /// <summary>
        /// Adiciona uma nova imagem na galleria de fotos do Pet.
        /// </summary>
        /// <param name="petImageRequest">O objeto Pet Gallery Image a ser criado.</param>
        /// <returns>Imagem Adicionado, Status do Processo e Descrição.</returns>
        [HttpPost("/pets-control/pets/gallery")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterPetAsync(PetGalleryImageRequest petImageRequest)
        {
            var image = _mapper.Map<PetGalleryImage>(petImageRequest);

            var imagePet = await _service.RegisterAsync(image, petImageRequest.Base64Image);

            var response = new CustomResponse<PetGalleryImage>(imagePet, StatusProcess.Success);

            var uri = Url.Action("GetAllPetsAsync", "GalleryController", new { id = imagePet.Id });

            return Created(uri, response);
        }

        /// <summary>
        /// Deleta uma Imagem existente.
        /// </summary>
        /// <param name="imageId">O ID da imagem do Pet a ser Deletada.</param>
        /// <returns>Uma CustomResponse contendo o objeto Pet atualizado ou uma mensagem de erro.</returns>
        [HttpDelete("/pets-control/pets/gallery/{petId}")]
        [ProducesResponseType(typeof(CustomResponse<PetStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePet(string imageId)
        {
            await _service.DeleteAsync(imageId);

            return NoContent();
        }
    }
}
