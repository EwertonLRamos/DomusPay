export interface Pessoa {
    id: string;
    nome: string;
    idade: number;
}

export interface Categoria {
    id: number;
    descricao: string;
    finalidade: number; // 0 = Despesa, 1 = Receita, 2 = Ambas
}

export interface ItemListagemPessoa {
    id: string;
    nome: string;
    idade: number;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface ItemListagemCategoria {
    id: string;
    descricao: string;
    finalidade: number;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface ListagemComValoresTotais<T> {
    itens: T[];
    totalReceitas: number;
    totalDespesas: number;
    saldoTotal: number;
}