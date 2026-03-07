namespace DomusPay.Domain.Exceptions;

public class PessoaMenorNaoPodeTerReceitaException()
 : Exception("Pessoa menor de idade não pode ter transações do tipo Receita.")
{
}
