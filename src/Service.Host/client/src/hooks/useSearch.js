import useGet from './useGet';

function useSearch(
    url,
    idFieldKey = 'id',
    nameFieldKey = 'name',
    descriptionFieldKey = 'description',
    requiresAuth = false
) {
    const { send, isLoading, result, clearData } = useGet(url, requiresAuth);

    const results =
        result?.map(s => ({
            ...s,
            id: s[idFieldKey],
            name: s[nameFieldKey],
            description: s[descriptionFieldKey]
        })) ?? [];
    const search = searchTerm => send(null, `?searchTerm=${searchTerm}`);
    const loading = isLoading;
    const clear = clearData;
    return { search, results, loading, clear };
}

export default useSearch;
