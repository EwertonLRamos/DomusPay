import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5235/api', 
    headers: {
        'Content-Type': 'application/json',
    }
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.data) {
            const errorMessage = error.response.data.detail || 'Ocorreu um erro inesperado.';
            console.error('Erro na API:', errorMessage);
            
            // toast.error(errorMessage);
        }
        return Promise.reject(error);
    }
);

export default api;