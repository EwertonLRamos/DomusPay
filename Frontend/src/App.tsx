import React from 'react';
import { ListaPessoas } from './components/ListaPessoas'; 

const App: React.FC = () => {
  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif', maxWidth: '800px', margin: '0 auto' }}>
      <header style={{ borderBottom: '2px solid #eee', marginBottom: '20px' }}>
        <h1>DomusPay</h1>
        <p>Controle de Gastos Residenciais</p>
      </header>
      
      <main>
        {/* Aqui estamos injetando o componente que vai buscar os dados na sua API */}
        <ListaPessoas />
      </main>
    </div>
  );
};

export default App;