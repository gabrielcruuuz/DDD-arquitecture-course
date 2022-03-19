using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Service.Usuario
{
    public class UsuarioTest : UsuarioDados
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName ="GET")]
        public async Task EhPossivelExecutarGet()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Get(IdUsuario)).ReturnsAsync(userDto);
            _service = _serviceMock.Object;

            var result = await _service.Get(IdUsuario);
            Assert.NotNull(result);
            Assert.True(result.Id == IdUsuario);
            Assert.Equal(NomeUsuario, result.Name);
        }

        [Fact(DisplayName = "GETALL")]
        public async Task EhPossivelExecutarGetAll()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.GetAll()).ReturnsAsync(listaUser);
            _service = _serviceMock.Object;

            var result = await _service.GetAll();
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
            Assert.True(result.Count() == 10);

            var _listResult = new List<UserDto>();
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.GetAll()).ReturnsAsync(_listResult);
            _service = _serviceMock.Object;

            var _resultEmpty = await _service.GetAll();

            Assert.Empty(_resultEmpty);
            Assert.True(_resultEmpty.Count() == 0);
        }

        [Fact(DisplayName = "POST")]
        public async Task EhPossivelExecutarAdicionar()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Post(userDtoCreate)).ReturnsAsync(userDtoCreateResult);
            _service = _serviceMock.Object;

            var result = await _service.Post(userDtoCreate);
            Assert.NotNull(result);
            Assert.Equal(NomeUsuario, result.Name);
            Assert.Equal(EmailUsuario, result.Email);

        }

        [Fact(DisplayName = "PUT")]
        public async Task EhPossivelExecutarAtualizar()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Put(userDtoUpdate)).ReturnsAsync(userDtoUpdateResult);
            _service = _serviceMock.Object;

            var result = await _service.Put(userDtoUpdate);
            Assert.NotNull(result);
            Assert.Equal(NomeUsuarioAlterado, result.Name);
            Assert.Equal(EmailUsuarioAlterado, result.Email);

        }


        [Fact(DisplayName = "DELETE")]
        public async Task EhPossivelExecutarDeletar()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Delete(IdUsuario)).ReturnsAsync(true);
            _service = _serviceMock.Object;

            var deletado = await _service.Delete(IdUsuario);
            Assert.True(deletado);

            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Delete((It.IsAny<Guid>()))).ReturnsAsync(false);
            _service = _serviceMock.Object;

            deletado = await _service.Delete(Guid.NewGuid());
            Assert.False(deletado);
        }

    }
}
