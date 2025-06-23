import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { Search, utilities } from '@linn-it/linn-form-components-library';
import config from '../config';
import itemTypes from '../itemTypes';
import useSearch from '../hooks/useSearch';
import Page from './Page';

function Pallets() {
    const navigate = useNavigate();
    const [searchPallet, setSearchPallet] = useState('');

    const {
        search: searchPallets,
        results: palletSearchResults,
        loading: palletSearchLoading,
        clear: clearPallets
    } = useSearch(itemTypes.pallets.url, 'palletNumber', 'palletNumber', 'description');

    const mappedPalletSearchResults =
        palletSearchResults?.map(r => ({
            ...r,
            name: r.palletNumber ? r.palletNumber.toString() : ''
        })) ?? [];

    const handlePalletSearchResultSelect = selected => {
        navigate(utilities.getSelfHref(selected));
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid size={10}>
                <Typography variant="h4">Pallets</Typography>
            </Grid>
            <Grid container spacing={3}>
                <Grid size={10}>
                    <Search
                        autoFocus
                        propertyName="palletNumber"
                        label="Pallet Number"
                        resultsInModal
                        resultLimit={100}
                        value={searchPallet}
                        loading={palletSearchLoading}
                        handleValueChange={(_, newVal) => setSearchPallet(newVal)}
                        search={searchPallets}
                        searchResults={mappedPalletSearchResults}
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
