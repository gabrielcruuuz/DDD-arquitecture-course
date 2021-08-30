using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;
        private SigningConfiguration _signingCredentials { get; set; }
        private TokenConfiguration _tokenConfiguration { get; set; }
        private IConfiguration _configuration { get; set; }

        public LoginService(IUserRepository repository, SigningConfiguration signingCredentials, 
                            TokenConfiguration tokenConfiguration, IConfiguration configuration)
        {
            _repository = repository;
            _signingCredentials = signingCredentials;
            _tokenConfiguration = tokenConfiguration;
            _configuration = configuration;
        }

        public async Task<object> FindByLogin(LoginDto login)
        {
            if (login != null && !string.IsNullOrWhiteSpace(login.Email))
            {
                UserEntity user = await _repository.FindByLogin(login.Email);
                if (user == null)
                {
                    return new                                                                                          
                    {
                        authenticated = false,
                        message = "Email não encontrado"
                    };
                }

                else
                {
                    DateTime createDate = DateTime.Now;
                    DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                    string token = CreateToken(user, createDate, expirationDate);
                    return SuccessObject(user, createDate, expirationDate, token);
                }
            }

            return new
            {
                authenticated = false,
                message = "Falha ao autenticar"
            };
        }

        private string CreateToken(UserEntity user, DateTime createDate, DateTime expirationDate)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                      new GenericIdentity(user.Email),
                      new[]
                      {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                      }
                  );

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingCredentials.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            string token = handler.WriteToken(securityToken);

            return token;
        }

        private object SuccessObject(UserEntity user, DateTime createDate, DateTime expirationDate, string token)
        {
            return new
            {
                authenticated = true,
                message = "Usuário logado com sucesso.",
                creted = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = token,
                email = user.Email,
            };
        }
    }
}
