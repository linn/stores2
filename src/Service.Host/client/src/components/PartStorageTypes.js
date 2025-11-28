import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { useNavigate } from 'react-router-dom';
import { DataGrid } from '@mui/x-data-grid';
import { Loading, CreateButton, utilities, Search } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import useSearch from '../hooks/useSearch';
import Page from './Page';

function PartStorageTypes() {
    const [partSearchTerm, setPartSearchTerm] = useState('');
    const [storageTypeSearchTerm, setStorageTypeSearchTerm] = useState('');

    const {
        send,
        isLoading: ispartStorageTypesLoading,
        result: partStorageTypes
    } = useGet(itemTypes.partStorageTypes.url);

    const {
        search: searchParts,
        results: partsSearchResults,
        loading: partsSearchLoading,
        clear: clearParts
    } = useSearch(itemTypes.parts.url, 'partNumber', 'partNumber', 'description');

    const {
        search: searchStorageTypes,
        results: storageTypesSearchResults,
        loading: storageTypesSearchLoading,
        clear: clearStorageTypes
    } = useSearch(itemTypes.storageTypes.url, 'storageTypeCode', 'storageTypeCode', 'description');

    const handlePartSearchResultSelect = selected => {
        setPartSearchTerm(selected.partNumber);
    };

    const handleStorageTypeSearchResultSelect = selected => {
        setStorageTypeSearchTerm(selected.storageTypeCode);
    };

    const navigate = useNavigate();

    const StorageTypeColumns = [
        { field: 'partNumber', headerName: 'Part Number', width: 150 },
        {
            field: 'storageTypeCode',
            headerName: 'Storage Type Code',
            width: 150
        },
        {
            field: 'storageType',
            headerName: 'Storage Type Description',
            width: 300,
            valueGetter: value => {
                return value?.description || '';
            }
        },
        { field: 'maximum', headerName: 'Maximum', width: 100 },
        { field: 'bridgeId', headerName: 'Bridge ID', width: 100 },
        { field: 'incr', headerName: 'Incr', width: 100 },
        { field: 'preference', headerName: 'Preference', width: 100 },
        { field: 'remarks', headerName: 'Remarks', width: 100 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={11}>
                    <Typography variant="h4">Part Storage Types</Typography>
                </Grid>
                <Grid size={1}>
                    <CreateButton createUrl="/stores2/parts-storage-types/create" />
                </Grid>
                {ispartStorageTypesLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}

                <Grid size={6}>
                    <Search
                        autoFocus
                        propertyName="part"
                        label="Part"
                        resultsInModal
                        resultLimit={100}
                        value={partSearchTerm}
                        loading={partsSearchLoading}
                        handleValueChange={(_, newVal) => setPartSearchTerm(newVal.toUpperCase())}
                        search={searchParts}
                        searchResults={partsSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handlePartSearchResultSelect}
                        clearSearch={clearParts}
                    />
                </Grid>
                <Grid size={6}>
                    <Search
                        propertyName="storageType"
                        label="Storage Type"
                        autoFocus={false}
                        resultsInModal
                        resultLimit={100}
                        value={storageTypeSearchTerm}
                        loading={storageTypesSearchLoading}
                        handleValueChange={(_, newVal) => setStorageTypeSearchTerm(newVal)}
                        search={searchStorageTypes}
                        searchResults={storageTypesSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleStorageTypeSearchResultSelect}
                        clearSearch={clearStorageTypes}
                    />
                </Grid>

                <Grid size={1}>
                    <Button
                        variant="contained"
                        fullWidth
                        disabled={partSearchTerm.length === 0 && storageTypeSearchTerm.length === 0}
                        onClick={() => {
                            send(`?part=${partSearchTerm}&storageType=${storageTypeSearchTerm}`);
                        }}
                    >
                        Search
                    </Button>
                </Grid>

                <Grid size={1}>
                    <Button
                        variant="outlined"
                        fullWidth
                        disabled={partSearchTerm.length === 0 && storageTypeSearchTerm.length === 0}
                        onClick={() => {
                            setPartSearchTerm('');
                            setStorageTypeSearchTerm('');
                        }}
                    >
                        Clear
                    </Button>
                </Grid>

                <Grid size={12}>
                    <DataGrid
                        getRowId={row => row.bridgeId}
                        rows={partStorageTypes}
                        editMode="cell"
                        columns={StorageTypeColumns}
                        onRowClick={clicked => {
                            navigate(utilities.getSelfHref(clicked.row));
                        }}
                        rowHeight={34}
                        loading={ispartStorageTypesLoading}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartStorageTypes;
