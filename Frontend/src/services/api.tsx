import axios from 'axios';
import { toast } from 'react-toastify';
import { ErrorToast } from '../components/ErrorToast';

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
            const errorMessage = error.response.data.detail || 'Ocorreu um erro inesperado ao conectar com o servidor.';
            const errorTitle = error.response.data.title || 'Erro de Conexão'; 
            
            toast.error(({ closeToast }) => (
                <ErrorToast
                    title={errorTitle}
                    message={errorMessage}
                    closeToast={closeToast}
                />
            ));
            console.error('Erro na API:', errorMessage);
        }
        return Promise.reject(error);
    }
);

export default api;