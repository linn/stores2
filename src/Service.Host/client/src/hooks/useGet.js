import { useState } from 'react';
import { useAuth } from 'react-oidc-context';

function useGet(url, requiresAuth = false) {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState(null);
    const [result, setResult] = useState(null);

    let token = '';

    const auth = useAuth();
    if (requiresAuth) {
        token = auth.user?.access_token;
    }
    const clearData = () => setResult(null);

    const send = async (id, queryString) => {
        setIsLoading(true);
        setResult(null);
        setErrorMessage(null);
        const headers = {
            accept: 'application/json'
        };
        const requestParameters = {
            method: 'GET',
            headers: requiresAuth ? { ...headers, Authorization: `Bearer ${token}` } : headers
        };
        const response = await fetch(
            id ? `${url}/${id}${queryString ?? ''}` : `${url}${queryString ?? ''}`,
            requestParameters
        );
        if (response.ok) {
            setResult(await response.json());
            setIsLoading(false);
        } else {
            const text = await response.text();
            setErrorMessage(text);
            setIsLoading(false);
        }
    };

    return { send, isLoading, errorMessage, result, clearData };
}

export default useGet;
