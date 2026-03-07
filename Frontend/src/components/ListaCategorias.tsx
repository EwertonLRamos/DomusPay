import React, { useEffect, useState } from 'react';
import { categoriaService } from '../services/categoriaService';
import type { ItemListagemCategoria, ListagemComValoresTotais } from '../types';

export const ListaCategorias: React.FC = () => {
    const [dados, setDados] = useState<ListagemComValoresTotais<ItemListagemCategoria> | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        carregarCategorias();
    }, []);

    const carregarCategorias = async () => {
        try {
            setLoading(true);
            const resultado = await categoriaService.listarTodas();
            setDados(resultado);
        } catch (error) {
            alert('Não foi possível carregar a lista de categorias.');
        } finally {
            setLoading(false);
        }
    };

    const formatarMoeda = (valor: number) => {
        if (isNaN(valor)) return 'R$ 0,00';
        return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);
    };

    if (loading) return <div className="loading">A carregar os dados... ⏳</div>;
    if (!dados) return <div className="erro">Nenhum dado encontrado.</div>;

    return (
        <div className="card-modulo">
            <div className="cabecalho-modulo">
                <h2>Controle de Categorias</h2>
                <button className="btn-sucesso">Cadastrar Categoria</button>
            </div>

            <table className="tabela-dados">
                <thead>
                    <tr>
                        <th>Descrição</th>
                        <th>Finalidade</th>
                        <th>Receitas</th>
                        <th>Despesas</th>
                        <th>Saldo</th>
                        <th style={{ textAlign: 'center' }}>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {dados.itens.map(({ id, descricao, finalidade, totalReceitas, totalDespesas, saldo }) => (
                        <tr key={id}>
                            <td><strong>{descricao}</strong></td>
                            <td>{finalidade === 1 ? 'Despesa' : finalidade === 2 ? 'Receita' : 'Ambas'}</td>
                            <td className="texto-verde">{formatarMoeda(totalReceitas)}</td>
                            <td className="texto-vermelho">{formatarMoeda(totalDespesas)}</td>
                            <td className={saldo >= 0 ? "texto-verde" : "texto-vermelho"}>
                                <strong>{formatarMoeda(saldo)}</strong>
                            </td>
                            <td className="acoes">
                                <button className="btn-icone detalhe" title="Detalhes">👁️</button>
                                <button className="btn-icone editar" title="Editar">✏️</button>
                                <button className="btn-icone deletar" title="Excluir">🗑️</button>
                            </td>
                        </tr>
                    ))}
                    {dados.itens.length === 0 && (
                        <tr><td colSpan={6} style={{ textAlign: 'center' }}>Nenhuma categoria cadastrada.</td></tr>
                    )}
                </tbody>
            </table>

            <div className="totais-globais">
                <div className="card-total">
                    <span>Total Receitas</span>
                    <h3 className="texto-verde">{formatarMoeda(dados.totalReceitas)}</h3>
                </div>
                <div className="card-total">
                    <span>Total Despesas</span>
                    <h3 className="texto-vermelho">{formatarMoeda(dados.totalDespesas)}</h3>
                </div>
                <div className="card-total destaque">
                    <span>Saldo</span>
                    <h3 className={dados.saldo >= 0 ? "texto-verde" : "texto-vermelho"}>
                        {formatarMoeda(dados.saldo)}
                    </h3>
                </div>
            </div>
        </div>
    );
};