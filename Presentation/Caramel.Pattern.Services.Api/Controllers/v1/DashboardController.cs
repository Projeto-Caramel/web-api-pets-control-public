using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Dashboard;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Services.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _service;
        private readonly IMapper _mapper;

        public DashboardController(
            ILogger<DashboardController> logger,
            IDashboardService service,
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
        /// <returns>Lista de Pets, Status do Processo e Descrição</returns>
        [HttpGet("/pets-control/dashboard")]
        [ProducesResponseType(typeof(CustomResponse<DashboardData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardAsync(string partnerId)
        {
            var dashboard = await _service.GetSingleOrDefaultByIdAsync(partnerId);

            var response = new CustomResponse<DashboardData>(dashboard, StatusProcess.Success);

            return Ok(response);
        }
    }
}
