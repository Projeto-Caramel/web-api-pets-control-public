using Caramel.Pattern.Services.Domain.Entities.Models.Users;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl
{
    public interface IPartnerApiService
    {
        Task<Partner> GetSingleOrDefaultByIdAsync(string id);
    }
}
