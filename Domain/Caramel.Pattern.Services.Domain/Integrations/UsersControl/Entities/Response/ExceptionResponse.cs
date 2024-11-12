using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Response
{
    [ExcludeFromCodeCoverage]
    public class ExceptionResponse
    {
        public ExceptionResponse() { }

        public ExceptionResponse(StatusProcess status, object details)
        {
            Status = status;
            Description = status.GetDescription();
            ErrorDetails = details;
        }

        public StatusProcess Status { get; set; }
        public string Description { get; set; }
        public object ErrorDetails { get; set; }

    }
}
