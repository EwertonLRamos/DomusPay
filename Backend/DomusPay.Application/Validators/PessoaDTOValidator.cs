using DomusPay.Application.DTOs;
using FluentValidation;

namespace DomusPay.Application.Validators;

public class PessoaDTOValidator : AbstractValidator<PessoaDTO>
{
    public PessoaDTOValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
                .WithMessage("O nome é obrigatório.")
            .MaximumLength(200)
                .WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(p => p.Idade)
            .NotEmpty()
                .WithMessage("A idade é obrigatória.")
            .InclusiveBetween(0, 150)
                .WithMessage("A idade deve estar entre 0 e 150.");

        RuleFor(t => t.Id)
            .NotEmpty()
            .NotNull()
                .WithMessage("O ID da pessoa é obrigatório.");
    }
}
