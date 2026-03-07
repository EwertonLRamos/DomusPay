import React, { useEffect, useState } from 'react';
import { pessoaService } from '../services/pessoaService';
import type { Pessoa, ListagemComValoresTotais } from '../types';

export const ListaPessoas: React.FC = () => {
    const [dados, setDados] = useState<ListagemComValoresTotais<Pessoa> | null>(null);
    const [loading, setLoading] = useState(true);

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

    if (loading) return <p>A carregar...</p>;
    if (!dados) return null;

    return (
        <div>
            <h2>Controlo de Pessoas</h2>
            <ul>
                {dados.itens.map(item => (
                    <li key={item.id}>
                        {item.nome} ({item.idade} anos)
                    </li>
                ))}
            </ul>

            <div style={{ marginTop: '20px', borderTop: '1px solid #ccc' }}>
                <h3>Resumo Global</h3>
                <p>Total Receitas: {dados.totalGeralReceitas} €</p>
                <p>Total Despesas: {dados.totalGeralDespesas} €</p>
                <p><strong>Saldo Líquido: {dados.saldoGeral} €</strong></p>
            </div>
        </div>
    );
};