import React, { useEffect, useState } from 'react';
import { transacaoService } from '../services/transacaoService';
import { pessoaService } from '../services/pessoaService';
import { categoriaService } from '../services/categoriaService';
import type { 
    ItemListagemTransacao, 
    ListagemBase, 
    ItemListagemPessoa,
    ItemListagemCategoria
} from '../types';

export const Transacoes: React.FC = () => {
    const [dados, setDados] = useState<ListagemBase<ItemListagemTransacao> | null>(null);
    const [loading, setLoading] = useState(true);

    const [modalAtivo, setModalAtivo] = useState<'nenhum' | 'cadastrar' | 'detalhes'>('nenhum');
    const [itemSelecionado, setItemSelecionado] = useState<ItemListagemTransacao | null>(null);

    const [pessoas, setPessoas] = useState<ItemListagemPessoa[]>([]);
    const [categorias, setCategorias] = useState<ItemListagemCategoria[]>([]);

    const [editPessoaId, setEditPessoaId] = useState('');
    const [editCategoriaId, setEditCategoriaId] = useState('');

    const [editValorDisplay, setEditValorDisplay] = useState('');

    const [editDescricao, setEditDescricao] = useState('');
    const [editTipo, setEditTipo] = useState('Despesa'); 

    useEffect(() => {
        carregarCategorias();
        carregarListasParaDropdown();
    }, []);

    const carregarListasParaDropdown = async () => {
        const resPessoas = await pessoaService.listarTodas();
        setPessoas(resPessoas.itens || []);
        
        const resCategorias = await categoriaService.listarTodas();
        setCategorias(resCategorias.itens || []);
    };
    
    const handleValorChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let valor = e.target.value.replace(/\D/g, ''); 
        
        if (!valor) {
            setEditValorDisplay('');
            return;
        }

        const numericValue = (parseInt(valor, 10) / 100).toFixed(2);
        
        const formatado = new Intl.NumberFormat('pt-BR', { 
            style: 'currency', 
            currency: 'BRL' 
        }).format(parseFloat(numericValue));

        setEditValorDisplay(formatado);
    };

    const obterValorNumerico = (valorFormatado: string): number => {
        const apenasNumeros = valorFormatado.replace(/[^\d]/g, '');
        return parseInt(apenasNumeros, 10) / 100;
    };

    const carregarCategorias = async () => {
        try {
            setLoading(true);
            const resultado = await transacaoService.listarTodas();
            setDados(resultado);
        } 
        finally {
            setLoading(false);
        }
    };

    const obterTipoTexto = (valor: number | string) => {
        if (valor === 1 || valor === 'Despesa') return 'Despesa';
        if (valor === 2 || valor === 'Receita') return 'Receita';
        return 'Desconhecido';
    };

    const formatarMoeda = (valor: number) => {
        if (isNaN(valor)) return 'R$ 0,00';
        return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);
    };

    const abrirModalNovo = () => {
        setItemSelecionado(null);
        setEditDescricao('');
        setEditTipo('Despesa');
        setModalAtivo('cadastrar');
    };

    const fecharModal = () => {
        setModalAtivo('nenhum');
        setItemSelecionado(null);
    };

    const handleCadastrar = async (e: React.FormEvent) => {
        e.preventDefault();
        if (editDescricao.trim() === '') return;
        
        await transacaoService.criar({
            descricao: editDescricao,
            tipo: editTipo,
            pessoaId: editPessoaId,
            categoriaId: editCategoriaId,
            valor: obterValorNumerico(editValorDisplay)
        });
        fecharModal();
        carregarCategorias();
    };

    if (loading) return <div className="loading">A carregar os dados...</div>;
    if (!dados) return <div className="erro">Nenhum dado encontrado.</div>;

    return (
        <div className="card-modulo">
            <div className="cabecalho-modulo">
                <h2>Controle de Transações</h2>
                <button className="btn-cadastro" onClick={abrirModalNovo}>Cadastrar Transação</button>
            </div>

            <table className="tabela-dados">
                <thead>
                    <tr>
                        <th>Descrição</th>
                        <th>Tipo</th>
                        <th>Responsável</th>
                        <th>Categoria</th>
                        <th>Valor</th>
                    </tr>
                </thead>
                <tbody>
                    {dados.itens.map((linha) => (
                        <tr key={linha.id} className={
                                    linha.tipo === "Despesa" ? "linha-despesa" : 
                                    linha.tipo === "Receita" ? "linha-receita" : ""
                                }>
                            <td><strong>{linha.descricao}</strong></td>
                            <td>
                                <span className={
                                    linha.tipo === "Despesa" ? "texto-vermelho" : 
                                    linha.tipo === "Receita" ? "texto-verde" : ""
                                }>
                                    {obterTipoTexto(linha.tipo)}
                                </span>
                            </td>
                            <td><strong>{linha.nomePessoa}</strong></td>
                            <td><strong>{linha.finalidadeCategoria}</strong></td>
                            <td>
                                <strong className={
                                    linha.tipo === "Despesa" ? "texto-vermelho" : 
                                    linha.tipo === "Receita" ? "texto-verde" : ""
                                }>
                                    {formatarMoeda(linha.valor)}
                                </strong>
                            </td>
                        </tr>
                    ))}
                    {dados.itens.length === 0 && (
                        <tr><td colSpan={3} style={{ textAlign: 'center', padding: '30px' }}>Nenhuma categoria cadastrada.</td></tr>
                    )}
                </tbody>
            </table>

            {modalAtivo !== 'nenhum' && (
                <div className="modal-overlay" onClick={fecharModal}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>

                        {/* Cadastro */}
                        {modalAtivo === 'cadastrar' && ( 
                            <form onSubmit={handleCadastrar}>
                                <div className="modal-header">
                                    <h2>Nova Transação</h2>
                                    <button type="button" className="btn-fechar" onClick={fecharModal}>&times;</button>
                                </div>
                                <div className="modal-body">
                                    <div className="form-group">
                                        <label>Descrição</label>
                                        <input 
                                            type="text" 
                                            value={editDescricao} 
                                            onChange={(e) => setEditDescricao(e.target.value)} 
                                            placeholder="Ex: Lanches das crianças"
                                            required 
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label>Finalidade</label>
                                        <select 
                                            value={editTipo} 
                                            onChange={(e) => setEditTipo(e.target.value)}
                                            required
                                        >
                                            <option value="Despesa">Despesa</option>
                                            <option value="Receita">Receita</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="form-group">
                                    <label>Pessoa</label>
                                    <select 
                                        value={editPessoaId} 
                                        onChange={(e) => setEditPessoaId(e.target.value)}
                                        required
                                    >
                                        <option value="" disabled>Selecione uma pessoa...</option>
                                        {pessoas.map((p) => (
                                            <option key={p.id} value={p.id}>
                                                {p.nome}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Categoria</label>
                                    <select 
                                        value={editCategoriaId} 
                                        onChange={(e) => setEditCategoriaId(e.target.value)}
                                        required
                                    >
                                        <option value="" disabled>Selecione uma categoria...</option>
                                        {categorias.map((c) => (
                                            <option key={c.id} value={c.id}>
                                                {c.descricao}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Valor</label>
                                    <input 
                                        type="text" 
                                        value={editValorDisplay} 
                                        onChange={handleValorChange} 
                                        placeholder="R$ 0,00"
                                        required 
                                    />
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn-cancelar" onClick={fecharModal}>Cancelar</button>
                                    <button type="submit" className="btn-salvar">Salvar</button>
                                </div>
                            </form>
                        )}

                    </div>
                </div>
            )}
        </div>
    );
};