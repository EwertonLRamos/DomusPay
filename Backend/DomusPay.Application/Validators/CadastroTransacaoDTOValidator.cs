using DomusPay.Application.DTOs;
using FluentValidation;

namespace DomusPay.Application.Validators;

public class CadastroTransacaoDTOValidator : AbstractValidator<CadastroTransacaoDTO>
{
    public CadastroTransacaoDTOValidator()
    {
        RuleFor(t => t.Valor)
            .NotEmpty()
                .WithMessage("O valor é obrigatório.")
            .GreaterThan(0)
                .WithMessage("O valor deve ser maior que zero.");

        RuleFor(t => t.Tipo)
            .NotEmpty()
                .WithMessage("O tipo é obrigatório.")
            .Must(t => t == "Receita" || t == "Despesa")
                .WithMessage("Os tipos disponíveis são: 'Receita' e 'Despesa'.");

        RuleFor(t => t.Descricao)
            .MaximumLength(400)
                .WithMessage("A descrição deve ter no máximo 400 caracteres.");

        RuleFor(t => t.CategoriaId)
            .NotEmpty()
            .NotNull()
                .WithMessage("O ID da categoria é obrigatório.");

        RuleFor(t => t.PessoaId)
            .NotEmpty()
            .NotNull()
                .WithMessage("O ID da pessoa é obrigatório.");
    }
}
