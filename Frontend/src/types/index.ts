export interface Pessoa {
    id: string;
    nome: string;
    idade: number;
}

export interface Categoria {
    id: number;
    descricao: string;
    finalidade: string; // "Despesa", "Receita" ou "Ambas"
}

export interface Transacao {
    id: string;
    descricao: string;
    valor: number;
    tipo: string; // "Despesa" ou "Receita"
    categoriaId: string;
    pessoaId: string;
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
    finalidade: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface ItemListagemTransacao {
    id: string;
    descricao: string;
    valor: number;
    tipo: string;
    finalidadeCategoria: string;
    nomePessoa: string;

}

export interface ListagemComValoresTotais<T> {
    itens: T[];
    totalReceitas: number;
    totalDespesas: number;
    saldoTotal: number;
}

export interface ListagemBase<T> {
    itens: T[];
}