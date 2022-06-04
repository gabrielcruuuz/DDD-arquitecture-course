using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Api.Test.Service.AutoMapper
{
    public class UsuarioMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void EhPossivelMapearModelos()
        {
            var model = new UserModel
            {
                Id = Guid.NewGuid(),
                Name = Faker.Name.FullName(),
                Email = Faker.Internet.Email(),
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            // Model > Entity
            var entity = Mapper.Map<UserEntity>(model);

            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Email, model.Email);

            // Entity > DTO
            var dto = Mapper.Map<UserDto>(entity);

            Assert.Equal(dto.Id, entity.Id);
            Assert.Equal(dto.Name, entity.Name);
            Assert.Equal(dto.Email, entity.Email);

            //Entity > DTO CREATE 
            var dtoCreate = Mapper.Map<UserDtoCreateResult>(entity);

            Assert.Equal(dtoCreate.Id, entity.Id);
            Assert.Equal(dtoCreate.Name, entity.Name);
            Assert.Equal(dtoCreate.Email, entity.Email);
            Assert.Equal(dtoCreate.CreateAt, entity.CreateAt);

            //Entity > DTO UPDATE 
            var dtoUpdate = Mapper.Map<UserDtoUpdateResult>(entity);

            Assert.Equal(dtoUpdate.Id, entity.Id);
            Assert.Equal(dtoUpdate.Name, entity.Name);
            Assert.Equal(dtoUpdate.Email, entity.Email);
            Assert.Equal(dtoUpdate.UpdateAt, entity.UpdateAt);

            // DTO > Model
            var model2 = Mapper.Map<UserModel>(dto);

            Assert.Equal(model2.Id, dto.Id);
            Assert.Equal(model2.Name, dto.Name);
            Assert.Equal(model2.Email, dto.Email);

        }
    }
}
