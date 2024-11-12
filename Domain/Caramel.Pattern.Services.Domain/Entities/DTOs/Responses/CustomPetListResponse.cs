using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Responses
{
    [ExcludeFromCodeCoverage]
    public class CustomPetListResponse<T>
    {
        public CustomPetListResponse(T data, StatusProcess status, int total)
        {
            this.Status = status;
            Description = status.GetDescription();
            Data = data;
            Total = total;
        }

        public T Data { get; private set; }
        public StatusProcess Status { get; private set; }
        public string Description { get; private set; }
        public int Total { get; private set; }
    }
}
