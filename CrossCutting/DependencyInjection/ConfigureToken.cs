using Api.Domain.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.CrossCutting.DependencyInjection
{
    public static class ConfigureToken
    {
        public static void ConfigureDependenciesToken(IServiceCollection serviceColletion, IConfiguration configuration)
        {
            SigningConfiguration signingConfiguration = new SigningConfiguration();
            serviceColletion.AddSingleton(signingConfiguration);           

            serviceColletion.AddAuthentication(authOption =>
            {
                authOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions => 
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfiguration.Key;
                paramsValidation.ValidAudience = Environment.GetEnvironmentVariable("TokenAudience");
                paramsValidation.ValidIssuer = Environment.GetEnvironmentVariable("TokenIssuer");
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            serviceColletion.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

        }
    }
}
