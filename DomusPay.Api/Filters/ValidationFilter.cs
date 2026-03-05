using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DomusPay.Api.Filters;

/// <summary>
/// Filtro de ação que intercepta as requisições e realiza a validação dos dados de entrada utilizando os validadores do FluentValidation.
/// Caso a validação falhe, retorna uma resposta HTTP 400 Bad Request com os detalhes dos erros de validação.
/// Caso a validação seja bem-sucedida, permite que a requisição prossiga normalmente para o próximo filtro ou para a ação do controlador.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values.Where(v => v != null))
            {
                var argumentType = argument.GetType();
                
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argument);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                        var problemDetails = new ProblemDetails
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Erro de validação nos dados enviados.",
                            Detail = string.Join("; ", errors),
                            Instance = context.HttpContext.Request.Path
                        };

                        context.Result = new BadRequestObjectResult(problemDetails);
                        return; 
                    }
                }
            }

            await next();
        }}
