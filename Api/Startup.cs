using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.ModelViews;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Servicos;
using dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Configuration;
using dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace dio_lab_trilha_net_minimal_api_desafio
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;
        private JWTSettings jwtSettings = new();

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.GetSection("Jwt").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            services.AddAuthorization();
            services.AddScoped<IAdministradorServico, AdministradorServico>();
            services.AddScoped<IVeiculosServico, VeiculosServico>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddDbContext<DBContexto>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString("mysql"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("mysql"))
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>{
                #region Home
                endpoints.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
                #endregion

                #region Administradores

                string GerarTokenJwt(Administrador administrador)
                {
                    SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(jwtSettings.Key));
                    SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

                    List<Claim> claims = [
                        new Claim("Email", administrador.Email),
                        new Claim("Perfil", administrador.Perfil),
                        new Claim(ClaimTypes.Role, administrador.Perfil)
                    ];

                    JwtSecurityToken token = new(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        audience: jwtSettings.Audience,
                        issuer: jwtSettings.Issuer,
                        signingCredentials: credentials
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }

                static ErrosDeValidacao validaDTOAdministrador(AdministradorDTO administradorDTO)
                {
                    ErrosDeValidacao validacao = new();

                    if (string.IsNullOrEmpty(administradorDTO.Email))
                        validacao.Mensagens.Add("O email não pode ficar em branco");

                    if (string.IsNullOrEmpty(administradorDTO.Senha))
                        validacao.Mensagens.Add("A senha não pode ficar em branco");

                    /*if(administradorDTO.Perfil is null)
                        validacao.Mensagens.Add("O perfil não pode ficar em branco");*/

                    return validacao;
                }

                endpoints.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
                {
                    ErrosDeValidacao validacao = validaDTOAdministrador(administradorDTO);

                    if (validacao.Mensagens.Count > 0)
                        return Results.BadRequest(validacao);

                    Administrador administrador = new()
                    {
                        Email = administradorDTO.Email,
                        Senha = administradorDTO.Senha,
                        Perfil = administradorDTO.Perfil.ToString()
                    };

                    administradorServico.Incluir(administrador);

                    return Results.Created($"/administrador/{administrador.Id}", administrador);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

                endpoints.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
                {
                    List<Administrador> administradores = administradorServico.Todos(pagina);
                    List<AdministradorModelView> administradoresModelView = [];

                    foreach (Administrador administrador in administradores)
                    {
                        administradoresModelView.Add(new()
                        {
                            Id = administrador.Id,
                            Email = administrador.Email,
                            Perfil = administrador.Perfil
                        });
                    }

                    return Results.Ok(administradoresModelView);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

                endpoints.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
                {
                    Administrador? administrador = administradorServico.BuscarPorId(id);

                    if (administrador is null)
                        return Results.NotFound();

                    AdministradorModelView administradorModelView = new()
                    {
                        Id = administrador.Id,
                        Email = administrador.Email,
                        Perfil = administrador.Perfil
                    };

                    return Results.Ok(administradorModelView);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

                endpoints.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
                {
                    Administrador? administradorLogado = administradorServico.Login(loginDTO);
                    if (administradorLogado != null)
                    {
                        string token = GerarTokenJwt(administradorLogado);
                        return Results.Ok(new AdminsitradorLogadoModelView()
                        {
                            Email = administradorLogado.Email,
                            Perfil = administradorLogado.Perfil,
                            Token = token
                        });
                    }
                    else
                        return Results.Unauthorized();
                }).AllowAnonymous().WithTags("Administradores");

                #endregion

                #region Veículos
                static ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
                {
                    ErrosDeValidacao validacao = new();

                    if (string.IsNullOrEmpty(veiculoDTO.Nome))
                        validacao.Mensagens.Add("O nome não pode ser vazio");

                    if (string.IsNullOrEmpty(veiculoDTO.Marca))
                        validacao.Mensagens.Add("A marca não pode ficar em branco");

                    if (veiculoDTO.Ano < 1950)
                        validacao.Mensagens.Add("Veículo muito antigo, aceito somente anos superiores a 1950");

                    return validacao;
                }

                endpoints.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
                {
                    ErrosDeValidacao validacao = validaDTO(veiculoDTO);

                    if (validacao.Mensagens.Count > 0)
                        return Results.BadRequest(validacao);

                    Veiculo veiculo = new()
                    {
                        Nome = veiculoDTO.Nome,
                        Marca = veiculoDTO.Marca,
                        Ano = veiculoDTO.Ano
                    };
                    veiculosServico.Incluir(veiculo);

                    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" }).WithTags("Veiculos");

                endpoints.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) =>
                {
                    List<Veiculo> veiculos = veiculosServico.Todos(pagina);
                    return Results.Ok(veiculos);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" }).WithTags("Veiculos");

                endpoints.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
                {
                    Veiculo? veiculo = veiculosServico.BuscaPorId(id);

                    return veiculo is null ? Results.NotFound() : Results.Ok(veiculo);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" }).WithTags("Veiculos");

                endpoints.MapPut("/veiculos/{id}", (int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
                {
                    ErrosDeValidacao validacao = validaDTO(veiculoDTO);

                    if (validacao.Mensagens.Count > 0)
                        return Results.BadRequest(validacao);

                    Veiculo? veiculoBanco = veiculosServico.BuscaPorId(id);
                    if (veiculoBanco is null)
                        return Results.NotFound();

                    veiculoBanco.Nome = veiculoDTO.Nome;
                    veiculoBanco.Ano = veiculoDTO.Ano;
                    veiculoBanco.Marca = veiculoDTO.Marca;

                    veiculosServico.Atualizar(veiculoBanco);
                    return Results.Ok(veiculoBanco);
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veiculos");

                endpoints.MapDelete("/veiculos/{id}", (int id, IVeiculosServico veiculosServico) =>
                {
                    Veiculo? veiculoBanco = veiculosServico.BuscaPorId(id);
                    if (veiculoBanco is null)
                        return Results.NotFound();

                    veiculosServico.Apagar(veiculoBanco);
                    return Results.NoContent();
                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veiculos");

                #endregion
            });
        }
    }
}