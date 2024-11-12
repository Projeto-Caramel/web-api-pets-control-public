using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Pattern.Services.Domain.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<Domain.Entities.Models.Dashboard.DashboardData> GetSingleOrDefaultByIdAsync(string partnerId);
    }
}
