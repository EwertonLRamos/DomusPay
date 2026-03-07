export interface Pessoa {
    id: number;
    nome: string;
    idade: number;
}

export interface Categoria {
    id: number;
    descricao: string;
    finalidade: number; // 0 = Despesa, 1 = Receita, 2 = Ambas
}

export interface ListagemComValoresTotais<T> {
    itens: T[];
    totalGeralReceitas: number;
    totalGeralDespesas: number;
    saldoGeral: number;
}