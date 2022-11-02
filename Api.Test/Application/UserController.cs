using Api.Application.Controllers;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Controller
{
    public class UserController
    {
        private UsersController _controller;

        [Fact(DisplayName = "É possível salvar novo usuario")]
        public async Task EhPossivelSalvarNovoUsuario()
        {
            var _serviceMock = new Mock<IUserService>();

            var nomeUsuario = Faker.Name.FullName();
            var emailUsuario = Faker.Internet.Email();

            _serviceMock.Setup(m => m.Post(It.IsAny<UserDtoCreate>())).ReturnsAsync(
                new UserDtoCreateResult
                {
                    Id = Guid.NewGuid(),
                    Name = nomeUsuario,
                    Email = emailUsuario,
                    CreateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(_serviceMock.Object);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");

            _controller.Url = url.Object;

            var userDtoCreate = new UserDtoCreate { Name = nomeUsuario, Email = emailUsuario };

            var result = await _controller.Post(userDtoCreate);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as UserDtoCreateResult;
            Assert.NotNull(resultValue);
            Assert.Equal(userDtoCreate.Name, resultValue.Name);
            Assert.Equal(userDtoCreate.Email, resultValue.Email);
        }

        [Fact(DisplayName = "É retornado BadRequest ao tentar salvar novo usuario invalido")]
        public async Task EhRetornadoBadRequestAoTentarSalvarNovoUsuarioInvalido()
        {
            var _serviceMock = new Mock<IUserService>();

            var nomeUsuario = Faker.Name.FullName();
            var emailUsuario = Faker.Internet.Email();

            _serviceMock.Setup(m => m.Post(It.IsAny<UserDtoCreate>())).ReturnsAsync(
                new UserDtoCreateResult
                {
                    Id = Guid.NewGuid(),
                    Name = nomeUsuario,
                    Email = emailUsuario,
                    CreateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(_serviceMock.Object);
            //deixando o ModelState invalido
            _controller.ModelState.AddModelError("Name", "É um campo obrigatorio");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");

            _controller.Url = url.Object;

            var userDtoCreate = new UserDtoCreate { Name = nomeUsuario, Email = emailUsuario };

            var result = await _controller.Post(userDtoCreate);
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact(DisplayName = "É possível realizar o update")]
        public async Task EhPossivelAtualizarUsuario()
        {
            var _serviceMock = new Mock<IUserService>();

            var nomeUsuario = Faker.Name.FullName();
            var emailUsuario = Faker.Internet.Email();

            _serviceMock.Setup(m => m.Put(It.IsAny<UserDtoUpdate>())).ReturnsAsync(
                new UserDtoUpdateResult
                {
                    Id = Guid.NewGuid(),
                    Name = nomeUsuario,
                    Email = emailUsuario,
                    UpdateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(_serviceMock.Object);

            var userDtoUpdate = new UserDtoUpdate { Id = Guid.NewGuid(), Name = nomeUsuario, Email = emailUsuario };

            var result = await _controller.Put(userDtoUpdate);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as UserDtoUpdateResult;
            Assert.NotNull(resultValue);
            Assert.Equal(userDtoUpdate.Name, resultValue.Name);
            Assert.Equal(userDtoUpdate.Email, resultValue.Email);
        }

        [Fact(DisplayName = "É retornado BadRequest ao tentar o update")]
        public async Task EhRetornadoBadRequestAoTentarAtualizarUsuarioInvalido()
        {
            var _serviceMock = new Mock<IUserService>();

            var nomeUsuario = Faker.Name.FullName();
            var emailUsuario = Faker.Internet.Email();

            _serviceMock.Setup(m => m.Put(It.IsAny<UserDtoUpdate>())).ReturnsAsync(
                new UserDtoUpdateResult
                {
                    Id = Guid.NewGuid(),
                    Name = nomeUsuario,
                    Email = emailUsuario,
                    UpdateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(_serviceMock.Object);
            _controller.ModelState.AddModelError("Email", "Campo obrigatorio");

            var userDtoUpdate = new UserDtoUpdate { Id = Guid.NewGuid(), Name = nomeUsuario, Email = emailUsuario };

            var result = await _controller.Put(userDtoUpdate);
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact(DisplayName = "É possível realizar o delete")]
        public async Task EhPossivelDeletarUsuario()
        {
            var _serviceMock = new Mock<IUserService>();

            _serviceMock.Setup(m => m.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

            _controller = new UsersController(_serviceMock.Object);

            var result = await _controller.Delete(Guid.NewGuid());
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value;
            Assert.NotNull(resultValue);
            Assert.True((Boolean)resultValue);
        }

        [Fact(DisplayName = "É retornado BadRequest ao tentar o delete")]
        public async Task EhRetornadoBadRequestAoTentarDeletarUsuarioInvalido()
        {
            var _serviceMock = new Mock<IUserService>();

            _serviceMock.Setup(m => m.Delete(It.IsAny<Guid>())).ReturnsAsync(false);

            _controller = new UsersController(_serviceMock.Object);
            _controller.ModelState.AddModelError("Id", "Campo obrigatorio");

            //passando um GUID zerado
            var result = await _controller.Delete(default(Guid));
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact(DisplayName = "É possível realizar o GET")]
        public async Task EhPossivelBuscarUsuario()
        {
            var _serviceMock = new Mock<IUserService>();

            var name = Faker.Name.FullName();
            var email = Faker.Internet.Email();

            _serviceMock.Setup(m => m.Get(It.IsAny<Guid>())).ReturnsAsync(
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    CreateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(_serviceMock.Object);

            var result = await _controller.Get(Guid.NewGuid());
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as UserDto;
            Assert.NotNull(resultValue);
            Assert.Equal(name, resultValue.Name);
            Assert.Equal(email, resultValue.Email);
        }

        [Fact(DisplayName = "É retornado BadRequest ao tentar o GET")]
        public async Task EhRetornadoBadRequestAoTentarBuscarUsuarioInvalido()
        {
            var _serviceMock = new Mock<IUserService>();

            _serviceMock.Setup(m => m.Delete(It.IsAny<Guid>())).ReturnsAsync(false);

            _controller = new UsersController(_serviceMock.Object);
            _controller.ModelState.AddModelError("Id", "Campo obrigatorio");

            //passando um GUID zerado
            var result = await _controller.Delete(default(Guid));
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact(DisplayName = "É possível realizar o GETALL")]
        public async Task EhPossivelBuscarTodosUsuario()
        {
            var _serviceMock = new Mock<IUserService>();

            var name = Faker.Name.FullName();
            var email = Faker.Internet.Email();

            _serviceMock.Setup(m => m.GetAll()).ReturnsAsync(
                new List<UserDto>
                {
                   new UserDto
                    {
                        Id = Guid.NewGuid(),
                        Name = Faker.Name.FullName(),
                        Email =  Faker.Internet.Email(),
                        CreateAt = DateTime.UtcNow
                    },
                   new UserDto
                    {
                        Id = Guid.NewGuid(),
                        Name = Faker.Name.FullName(),
                        Email =  Faker.Internet.Email(),
                        CreateAt = DateTime.UtcNow
                    }
                }
            );

            _controller = new UsersController(_serviceMock.Object);

            var result = await _controller.GetAll();
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as IEnumerable<UserDto>;
            Assert.True(resultValue.Count() == 2);
        }

        [Fact(DisplayName = "É retornado BadRequest ao tentar o GETALL")]
        public async Task EhRetornadoBadRequestAoTentarBuscarTodosUsuarioInvalido()
        {
            var _serviceMock = new Mock<IUserService>();


            _serviceMock.Setup(m => m.GetAll()).ReturnsAsync(
                new List<UserDto>
                {
                   new UserDto
                    {
                        Id = Guid.NewGuid(),
                        Name = Faker.Name.FullName(),
                        Email =  Faker.Internet.Email(),
                        CreateAt = DateTime.UtcNow
                    },
                   new UserDto
                    {
                        Id = Guid.NewGuid(),
                        Name = Faker.Name.FullName(),
                        Email =  Faker.Internet.Email(),
                        CreateAt = DateTime.UtcNow
                    }
                }
            );

            _controller = new UsersController(_serviceMock.Object);
            _controller.ModelState.AddModelError("Id", "Campo obrigatorio");

            var result = await _controller.GetAll();
            Assert.True(result is BadRequestObjectResult);
        }
    }
}
