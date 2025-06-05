import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { Search } from '@linn-it/linn-form-components-library';
import config from '../config';
import itemTypes from '../itemTypes';
import useSearch from '../hooks/useSearch';
import Page from './Page';

function Pallets() {
    const [searchPallet, setSearchPallet] = useState('');

    const {
        search: search,
        results: palletSearchResults,
        loading: palletSearchLoading,
        clear: clearPallets
    } = useSearch(itemTypes.pallets.url, 'palletNumber', 'palletNumber', 'description');

    const handlePalletSearchResultSelect = selected => {
        setSearchPallet(selected.description);
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Pallets</Typography>
                </Grid>
                <Grid size={2}>
                    <Search
                        autoFocus
                        propertyName="palletNumber"
                        label="Pallet Number"
                        resultsInModal
                        resultLimit={100}
                        value={searchPallet}
                        loading={palletSearchLoading}
                        handleValueChange={(_, newVal) => setSearchPallet(newVal)}
                        search={search}
                        searchResults={palletSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handlePalletSearchResultSelect}
                        clearSearch={clearPallets}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default Pallets;
