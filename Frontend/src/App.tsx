import React, { useState } from 'react';
import { ListaPessoas } from './components/ListaPessoas';
import { ListaCategorias } from './components/ListaCategorias';
import { Transacoes } from './components/Transacoes';
import logo from './assets/domuspay-logo.png';

const App: React.FC = () => {
  const [telaAtiva, setTelaAtiva] = useState<'pessoas' | 'categorias' | 'transacoes'>('transacoes');

  return (
    <div className="layout-container">
      <nav className="navbar">
        <div className="logo">
          <img src={logo} alt="DomusPay" className="logo-img" /> 
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
            className={telaAtiva === 'transacoes' ? 'ativo' : ''} 
            onClick={() => setTelaAtiva('transacoes')}
          >
            Transações
          </button>
        </div>
      </nav>

      <main className="conteudo-principal">
        {telaAtiva === 'pessoas' && <ListaPessoas />}
        {telaAtiva === 'categorias' && <ListaCategorias />}
        {telaAtiva === 'transacoes' && <Transacoes />}
      </main>
    </div>
  );
};

export default App;