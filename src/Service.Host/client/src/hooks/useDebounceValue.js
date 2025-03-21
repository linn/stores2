import { useEffect, useState } from 'react';

// This hook returns debounced updates to a passed input value
// The returned value is only updated when the input value has been changed and the specified delay time has elapsed#
function useDebounceValue(value, delay = 1000) {
    const [debouncedValue, setDebouncedValue] = useState(value);

    useEffect(() => {
        const handler = setTimeout(() => {
            setDebouncedValue(value);
        }, delay);

        return () => clearTimeout(handler);
    }, [value, delay]);

    return debouncedValue;
}

export default useDebounceValue;
