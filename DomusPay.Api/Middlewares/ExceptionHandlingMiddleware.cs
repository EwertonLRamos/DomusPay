using System.Net;
using System.Text.Json;
using DomusPay.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await TratarExceptionAsync(context, ex);
        }
    }

    private static Task TratarExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var title = "Ocorreu um erro inesperado no servidor.";

        switch (exception)
        {
            case ArgumentException _:
                statusCode = HttpStatusCode.BadRequest;
                title = "Requisição inválida.";
                break;
            case CategoriaIncompativelComFinalidadeException _:
            case PessoaMenorNaoPodeTerReceitaException _:
            case TipoTransferenciaInvalidoException _:
            case FinalidadeCategoriaInvalidaException _:
                statusCode = HttpStatusCode.BadRequest;
                title = "Inconsistência nas informações inseridas.";
                break;
            case PessoaNaoEncontradaOuNaoExisteException _:
            case CategoriaNaoEncontradaOuNaoExisteException _:
                statusCode = HttpStatusCode.NotFound;
                title = "Informação não encontrada.";
                break;
        }

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, options);

        return context.Response.WriteAsync(result);
    }
}
