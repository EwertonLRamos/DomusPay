using DomusPay.Application.DTOs;
using FluentValidation;

namespace DomusPay.Application.Validators;

public class CadastroCategoriaDTOValidator : AbstractValidator<CadastroCategoriaDTO>
{
    public CadastroCategoriaDTOValidator()
    {
        RuleFor(c => c.Descricao)
            .NotEmpty()
                .WithMessage("A descrição é obrigatória.")
            .MaximumLength(400)
                .WithMessage("A descrição deve ter no máximo 400 caracteres.");

        RuleFor(c => c.Finalidade)
            .NotEmpty()
                .WithMessage("A finalidade é obrigatória.")
            .Must(f => f == "Receita" || f == "Despesa" || f == "Ambas")
                .WithMessage("As finalidades disponíveis são: 'Receita', 'Despesa' ou 'Ambas'.");
    }
}
