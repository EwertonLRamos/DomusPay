import api from './api';
import type { Transacao, ItemListagemTransacao, ListagemComValoresTotais } from '../types';

export const transacaoService = {
    listarTodas: async () => {
        const response = await api.get<ListagemComValoresTotais<ItemListagemTransacao>>('/Transacao');
        return response.data;
    },

    criar: async (dados: Omit<Transacao, 'id'>) => {
        const response = await api.post<Transacao>('/Transacao', dados);
        return response.data;
    },
};