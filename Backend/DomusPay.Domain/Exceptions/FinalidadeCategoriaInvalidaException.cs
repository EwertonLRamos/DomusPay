namespace DomusPay.Domain.Exceptions;

public class FinalidadeCategoriaInvalidaException(string finalidade)
 : Exception($"Finalidade da categoria '{finalidade}' é inválida. Valores permitidos: Receita, Despesa, Ambas.")
{
}
