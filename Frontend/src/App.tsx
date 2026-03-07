import React, { useState } from 'react';
import { ListaPessoas } from './components/ListaPessoas';
import './App.css'; // Vamos usar para uns estilos básicos
import { ListaCategorias } from './components/ListaCategorias';

const App: React.FC = () => {
  const [telaAtiva, setTelaAtiva] = useState<'pessoas' | 'categorias' | 'transacoes'>('pessoas');

  return (
    <div className="layout-container">
      {/* Menu Superior / Navegação */}
      <nav className="navbar">
        <div className="logo">
          <h2>DomusPay</h2>
        </div>
        <div className="menu-botoes">
          <button 
            className={telaAtiva === 'pessoas' ? 'ativo' : ''} 
            onClick={() => setTelaAtiva('pessoas')}
          >
            Pessoas
          </button>
          <button 
            className={telaAtiva === 'categorias' ? 'ativo' : ''} 
            onClick={() => setTelaAtiva('categorias')}
          >
            Categorias
          </button>
          <button 
            className="btn-destaque" 
            onClick={() => setTelaAtiva('transacoes')}
          >
            Nova Transação
          </button>
        </div>
      </nav>

      {/* Conteúdo Dinâmico */}
      <main className="conteudo-principal">
        {telaAtiva === 'pessoas' && <ListaPessoas />}
        {telaAtiva === 'categorias' && <ListaCategorias />}
        {telaAtiva === 'transacoes' && <div><h2>Nova Transação</h2><p>Formulário em construção...</p></div>}
      </main>
    </div>
  );
};

export default App;