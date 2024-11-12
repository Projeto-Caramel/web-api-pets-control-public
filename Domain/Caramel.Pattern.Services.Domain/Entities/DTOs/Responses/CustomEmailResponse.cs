using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Responses
{
    [ExcludeFromCodeCoverage]
    public class CustomEmailResponse
    {
        public CustomEmailResponse(string id, StatusProcess status)
        {
            Id = id;
            Status = status;
            Description = status.GetDescription();
        }

        public string Id { get; set; }
        public StatusProcess Status { get; set; }
        public string Description { get; set; }
    }
}
