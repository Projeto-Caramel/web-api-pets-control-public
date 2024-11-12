﻿using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Responses
{
    [ExcludeFromCodeCoverage]
    public class CustomUpdateResponse<T>
    {
        public CustomUpdateResponse(T data, StatusProcess status)
        {
            Data = data;
            Status = status;
            Description = status.GetDescription();
        }

        public T Data { get; set; }
        public StatusProcess Status { get; set; }
        public string Description { get; set; }
    }
}
