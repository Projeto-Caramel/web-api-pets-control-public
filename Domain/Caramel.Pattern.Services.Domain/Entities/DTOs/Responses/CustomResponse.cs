using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Responses
{
    [ExcludeFromCodeCoverage]
    public class CustomResponse<T>
    {
        public CustomResponse(T data, StatusProcess status)
        {
            this.status = status;
            Description = status.GetDescription();
            Data = data;
        }

        public T Data { get; set; }
        public StatusProcess status { get; set; }
        public string Description { get; set; }
    }
}
