import { useState, useRef } from 'react';
import { utilities } from '@linn-it/linn-form-components-library';
import { useAuth } from 'react-oidc-context';
import { useNavigate } from 'react-router-dom';

function usePost(url, requiresAuth = false, redirectOnSuccess = false) {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState(null);
    const [postResult, setPostResult] = useState(null);

    const navigate = useNavigate();
    const abortControllerRef = useRef(null); // Store AbortController

    let token = '';
    const auth = useAuth();
    if (requiresAuth) {
        token = auth.user?.access_token;
    }

    const clearPostResult = () => {
        setPostResult(null);
        setErrorMessage(null);
    };

    const send = async (id, data) => {
        // Cancel any ongoing request before starting a new one
        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }

        const controller = new AbortController(); // Create a new controller
        abortControllerRef.current = controller;

        setIsLoading(true);
        setPostResult(null);
        setErrorMessage(null);

        const headers = {
            accept: 'application/json',
            'Content-Type': 'application/json'
        };

        const requestParameters = {
            method: 'POST',
            body: JSON.stringify(data),
            headers: requiresAuth ? { ...headers, Authorization: `Bearer ${token}` } : headers,
            signal: controller.signal // Attach the abort signal
        };

        try {
            const response = await fetch(id ? `${url}/${id}` : url, requestParameters);

            if (response.ok) {
                const result = await response.json();
                setPostResult(result);
                if (redirectOnSuccess) {
                    navigate(utilities.getSelfHref(result));
                }
            } else {
                const text = await response.text();
                setErrorMessage(text);
            }
        } catch (error) {
            if (error.name === 'AbortError') {
                console.log('Request was aborted.');
            } else {
                setErrorMessage('An error occurred.');
            }
        } finally {
            setIsLoading(false);
        }
    };

    const cancelRequest = () => {
        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }
    };

    return { send, cancelRequest, isLoading, errorMessage, postResult, clearPostResult };
}

export default usePost;
