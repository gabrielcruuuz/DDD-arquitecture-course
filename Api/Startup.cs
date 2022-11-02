using Api.CrossCutting.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //configuração para os teste integrados
            if (_environment.IsEnvironment("Testing"))
            {
                Environment.SetEnvironmentVariable("DB_CONNECTION", "Server=localhost\\SQLEXPRESS;Database=DDD_Integration;user=gabrielCruz;password=56210160Casa");
                Environment.SetEnvironmentVariable("DATABASE", "SQLSERVER");
                Environment.SetEnvironmentVariable("TokenAudience", "ExemploAudience");
                Environment.SetEnvironmentVariable("TokenSeconds", "3600");
                Environment.SetEnvironmentVariable("TokenIssuer", "ExemploIssuer");
            }

            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            ConfigureToken.ConfigureDependenciesToken(services, Configuration);
            services.AddSingleton(ConfigureAutoMapper.ConfigureMapper()); 

            services.AddControllers();

            //PARAMETROS OPCIONAIS NO ADD SWAGGER GEN E AUTORIZAÇÃO
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = ".net Core com DDD",
                    Description = "Curso arquitetura DDD com .netCore e entity",
                    TermsOfService = new Uri("http://www.mfrinfo.com.br")
                    //Contact = new OpenApiContact { Name = "Gabriel", Email = "teste@teste.com",  }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference{
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aplicação DDD");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
