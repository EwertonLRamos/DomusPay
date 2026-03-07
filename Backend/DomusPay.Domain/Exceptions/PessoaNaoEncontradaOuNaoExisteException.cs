namespace DomusPay.Domain.Exceptions;

public class PessoaNaoEncontradaOuNaoExisteException(Guid id)
 : Exception($"Pessoa com ID {id} não encontrada ou não existe.")
{
}
