import React, { useEffect } from 'react';
import Grid from '@mui/material/Grid';
import { Search } from '@linn-it/linn-form-components-library';
import itemTypes from '../../../itemTypes';
import useSearch from '../../../hooks/useSearch';

function AuditLocationSearch({
    auditLocation = null,
    setAuditLocation,
    setAuditLocationDetails,
    disabled = false,
    shouldRender = true
}) {
    const {
        search: searchAuditLocations,
        results: auditLocationsSearchResults,
        loading: auditLocationsSearchLoading,
        clear: clearAuditLocationsSearch
    } = useSearch(itemTypes.auditLocations.url, 'storagePlace', 'storagePlace', 'storagePlace');

    useEffect(() => {
        if (auditLocationsSearchResults?.length) {
            const exactMatch = auditLocationsSearchResults.find(
                loc => loc.storagePlace === auditLocation?.toUpperCase()
            );
            if (exactMatch) {
                setAuditLocationDetails(exactMatch);
                clearAuditLocationsSearch();
            } else {
                const lookForPalletExactMatch = auditLocationsSearchResults.find(
                    loc => loc.storagePlace === `P${auditLocation}`
                );
                if (lookForPalletExactMatch) {
                    setAuditLocationDetails(lookForPalletExactMatch);
                    clearAuditLocationsSearch();
                }
            }
        }
    }, [
        auditLocationsSearchResults,
        auditLocation,
        clearAuditLocationsSearch,
        setAuditLocationDetails
    ]);

    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={2}>
                <Search
                    propertyName="auditLocation"
                    label="Audit Location"
                    resultsInModal
                    resultLimit={100}
                    helperText={
                        disabled || !setAuditLocation ? '' : '<Enter> to search. <Tab> to select'
                    }
                    value={auditLocation}
                    disabled={disabled || !setAuditLocation}
                    handleValueChange={(_, newVal) => {
                        setAuditLocation(newVal);
                    }}
                    search={searchAuditLocations}
                    loading={auditLocationsSearchLoading}
                    searchResults={auditLocationsSearchResults}
                    priorityFunction="closestMatchesFirst"
                    onKeyPressFunctions={[
                        {
                            keyCode: 9,
                            action: () => {
                                if (auditLocation) {
                                    searchAuditLocations(auditLocation?.toUpperCase());
                                }
                            }
                        }
                    ]}
                    onResultSelect={r => {
                        setAuditLocationDetails(r);
                    }}
                    clearSearch={clearAuditLocationsSearch}
                    autoFocus={false}
                />
            </Grid>
            <Grid size={10} />
        </>
    );
}

export default AuditLocationSearch;
