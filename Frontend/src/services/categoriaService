import api from './api';
import type { Categoria, ItemListagemCategoria, ListagemComValoresTotais } from '../types';

export const categoriaService = {
    listarTodas: async () => {
        const response = await api.get<ListagemComValoresTotais<ItemListagemCategoria>>('/Categoria');
        return response.data;
    },

    criar: async (dados: Omit<Categoria, 'id'>) => {
        const response = await api.post<Categoria>('/Categoria', dados);
        return response.data;
    },
};