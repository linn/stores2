import useGet from './useGet';

function useSearch(url, requiresAuth = false) {
    const { send, isLoading, result, clearData } = useGet(url, requiresAuth);

    const results = result;
    const search = searchTerm => send(null, `searchTerm=${searchTerm}`);
    const loading = isLoading;
    const clear = clearData;
    return { search, results, loading, clear };
}

export default useSearch;
