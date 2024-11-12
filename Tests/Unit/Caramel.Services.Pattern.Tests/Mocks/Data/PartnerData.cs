using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class PartnerData
    {
        public static Dictionary<string, Partner> Data = new Dictionary<string, Partner>
        {
            {
                "Basic", new Partner()
                {
                    Id = "t35t3",
                    Name = "Basic",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Role = 0,
                    Email = "test@basic.com",
                    MaxCapacity = 100
                }
            },
            {
                "InvalidPassword", new Partner()
                {
                    Id = "t35t3",
                    Name = "Invalid",
                    Password = "XW+cxE2WUFQMkjEKgwT2Hw==",
                    Role = 0,
                    Email = "test@basic.com",
                    MaxCapacity = 100
                }
            },
            {
                "BasicUpdate", new Partner()
                {
                    Id = "t35t3",
                    Name = "Basic Update",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Role = 0,
                    Email = "test@basic-update.com",
                    MaxCapacity = 10
                }
            },
            {
                "UpdateException", new Partner()
                {
                    Id = "t35t3-3rr0r",
                    Name = "Update exception",
                    Password = "dTy7m726FLaYCQWMdzOYTg==",
                    Role = 0,
                    Email = "test@update-exception.com",
                    MaxCapacity = 100
                }
            },
            { "Empty", new Partner() { Id = "t35t3" } },
            { "WithoutId", new Partner() },
            { "Null", null },
        };
    }
}
