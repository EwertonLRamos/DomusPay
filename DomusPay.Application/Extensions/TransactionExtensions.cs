using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;

namespace DomusPay.Application.Extensions;

public static class TransacaoExtensions
{
    public static decimal CalcularValorTotal(this IEnumerable<Transacao> transacoes, TipoTransacao tipoTransacao)
     => transacoes
            .Where(t => t.Tipo == tipoTransacao)
            .Sum(t => t.Valor > 0 ? t.Valor : 0);
}
