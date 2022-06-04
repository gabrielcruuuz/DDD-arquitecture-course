using Api.Domain.Dtos;
using Api.Domain.Interfaces.Services.User;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Service.Login
{
    public class LoginTest
    {
        private ILoginService _service;

        private Mock<ILoginService> _serviceMock;

        [Fact(DisplayName = "É possivel executar find by login")]
        public async Task EhPossivelExecutarFindByLogin()
        {
            var email = Faker.Internet.Email();

            var objetoRetorno = new
            {
                authenticated = true,
                message = "Usuário logado com sucesso.",
                creted = DateTime.UtcNow,
                expiration = DateTime.UtcNow.AddHours(8),
                acessToken = Guid.NewGuid(),
                email = email,
                name = Faker.Name.FullName()
            };

            var loginDto = new LoginDto
            {
                Email = email
            };

            _serviceMock = new Mock<ILoginService>();
            _serviceMock.Setup(m => m.FindByLogin(loginDto)).ReturnsAsync(objetoRetorno);
            _service = _serviceMock.Object;

            var result = await _service.FindByLogin(loginDto);
            Assert.NotNull(result);

        }
    }
}
