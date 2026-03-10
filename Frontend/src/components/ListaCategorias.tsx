import React, { useEffect, useState } from 'react';
import { categoriaService } from '../services/categoriaService';
import type { ListagemComValoresTotais, ItemListagemCategoria } from '../types';

export const ListaCategorias: React.FC = () => {
    const [dados, setDados] = useState<ListagemComValoresTotais<ItemListagemCategoria> | null>(null);
    const [loading, setLoading] = useState(true);

    const [modalAtivo, setModalAtivo] = useState<'nenhum' | 'cadastrar' | 'detalhes'>('nenhum');
    const [itemSelecionado, setItemSelecionado] = useState<ItemListagemCategoria | null>(null);

    const [editDescricao, setEditDescricao] = useState('');
    const [editFinalidade, setEditFinalidade] = useState('Despesa'); 

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

    const obterFinalidadeTexto = (valor: number | string) => {
        if (valor === 1 || valor === 'Despesa') return 'Despesa';
        if (valor === 2 || valor === 'Receita') return 'Receita';
        if (valor === 3 || valor === 'Ambas') return 'Ambas';
        return 'Desconhecido';
    };

    const formatarMoeda = (valor: number) => {
        if (isNaN(valor)) return 'R$ 0,00';
        return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);
    };

    const abrirModalNovo = () => {
        setItemSelecionado(null);
        setEditDescricao('');
        setEditFinalidade('Despesa');
        setModalAtivo('cadastrar');
    };

    const abrirModal = (tipo: 'detalhes', item: ItemListagemCategoria) => {
        setItemSelecionado(item);
        setModalAtivo(tipo);
    };

    const fecharModal = () => {
        setModalAtivo('nenhum');
        setItemSelecionado(null);
    };

    const handleCadastrar = async (e: React.FormEvent) => {
        e.preventDefault();
        if (editDescricao.trim() === '') return;
        
        try {
            await categoriaService.criar({
                descricao: editDescricao,
                finalidade: editFinalidade
            });
            fecharModal();
            carregarCategorias();
        } catch (error) {
            alert('Erro ao cadastrar a categoria.');
        }
    };

    if (loading) return <div className="loading">A carregar os dados...</div>;
    if (!dados) return <div className="erro">Nenhum dado encontrado.</div>;

    return (
        <div className="card-modulo">
            <div className="cabecalho-modulo">
                <h2>Controle de Categorias</h2>
                <button className="btn-sucesso" onClick={abrirModalNovo}>Cadastrar Categoria</button>
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
                    {dados.itens.map((linha) => (
                        <tr key={linha.id}>
                            <td><strong>{linha.descricao}</strong></td>
                            <td>
                                <span className={
                                    linha.finalidade === "Despesa" ? "texto-vermelho" : 
                                    linha.finalidade === "Receita" ? "texto-verde" : 
                                    linha.finalidade === "Ambas" ? "texto-amarelo" : ""
                                }>
                                    {obterFinalidadeTexto(linha.finalidade)}
                                </span>
                            </td>
                            <td className={linha.totalReceitas === 0 ? "" : "texto-verde"}>{formatarMoeda(linha.totalReceitas)}</td>
                            <td className={linha.totalDespesas === 0 ? "" : "texto-vermelho"}>{formatarMoeda(linha.totalDespesas)}</td>
                            <td className={
                                linha.saldo > 0 ? "texto-verde" : 
                                linha.saldo < 0 ? "texto-vermelho" : 
                                linha.saldo == 0 ? "texto-azul" : 
                                ""}>
                                <strong>{formatarMoeda(linha.saldo)}</strong>
                            </td>
                            <td className="acoes">
                                <button className="btn-icone detalhe" title="Detalhes" onClick={() => abrirModal('detalhes', linha)}>👁️</button>
                            </td>
                        </tr>
                    ))}
                    {dados.itens.length === 0 && (
                        <tr><td colSpan={3} style={{ textAlign: 'center', padding: '30px' }}>Nenhuma categoria cadastrada.</td></tr>
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
                    <span>Saldo Total</span>
                    <h3 className={
                        dados.saldoTotal > 0 ? "texto-verde" : 
                        dados.saldoTotal < 0 ? "texto-vermelho" : 
                        dados.saldoTotal == 0 ? "texto-azul" : 
                        ""}>
                        {formatarMoeda(dados.saldoTotal)}
                    </h3>
                </div>
            </div>

            {modalAtivo !== 'nenhum' && (
                <div className="modal-overlay" onClick={fecharModal}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>

                        {/* Cadastro */}
                        {modalAtivo === 'cadastrar' && ( 
                            <form onSubmit={handleCadastrar}>
                                <div className="modal-header">
                                    <h2>Nova Categoria</h2>
                                    <button type="button" className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <div className="form-group">
                                        <label>Descrição</label>
                                        <input 
                                            type="text" 
                                            value={editDescricao} 
                                            onChange={(e) => setEditDescricao(e.target.value)} 
                                            placeholder="Ex: Supermercado"
                                            required 
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label>Finalidade</label>
                                        <select 
                                            value={editFinalidade} 
                                            onChange={(e) => setEditFinalidade(e.target.value)}
                                            style={{ width: '100%', padding: '12px', borderRadius: '6px', border: '1px solid #d1d5db' }}
                                            required
                                        >
                                            <option value="Despesa">Despesa</option>
                                            <option value="Receita">Receita</option>
                                            <option value="Ambas">Ambas</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn-cancelar" onClick={fecharModal}>Cancelar</button>
                                    <button type="submit" className="btn-salvar">Salvar</button>
                                </div>
                            </form>
                        )}

                        {/* Detalhes */}
                        {modalAtivo === 'detalhes' && itemSelecionado && (
                            <>
                                <div className="modal-header">
                                    <h2>{itemSelecionado.descricao}</h2>
                                    <button className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <p><strong>Finalidade:</strong> {itemSelecionado.finalidade}</p>
                                    <p><strong>Total de Receitas:</strong> <span className="texto-verde">{formatarMoeda(itemSelecionado.totalReceitas)}</span></p>
                                    <p><strong>Total de Despesas:</strong> <span className="texto-vermelho">{formatarMoeda(itemSelecionado.totalDespesas)}</span></p>
                                    <hr style={{ margin: '15px 0', border: '1px solid #eee' }} />
                                    <p style={{ fontSize: '18px' }}>
                                        <strong>Saldo Atual:</strong> <span className={
                                            itemSelecionado.saldo > 0 ? "texto-verde" : 
                                            itemSelecionado.saldo < 0 ? "texto-vermelho" : 
                                            itemSelecionado.saldo == 0 ? "texto-azul" : 
                                            ""} >
                                            {formatarMoeda(itemSelecionado.saldo)}
                                        </span>
                                    </p>
                                </div>
                                <div className="modal-footer">
                                    <button className="btn-cancelar" onClick={fecharModal}>Fechar</button>
                                </div>
                            </>
                        )}

                    </div>
                </div>
            )}
        </div>
    );
};