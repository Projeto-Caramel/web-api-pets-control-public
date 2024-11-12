using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Dashboard;
using Caramel.Pattern.Services.Domain.Entities.Models.Favorites;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Services.Favorites;
using Caramel.Pattern.Services.Domain.Services.Pets;
using Microsoft.AspNetCore.Mvc;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{

    [Route("[controller]")]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    public class FavoritesController : BaseController
    {
        private readonly ILogger<FavoritesController> _logger;
        private readonly IFavoritePetsService _service;
        private readonly IMapper _mapper;

        public FavoritesController(
            ILogger<FavoritesController> logger,
            IFavoritePetsService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera os Pets Favoritos do usuário.
        /// </summary>
        /// <param name="userId">O ID do parceiro.</param>
        /// <returns>Pets Favoritos, Status do Processo e Descrição</returns>
        [HttpGet("/pets-control/favorites")]
        [ProducesResponseType(typeof(CustomResponse<FavoritePets>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardAsync(string userId)
        {
            var favorites = await _service.GetFavoritePetsByUserAsync(userId);

            var response = new CustomResponse<FavoritePets>(favorites, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Adiciona um Pet nos Favoritos do Usuário.
        /// </summary>
        /// <param name="userId">O ID do parceiro.</param>
        /// <returns>Pets Favoritos, Status do Processo e Descrição</returns>
        [HttpPost("/pets-control/favorites/add")]
        [ProducesResponseType(typeof(CustomResponse<DashboardData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPetOnFavoritesAsync(string userId, string petId)
        {
            var favorites = await _service.AddFavoritePetForUser(userId, petId);

            var response = new CustomResponse<FavoritePets>(favorites, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Remove um Pet nos Favoritos do Usuário.
        /// </summary>
        /// <param name="userId">O ID do parceiro.</param>
        /// <returns>Pets Favoritos, Status do Processo e Descrição</returns>
        [HttpPost("/pets-control/favorites/remove")]
        [ProducesResponseType(typeof(CustomResponse<DashboardData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemovePetOnFavoritesAsync(string userId, string petId)
        {
            var favorites = await _service.RemoveFavoritePetForUser(userId, petId);

            var response = new CustomResponse<FavoritePets>(favorites, StatusProcess.Success);

            return Ok(response);
        }
    }
}
