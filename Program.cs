using dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.ModelViews;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Servicos;
using dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculosServico>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DBContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
        else
        return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region Veículos
static ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    ErrosDeValidacao validacao = new();

    if(string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O nome não pode ser vazio");
        
    if(string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("A marca não pode ficar em branco");

    if(veiculoDTO.Ano < 1950)
        validacao.Mensagens.Add("Veículo muito antigo, aceito somente anos superiores a 1950");
    
    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) => {
    ErrosDeValidacao validacao = validaDTO(veiculoDTO);

    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    Veiculo veiculo = new()
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculosServico.Incluir(veiculo);

    return Results.Created($"/veiculos/{veiculo.Id}", veiculo); 
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) => {
    List<Veiculo> veiculos = veiculosServico.Todos(pagina);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) => {
    Veiculo? veiculo = veiculosServico.BuscaPorId(id);
    
    return veiculo is null? Results.NotFound() : Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", (int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) => {
    ErrosDeValidacao validacao = validaDTO(veiculoDTO);
    
    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);
        
    Veiculo? veiculoBanco = veiculosServico.BuscaPorId(id);
    if(veiculoBanco is null)
        return Results.NotFound();
    
    veiculoBanco.Nome = veiculoDTO.Nome;
    veiculoBanco.Ano = veiculoDTO.Ano;
    veiculoBanco.Marca = veiculoDTO.Marca;

    veiculosServico.Atualizar(veiculoBanco);
    return Results.Ok(veiculoBanco);
}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", (int id, IVeiculosServico veiculosServico) => {
    Veiculo? veiculoBanco = veiculosServico.BuscaPorId(id);
    if(veiculoBanco is null)
        return Results.NotFound();

    veiculosServico.Apagar(veiculoBanco);
    return Results.NoContent();
}).WithTags("Veiculos");

#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
