using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Data
{
    public class UsuarioCrudCompleto : BaseTestData, IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public UsuarioCrudCompleto(DbTest dbTest)
        {
            _serviceProvider = dbTest.serviceProvider;
        }

        [Fact(DisplayName ="CRUD DE USUARIO")]
        [Trait("CRUD", "UserEntity")]
        public async Task EhPossivelRealizarCrudUsuario()
        {
            using (var context = _serviceProvider.GetService<MyContext>())
            {
                UserRepository _repositorio = new UserRepository(context);
                UserEntity usuario = new UserEntity
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName()
                };

                var registroCriado = await _repositorio.InsertAsync(usuario);

                Assert.NotNull(registroCriado);
                Assert.Equal(usuario.Email, registroCriado.Email);
                Assert.Equal(usuario.Name, registroCriado.Name);
                Assert.False(registroCriado.Id == Guid.Empty);

                usuario.Name = Faker.Name.First();
                var registroAtualizado = await _repositorio.UpdateAsync(usuario);

                Assert.NotNull(registroAtualizado);
                Assert.Equal(usuario.Name, registroAtualizado.Name);
                Assert.Equal(usuario.Email, registroAtualizado.Email);

                var registroExiste = await _repositorio.ExistAsync(registroAtualizado.Id);
                Assert.True(registroExiste);

                var registroSelecionado = await _repositorio.SelectAsync(registroAtualizado.Id);
                Assert.NotNull(registroSelecionado);
                Assert.Equal(registroSelecionado , registroAtualizado);

                var todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(todosRegistros);
                Assert.True(todosRegistros.Count() > 0);

                var achouPeloLogin = await _repositorio.FindByLogin(registroSelecionado.Email);

                Assert.NotNull(achouPeloLogin);
                Assert.Equal(achouPeloLogin, registroSelecionado);

                var removeu = await _repositorio.DeleteAsync(registroSelecionado.Id);
                Assert.True(removeu);

            }
        }

    }
}
