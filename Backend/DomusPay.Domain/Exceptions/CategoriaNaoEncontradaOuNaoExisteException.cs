namespace DomusPay.Domain.Exceptions;

public class CategoriaNaoEncontradaOuNaoExisteException(Guid id)
 : Exception($"Categoria com ID {id} não encontrada ou não existe.")
{
}
