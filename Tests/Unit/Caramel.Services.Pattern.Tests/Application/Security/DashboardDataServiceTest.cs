using Caramel.Pattern.Services.Application.Services.Dashboard;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services.Pets;
using Caramel.Services.Pattern.Tests.Mocks.Data;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Security
{
    [ExcludeFromCodeCoverage]
    public class DashboardDataServiceTest
    {

        [Fact]
        public async Task GetSingleOrDefaultByIdAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();
            var petService = new Mock<IPetService>();

            petService.Setup(x => x.FetchAsync(It.IsAny<string>())).ReturnsAsync(new List<Pet>
            {
                PetData.Data["NotVaccinated"]
            });

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);

            var service = new DashboardDataService(unitOfWorkMock.Object, petService.Object, apiService.Object);

            var dashboard = await service.GetSingleOrDefaultByIdAsync("t344r");

            Assert.NotNull(dashboard);
            Assert.Equivalent(DashData.Data["Basic"], dashboard);
        }

        [Theory]
        [InlineData("", "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData(null, "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("t344r", "Null", "Não foi possível encontrar nenhum Partner com esse Id.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        public async Task GetSingleOrDefaultByIdAsync_BusinessExceptions(
            string partnerDataKey,
            string partnerDataRequest,
            string message,
            StatusProcess statusProcess,
            HttpStatusCode httpStatus)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();
            var petService = new Mock<IPetService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data[partnerDataRequest]);

            unitOfWorkMock.Setup(x => x.Pets.FetchAsync(It.IsAny<Expression<Func<Pet, bool>>>())).ReturnsAsync(new List<Pet>
            {
                PetData.Data["Basic"]
            });
            

            var service = new DashboardDataService(unitOfWorkMock.Object, petService.Object, apiService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.GetSingleOrDefaultByIdAsync(partnerDataKey));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(statusProcess, exception.Status);
            Assert.Equal(httpStatus, exception.StatusCode);
        }
    }
}
