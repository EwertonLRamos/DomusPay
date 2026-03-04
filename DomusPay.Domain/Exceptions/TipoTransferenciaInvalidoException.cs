namespace DomusPay.Domain.Exceptions;

public class TipoTransferenciaInvalidoException(string tipo)
 : Exception($"Tipo de transferência '{tipo}' é inválido. Os tipos válidos são: 'Receita' e 'Despesa'.")
{
}
