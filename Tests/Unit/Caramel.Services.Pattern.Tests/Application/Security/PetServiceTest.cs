using Caramel.Pattern.Services.Application.Services.Pets;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Services.Pattern.Tests.Mocks.Data;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Security
{
    [ExcludeFromCodeCoverage]
    public class PetServiceTest
    {
        private readonly Mock<IBucketService> _bucketService;
    
        public PetServiceTest()
        {
            _bucketService = new Mock<IBucketService>();

            _bucketService.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("teste");
            _bucketService.Setup(x => x.DeleteFileAsync(It.IsAny<string>())).Verifiable();
            _bucketService.Setup(x => x.GetImagesForEntityAsync(It.IsAny<string>())).ReturnsAsync(["teste"]);
        }


        [Fact]
        public async Task GetSingleOrDefaultByIdAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data["Basic"]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var pet = await service.GetSingleOrDefaultByIdAsync("t344r");

            Assert.NotNull(pet);
            Assert.Equivalent(PetData.Data["Basic"], pet);
        }

        [Theory]
        [InlineData("", "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData(null, "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("t344r", "Null", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest)]
        public async Task GetSingleOrDefaultByIdAsync_BusinessExceptions(
            string petDataKey,
            string petDataRequest,
            string message,
            StatusProcess statusProcess,
            HttpStatusCode httpStatus)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data[petDataRequest]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.GetSingleOrDefaultByIdAsync(petDataKey));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(statusProcess, exception.Status);
            Assert.Equal(httpStatus, exception.StatusCode);
        }

        [Fact]
        public async Task GetPetStatusAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data["Basic"]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var petStatus = await service.GetPetStatusAsync("t344r");

            Assert.Equivalent(PetData.Data["Basic"].Info.Status, petStatus);
        }

        [Theory]
        [InlineData("", "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData(null, "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("t344r", "Null", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest)]
        [InlineData("t344r", "Empty", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest)]
        public async Task GetPetStatusAsync_BusinessExceptions(
           string petDataKey,
           string petDataRequest,
           string message,
           StatusProcess statusProcess,
           HttpStatusCode httpStatus)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data[petDataRequest]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.GetPetStatusAsync(petDataKey));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(statusProcess, exception.Status);
            Assert.Equal(httpStatus, exception.StatusCode);
        }

        [Fact]
        public async Task FetchAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Pets.FetchAsync(It.IsAny<Expression<Func<Pet, bool>>>())).ReturnsAsync(new List<Pet>
            {
                PetData.Data["Basic"]
            });

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var pets = await service.FetchAsync("t344r");

            Assert.NotNull(pets);
            Assert.Equivalent(new List<Pet> { PetData.Data["Basic"] }, pets);
        }

        [Theory]
        [InlineData("", "Basic", "O campo Partner ID é obrigatório.")]
        [InlineData(null, "Basic", "O campo Partner ID é obrigatório.")]
        [InlineData("t344r", "Null", "Não foi possível encontrar nenhum Partner com esse ID.")]
        public async Task FetchAsync_BusinessExceptions(
           string petDataKey,
           string petDataRequest,
           string message)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data[petDataRequest]);
            unitOfWorkMock.Setup(x => x.Pets.FetchAsync(It.IsAny<Expression<Func<Pet, bool>>>())).ReturnsAsync(new List<Pet>
            {
                PetData.Data["Basic"]
            });

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.FetchAsync(petDataKey));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task FetchByFilterAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Pets.FetchAsync(It.IsAny<Expression<Func<Pet, bool>>>())).ReturnsAsync(new List<Pet>
            {
                PetData.Data["Basic"]
            });

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var pets = await service.FetchByFilterAsync("t344r", PetFilterData.Data["Basic"]);

            Assert.NotNull(pets);
            Assert.Equivalent(new List<Pet> { PetData.Data["Basic"] }, pets);
        }

        [Theory]
        [InlineData("", "Basic", "Basic", "O campo Partner ID é obrigatório.")]
        [InlineData(null, "Basic", "Basic", "O campo Partner ID é obrigatório.")]
        [InlineData("t344r", "Null", "Null", "O parâmetro Pet Filter não pode ser nulo.")]
        public async Task FetchByFilterAsync_BusinessException(
            string petDataKey,
            string petDataRequest,
            string filterDataRequest,
            string message)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data[petDataRequest]);
            unitOfWorkMock.Setup(x => x.Pets.FetchAsync(It.IsAny<Expression<Func<Pet, bool>>>())).ReturnsAsync(new List<Pet>
            {
                PetData.Data["Basic"]
            });

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.FetchByFilterAsync(petDataKey, PetFilterData.Data[filterDataRequest]));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task RegisterAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Pets.AddAsync(It.IsAny<Pet>())).Verifiable();

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var pet = await service.RegisterAsync(PetData.Data["Basic"], "teste");

            Assert.NotNull(pet);
            Assert.Equivalent(PetData.Data["Basic"], pet);
        }

        [Theory]
        [InlineData("Null", "Basic", "O parâmetro Pet não pode ser nulo.")]
        [InlineData("Basic", "Null", "Não foi possível encontrar nenhum Partner com esse ID.")]
        public async Task RegisterAsync_BusinessException(
            string petDataKey,
            string petDataRequest,
            string message)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data[petDataRequest]);
            unitOfWorkMock.Setup(x => x.Pets.AddAsync(It.IsAny<Pet>())).Verifiable();

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.RegisterAsync(PetData.Data[petDataKey], "teste"));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);

        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Pets.Update(It.IsAny<Pet>())).Verifiable();

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var result = await service.UpdateAsync(PetData.Data["Basic"], "teste");

            unitOfWorkMock.Verify(u => u.Pets.Update(It.IsAny<Pet>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equivalent(PetData.Data["Basic"], result);
        }

        [Theory]
        [InlineData("Null", "Basic", "Basic", "O parâmetro Pet não pode ser nulo.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("WithoutId", "Basic", "Basic",  "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("Basic", "Null", "Basic", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest)]
        [InlineData("Basic", "Basic", "Null", "Não foi possível encontrar nenhum Partner com esse ID.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        public async Task UpdateAsync_BusinessException(
            string petDataKey,
            string petDataRequest,
            string partnerDataRequest,
            string message, 
            StatusProcess statusProcess,
            HttpStatusCode httpStatus)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            apiService.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data[partnerDataRequest]);

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data[petDataRequest]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            service.UpdateAsync(PetData.Data[petDataKey], "teste"));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(statusProcess, exception.Status);
            Assert.Equal(httpStatus, exception.StatusCode);
        }

        [Fact]
        public async Task UpdateStatusAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Pets.Update(It.IsAny<Pet>())).Verifiable();

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var result = await service.UpdateStatusAsync("t3st3", PetStatus.AdoptOng);

            unitOfWorkMock.Verify(u => u.Pets.Update(It.IsAny<Pet>()), Times.Once);

            Assert.Equivalent(PetStatus.AdoptOng, result);
        }


        [Theory]
        [InlineData("", "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("t3st3","Null", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest)]
        [InlineData("t3st3", "Empty", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.UnprocessableEntity)]
        public async Task UpdateStatusAsync_BusinessException(
            string petDataKey,
            string petDataRequest,
            string message, 
            StatusProcess statusProcess,
            HttpStatusCode httpStatus)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>()))
                .ReturnsAsync(PetData.Data[petDataRequest]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.UpdateStatusAsync(petDataKey, PetStatus.AdoptOng));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(statusProcess, exception.Status);
            Assert.Equal(httpStatus, exception.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>()))
                .ReturnsAsync(PetData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Pets.Delete(It.IsAny<Pet>())).Verifiable();

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            await service.DeleteAsync("t3st3");

            unitOfWorkMock.Verify(x => x.Pets.Delete(It.Is<Pet>(p => p == PetData.Data["Basic"])), Times.Once);
        }

        [Theory]
        [InlineData("", "Basic", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("t3st3", "Null", "Não foi possível encontrar nenhum Pet com essas informações.", StatusProcess.Failure, HttpStatusCode.BadRequest)]
        public async Task DeleteAsync_BusinessException(
            string petDataKey,
            string petDataRequest,
            string message,
            StatusProcess statusProcess,
            HttpStatusCode httpStatus)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var apiService = new Mock<IPartnerApiService>();

            unitOfWorkMock.Setup(x => x.Pets.GetSingleAsync(It.IsAny<string>())).ReturnsAsync(PetData.Data[petDataRequest]);

            var service = new PetService(unitOfWorkMock.Object, apiService.Object, _bucketService.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.DeleteAsync(petDataKey));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(statusProcess, exception.Status);
            Assert.Equal(httpStatus, exception.StatusCode);
        }
    }
}
