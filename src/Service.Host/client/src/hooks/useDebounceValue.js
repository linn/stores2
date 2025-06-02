import { useEffect, useState } from 'react';

// Enhanced hook: returns both the debounced value and an isDebouncing flag
function useDebounceValue(value, delay = 1000) {
    const [debouncedValue, setDebouncedValue] = useState(value);
    const [isDebouncing, setIsDebouncing] = useState(false);

    useEffect(() => {
        setIsDebouncing(true);
        const handler = setTimeout(() => {
            setDebouncedValue(value);
            setIsDebouncing(false);
        }, delay);

        return () => {
            clearTimeout(handler);
        };
    }, [value, delay]);

    return [debouncedValue, isDebouncing];
}

export default useDebounceValue;
