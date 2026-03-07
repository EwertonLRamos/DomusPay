using DomusPay.Api.Filters;
using DomusPay.Api.Middlewares;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Application.Services;
using DomusPay.Application.Validators;
using DomusPay.Infrastructure;
using DomusPay.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

builder.Services.AddDbContext<DomusPayDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddValidatorsFromAssemblyContaining<CadastroPessoaDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CadastroCategoriaDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CadastroTransacaoDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PessoaDTOValidator>();

builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
