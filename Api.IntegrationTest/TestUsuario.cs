using Api.Domain.Dtos.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.IntegrationTest
{
    public class TestUsuario : BaseIntegration
    {

        private string _name{ get; set; }
        private string _email { get; set; }

        [Fact]
        public async Task PostDeUsuario()
        {
            await AddToken();

            _name = Faker.Name.First();
            _email = Faker.Internet.Email();

            var UserDto = new UserDtoCreate()
            {
                Name = _name,
                Email = _email
            };

            var response = await PostJsonAsync(UserDto, $"{hostApi}/users", client);
            var postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<UserDtoCreateResult>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(registroPost);
            Assert.Equal(_name, registroPost.Name);
            Assert.Equal(_email, registroPost.Email);
            Assert.False(registroPost.Id == default(Guid));
        }
    }
}
