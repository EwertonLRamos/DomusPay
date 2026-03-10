import React, { useEffect, useState } from 'react';
import { pessoaService } from '../services/pessoaService';
import type { ListagemComValoresTotais, ItemListagemPessoa } from '../types';

export const ListaPessoas: React.FC = () => {
    const [dados, setDados] = useState<ListagemComValoresTotais<ItemListagemPessoa> | null>(null);
    const [loading, setLoading] = useState(true);

    const [modalAtivo, setModalAtivo] = useState<'nenhum' | 'cadastrar' | 'detalhes' | 'editar' | 'excluir'>('nenhum');
    const [itemSelecionado, setItemSelecionado] = useState<ItemListagemPessoa | null>(null);

    const [editNome, setEditNome] = useState('');
    const [editIdade, setEditIdade] = useState<number | ''>('');

    useEffect(() => {
        carregarPessoas();
    }, []);

    const carregarPessoas = async () => {
        try {
            setLoading(true);
            const resultado = await pessoaService.listarTodas();
            setDados(resultado);
        } catch (error) {
            alert('Não foi possível carregar a lista de pessoas.');
        } finally {
            setLoading(false);
        }
    };

    const formatarMoeda = (valor: number) => {
        if (isNaN(valor)) return 'R$ 0,00';
        return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);
    };

    const abrirModalNovo = () => {
        setItemSelecionado(null);
        setEditNome('');
        setEditIdade('');
        setModalAtivo('cadastrar');
    };

    const abrirModal = (tipo: 'detalhes' | 'editar' | 'excluir', item: ItemListagemPessoa) => {
        setItemSelecionado(item);
        if (tipo === 'editar') {
            setEditNome(item.nome);
            setEditIdade(item.idade);
        }
        setModalAtivo(tipo);
    };

    const fecharModal = () => {
        setModalAtivo('nenhum');
        setItemSelecionado(null);
    };

    const handleExcluir = async () => {
        if (!itemSelecionado) return;
        try {
            await pessoaService.eliminar(itemSelecionado.id);
            fecharModal();
            carregarPessoas(); 
        } catch (error) {
            alert('Erro ao excluir a pessoa. Verifique se existem transações pendentes.');
        }
    };

    const handleSalvarEdicao = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!itemSelecionado || editNome.trim() === '' || editIdade === '') return;
        
        try {
            await pessoaService.atualizar(itemSelecionado.id, {
                id: itemSelecionado.id,
                nome: editNome,
                idade: Number(editIdade)
            });
            fecharModal();
            carregarPessoas(); 
        } catch (error) {
            alert('Erro ao atualizar os dados.');
        }
    };

    const handleCadastrar = async (e: React.FormEvent) => {
        e.preventDefault();
        if (editNome.trim() === '' || editIdade === '') return;
        
        try {
            await pessoaService.criar({
                nome: editNome,
                idade: Number(editIdade)
            });
            fecharModal();
            carregarPessoas();
        } catch (error) {
            alert('Erro ao cadastrar a pessoa.');
        }
    };

    if (loading) return <div className="loading">A carregar os dados...</div>;
    if (!dados) return <div className="erro">Nenhum dado encontrado.</div>;

    return (
        <div className="card-modulo">
            <div className="cabecalho-modulo">
                <h2>Controle de Pessoas</h2>
                <button className="btn-sucesso" onClick={abrirModalNovo}>Cadastrar Pessoa</button>
            </div>

            <table className="tabela-dados">
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>Idade</th>
                        <th>Receitas</th>
                        <th>Despesas</th>
                        <th>Saldo</th>
                        <th style={{ textAlign: 'center' }}>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {dados.itens.map((linha) => (
                        <tr key={linha.id}>
                            <td><strong>{linha.nome}</strong></td>
                            <td>{linha.idade} anos</td>
                            <td className="texto-verde">{formatarMoeda(linha.totalReceitas)}</td>
                            <td className="texto-vermelho">{formatarMoeda(linha.totalDespesas)}</td>
                            <td className={linha.saldo >= 0 ? "texto-verde" : "texto-vermelho"}>
                                <strong>{formatarMoeda(linha.saldo)}</strong>
                            </td>
                            <td className="acoes">
                                <button className="btn-icone detalhe" title="Detalhes" onClick={() => abrirModal('detalhes', linha)}>👁️</button>
                                <button className="btn-icone editar" title="Editar" onClick={() => abrirModal('editar', linha)}>✏️</button>
                                <button className="btn-icone deletar" title="Excluir" onClick={() => abrirModal('excluir', linha)}>🗑️</button>
                            </td>
                        </tr>
                    ))}
                    {dados.itens.length === 0 && (
                        <tr><td colSpan={6} style={{ textAlign: 'center', padding: '30px' }}>Nenhuma pessoa cadastrada.</td></tr>
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
                    <h3 className={dados.saldoTotal >= 0 ? "texto-verde" : "texto-vermelho"}>
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
                                    <h2>Cadastrar Nova Pessoa</h2>
                                    <button type="button" className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <div className="form-group">
                                        <label>Nome Completo</label>
                                        <input 
                                            type="text" 
                                            value={editNome} 
                                            onChange={(e) => setEditNome(e.target.value)} 
                                            placeholder="Ex: Ana Silva"
                                            required 
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label>Idade</label>
                                        <input 
                                            type="number" 
                                            min="0"
                                            value={editIdade} 
                                            onChange={(e) => setEditIdade(e.target.value !== '' ? Number(e.target.value) : '')} 
                                            placeholder="Ex: 25"
                                            required 
                                        />
                                    </div>
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn-cancelar" onClick={fecharModal}>Cancelar</button>
                                    <button type="submit" className="btn-sucesso">Salvar</button>
                                </div>
                            </form>
                        )}

                        {/* Detalhes */}
                        {modalAtivo === 'detalhes' && itemSelecionado && (
                            <>
                                <div className="modal-header">
                                    <h2>{itemSelecionado.nome}</h2>
                                    <button className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <p><strong>Idade:</strong> {itemSelecionado.idade} anos</p>
                                    <p><strong>Total de Receitas:</strong> <span className="texto-verde">{formatarMoeda(itemSelecionado.totalReceitas)}</span></p>
                                    <p><strong>Total de Despesas:</strong> <span className="texto-vermelho">{formatarMoeda(itemSelecionado.totalDespesas)}</span></p>
                                    <hr style={{ margin: '15px 0', border: '1px solid #eee' }} />
                                    <p style={{ fontSize: '18px' }}>
                                        <strong>Saldo Atual:</strong> <span className={itemSelecionado.saldo >= 0 ? "texto-verde" : "texto-vermelho"}>{formatarMoeda(itemSelecionado.saldo)}</span>
                                    </p>
                                </div>
                                <div className="modal-footer">
                                    <button className="btn-cancelar" onClick={fecharModal}>Fechar</button>
                                </div>
                            </>
                        )}

                        {/* Exclusão */}
                        {modalAtivo === 'excluir' && itemSelecionado && (
                            <>
                                <div className="modal-header">
                                    <h2>Confirmar Exclusão</h2>
                                    <button className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <p>Tem certeza que deseja excluir permanentemente <strong>{itemSelecionado.nome}</strong>?</p>
                                    <p style={{ color: '#ef4444', fontSize: '14px', marginTop: '10px' }}>
                                        Atenção: Se essa pessoa possuir transações vinculadas, a exclusão apagará o histórico em cascata.
                                    </p>
                                </div>
                                <div className="modal-footer">
                                    <button className="btn-cancelar" onClick={fecharModal}>Cancelar</button>
                                    <button className="btn-perigo" onClick={handleExcluir}>Sim, Excluir</button>
                                </div>
                            </>
                        )}

                        {/* Edição */}
                        {modalAtivo === 'editar' && itemSelecionado && (
                            <form onSubmit={handleSalvarEdicao}>
                                <div className="modal-header">
                                    <h2>Editar Pessoa</h2>
                                    <button type="button" className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <div className="form-group">
                                        <label>Nome Completo</label>
                                        <input 
                                            type="text" 
                                            value={editNome} 
                                            onChange={(e) => setEditNome(e.target.value)} 
                                            required 
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label>Idade</label>
                                        <input 
                                            type="number" 
                                            min="0"
                                            value={editIdade} 
                                            onChange={(e) => setEditIdade(e.target.value !== '' ? Number(e.target.value) : '')} 
                                            required 
                                        />
                                    </div>
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn-cancelar" onClick={fecharModal}>Cancelar</button>
                                    <button type="submit" className="btn-salvar">Salvar Alterações</button>
                                </div>
                            </form>
                        )}

                    </div>
                </div>
            )}
        </div>
    );
};