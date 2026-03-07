import api from './api';
import type { Pessoa, ItemListagemPessoa, ListagemComValoresTotais } from '../types';

export const pessoaService = {
    listarTodas: async () => {
        const response = await api.get<ListagemComValoresTotais<ItemListagemPessoa>>('/Pessoa');
        return response.data;
    },

    criar: async (dados: Omit<Pessoa, 'id'>) => {
        const response = await api.post<Pessoa>('/Pessoa', dados);
        return response.data;
    },

    eliminar: async (id: number) => {
        await api.delete(`/Pessoa/${id}`);
    }
};