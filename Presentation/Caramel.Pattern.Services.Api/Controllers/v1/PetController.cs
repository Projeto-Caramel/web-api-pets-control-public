using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Services.Pets;
using Microsoft.AspNetCore.Mvc;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    public class PetController : BaseController
    {
        private readonly ILogger<PetController> _logger;
        private readonly IPetService _service;
        private readonly IMapper _mapper;

        public PetController(
            ILogger<PetController> logger,
            IPetService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera uma lista de pets associados a um parceiro específico.
        /// </summary>
        /// <param name="partnerId">O ID do parceiro.</param>
        /// <param name="page">Página de dados a serem trazidos. Default: Page = 1.</param>
        /// <param name="size">Tamanho da página de dados a serem trazidos. Default: Size = 10.</param>
        /// <returns>Lista de Pets, Status do Processo e Descrição</returns>
        [HttpGet("/pets-control/pets")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPetsAsync(string partnerId, int page, int size)
        {
            Pagination pagination = new(page, size);

            var pets = await _service.FetchAsync(partnerId);

            var paginetedPets = ReturnPaginated<Pet>(pets, pagination);

            var response = new CustomPetListResponse<IEnumerable<Pet>>(paginetedPets, StatusProcess.Success, pets.Count());

            return Ok(response);
        }

        /// <summary>
        /// Recupera uma lista de pets associados a um parceiro específico.
        /// </summary>
        /// <param name="page">Página de dados a serem trazidos. Default: Page = 1.</param>
        /// <param name="size">Tamanho da página de dados a serem trazidos. Default: Size = 10.</param>
        /// <returns>Lista de Pets, Status do Processo e Descrição</returns>
        [HttpGet("/pets-control/pets/all")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPetsAsync(int page, int size)
        {
            Pagination pagination = new(page, size);

            var pets = await _service.FetchAllAsync();

            var paginetedPets = ReturnPaginated<Pet>(pets, pagination);

            var response = new CustomPetListResponse<IEnumerable<Pet>>(paginetedPets, StatusProcess.Success, pets.Count());

            return Ok(response);
        }

        /// <summary>
        /// Recupera uma lista de pets filtrada por critérios específicos para um parceiro.
        /// </summary>
        /// <param name="partnerId">O ID do parceiro.</param>
        /// <param name="request">Página e Total de dados a serem trazidos e Filtro a ser realizado. Default: Page = 1 e Size = 10</param>
        /// <returns>Lista de Pets Filtrados, Status do Processo e Descrição.</returns>
        [HttpPost("/pets-control/pets/filtered")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPetsFiltered(string partnerId, GetPetsFilteredRequest request)
        {
            var pets = await _service.FetchByFilterAsync(partnerId, request.PetFilter);

            var paginetedPets = ReturnPaginated<Pet>(pets, request.Pagination);

            var response = new CustomPetListResponse<IEnumerable<Pet>>(paginetedPets, StatusProcess.Success, pets.Count());

            return Ok(response);
        }

        /// <summary>
        /// Recupera uma lista de pets a partir de uma lista de IDs.
        /// </summary>
        /// <param name="ids">Range de IDs de pets.</param>
        /// /// <param name="page">Página de dados a serem trazidos. Default: Page = 1.</param>
        /// <param name="size">Tamanho da página de dados a serem trazidos. Default: Size = 10.</param>     
        /// <returns>Lista de Pets, Status do Processo e Descrição.</returns>
        [HttpPost("/pets-control/pets/range")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPetsRangeAsync(List<string> ids, int page, int size)
        {
            var pets = await _service.FetchByRangeAsync(ids);

            Pagination pagination = new(page, size);

            var paginetedPets = ReturnPaginated(pets, pagination);

            var response = new CustomPetListResponse<IEnumerable<Pet>>(paginetedPets, StatusProcess.Success, pets.Count());

            return Ok(response);
        }

        /// <summary>
        /// Recupera um pet específico por ID.
        /// </summary>
        /// <param name="petId">O ID do pet a ser recuperado.</param>
        /// <returns>Pet, Status do Processo e Descrição.</returns>
        [HttpGet("/pets-control/pets/{petId}")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPetAsync(string petId)
        {
            var pet = await _service.GetSingleOrDefaultByIdAsync(petId);

            var response = new CustomResponse<Pet>(pet, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Recupera o status de um pet específico por ID.
        /// </summary>
        /// <param name="petId">O ID do pet a ser consultado.</param>
        /// <returns>Status do Pet, Status do Processo e Descrição.</returns>
        [HttpGet("/pets-control/pets/{petId}/status")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPetStatus(string petId)
        {
            var petStatus = await _service.GetPetStatusAsync(petId);

            var response = new CustomResponse<PetStatus>(petStatus, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo pet.
        /// </summary>
        /// <param name="petRequest">O objeto Pet a ser criado.</param>
        /// <returns>Pet Adicionado, Status do Processo e Descrição.</returns>
        [HttpPost("/pets-control/pets")]
        [ProducesResponseType(typeof(CustomResponse<Pet>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterPetAsync(PetRequest petRequest)
        {
            var pet = _mapper.Map<Pet>(petRequest);

            var addedPet = await _service.RegisterAsync(pet, petRequest.Info.Base64Image);

            var response = new CustomResponse<Pet>(addedPet, StatusProcess.Success);

            var uri = Url.Action("GetPet", "PetsController", new { id = addedPet.Id });

            return Created(uri, response);
        }

        /// <summary>
        /// Atualiza um pet existente.
        /// </summary>
        /// <param name="petId">O ID do pet a ser Alterado.</param>
        /// <param name="petRequest">O objeto Pet com dados atualizados.</param>
        /// <returns>Pet Atualizado, Status do Processo e Descrição.</returns>
        [HttpPut("/pets-control/pets")]
        [ProducesResponseType(typeof(CustomResponse<PetStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePetAsync(string petId, PetEditRequest petRequest)
        {
            var petInfo = _mapper.Map<PetInfo>(petRequest.Info);
            var pet = _mapper.Map<Pet>(petRequest);
            pet.Info = petInfo;
            pet.Id = petId;

            var updatedPet = await _service.UpdateAsync(pet, petRequest.Info.Base64Image);

            var response = new CustomResponse<Pet>(updatedPet, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Atualiza o status de um pet existente.
        /// </summary>
        /// <param name="petId">O ID do pet a ser Alterado.</param>
        /// <param name="status">O novo Status.</param>
        /// <returns>Pet Atualizado, Status do Processo e Descrição.</returns>
        [HttpPatch("/pets-control/pets/{petId}/status")]
        [ProducesResponseType(typeof(CustomResponse<PetStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePetStatusAsync(string petId, PetStatus status)
        {
            var updatedStatus = await _service.UpdateStatusAsync(petId, status);

            var response = new CustomResponse<PetStatus>(updatedStatus, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Deleta um pet existente.
        /// </summary>
        /// <param name="petId">O ID do pet a ser Deletado.</param>
        /// <returns>Uma CustomResponse contendo o objeto Pet atualizado ou uma mensagem de erro.</returns>
        [HttpDelete("/pets-control/pets/{petId}")]
        [ProducesResponseType(typeof(CustomResponse<PetStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePet(string petId)
        {
            await _service.DeleteAsync(petId);

            return NoContent();
        }
    }
}
