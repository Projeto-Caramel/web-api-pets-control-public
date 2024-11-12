using Caramel.Pattern.Services.Domain.Enums.Pets;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partner
{
    [ExcludeFromCodeCoverage]
    public class UpdatePasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public string CNPJ { get; set; }
        public string AdoptionRate { get; set; }
        public string PIX { get; set; }
        public string Website { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public OrganizationType Type { get; set; }
        public Roles Role { get; set; }
        public byte[] ProfileImageBytes { get; set; }
    }
}
