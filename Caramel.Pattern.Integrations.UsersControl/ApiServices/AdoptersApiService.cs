using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Services.Security;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Caramel.Pattern.Integrations.UsersControl.ApiServices
{
    public class AdoptersApiService : IAdopterApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenService _tokenService;
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _httpClient;

        public AdoptersApiService(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("AuthClient");
            _tokenService = tokenService;
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public async Task<Adopter> GetSingleOrDefaultByIdAsync(string id)
        {
            SetAuthorization();

            var result = await _httpClient.GetAsync($"/users-control/adopter?id={id}");
            var content = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                ValidateResponse(content, result.StatusCode);

            var response = JsonSerializer.Deserialize<CustomResponse<Adopter>>(content, _options);

            BusinessException.ThrowIfNull(response, "CustomResponse");

            return response.Data;
        }

        private void ValidateResponse(string content, HttpStatusCode statusCode)
        {
            var exception = JsonSerializer.Deserialize<ExceptionResponse>(content, _options);
            BusinessException.ThrowIfNull(exception, "ExceptionResponse");

            throw new BusinessException(exception.ErrorDetails, exception.Status, statusCode);
        }

        private void SetAuthorization()
        {
            var token = _tokenService.GenerateIssuerJwtTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
