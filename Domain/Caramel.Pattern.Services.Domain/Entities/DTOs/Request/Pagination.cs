using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request
{
    [ExcludeFromCodeCoverage]
    public class Pagination
    {
        public Pagination() { }

        public Pagination(int page, int size)
        {
            Page = page;
            Size = size;
        }

        public int Page { get; set; }
        public int Size { get; set; }
    }
}
