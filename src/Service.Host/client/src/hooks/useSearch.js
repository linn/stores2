import useGet from './useGet';

function useSearch(
    url,
    idFieldKey = 'id',
    nameFieldKey = 'name',
    descriptionFieldKey = 'description',
    requiresAuth = false,
    itemsArray = false
) {
    const { send, isLoading, result, clearData } = useGet(url, requiresAuth);

    // some old lookups like employees have stupid items array
    const results = itemsArray
        ? (result?.items.map(s => ({
              ...s,
              id: s[idFieldKey],
              name: s[nameFieldKey].toString(),
              description: s[descriptionFieldKey] != null ? s[descriptionFieldKey].toString() : ''
          })) ?? [])
        : (result?.map(s => ({
              ...s,
              id: s[idFieldKey],
              name: s[nameFieldKey].toString(),
              description: s[descriptionFieldKey] != null ? s[descriptionFieldKey].toString() : ''
          })) ?? []);
    const search = searchTerm => send(null, `?searchTerm=${searchTerm}`);
    const loading = isLoading;
    return { search, results, loading, clear: clearData };
}

export default useSearch;
