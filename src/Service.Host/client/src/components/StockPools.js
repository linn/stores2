import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import Dialog from '@mui/material/Dialog';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import { DataGrid, GridSearchIcon } from '@mui/x-data-grid';
import { CreateButton, Loading, Search, ErrorCard } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import useInitialise from '../hooks/useInitialise';

function StockPools() {
    const { isLoading, result } = useInitialise(itemTypes.stockPools.url);
    const { stockPools, setStockPools } = useState(result);
    const { searchTerm, setSearchTerm } = useState(result);

    const [searchDialogOpen, setSearchDialogOpen] = useState({
        forRow: null,
        forColumn: null
    });
    const searchRenderCell = params => (
        <>
            <GridSearchIcon
                style={{ cursor: 'pointer' }}
                onClick={() =>
                    setSearchDialogOpen({
                        forRow: params.id,
                        forColumn: params.field
                    })
                }
            />
            {params.value}
        </>
    );

    const addNewRow = () => {
        setStockPools(
            ...stockPools,
            {
                    accountingCompany: null,
                    availableToMrp: 'Y',
                    bridgeId: null,
                    dateInvalid: null,
                    defaultLocation: null,
                    links: null,
                    sequence: null,
                    stockCategory: null,
                    stockPoolCode: null,
                    stockPoolDescription: null,
                    storageLocation: null
                }
        );
    };

    const processRowUpdate = newRow => {
        setPaymentType(pt => ({
            ...pt,
            elements: pt.elements?.map(x => (x.seq === newRow.seq ? newRow : x))
        }));
        return newRow;
    };

    const processRowUpdate = newRow => {
        setStockPools(pt => [
            ...pt
            // {
            //     accountingCompany: null,
            //     availableToMrp: 'Y',
            //     bridgeId: null,
            //     dateInvalid: null,
            //     defaultLocation: null,
            //     links: null,
            //     sequence: null,
            //     stockCategory: null,
            //     stockPoolCode: null,
            //     stockPoolDescription: null,
            //     storageLocation: null
            // }
        ]);
        return newRow;
    };

    const renderSearchDialog = c => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = selected.find(r => r.stockPoolCode === searchDialogOpen.forRow);
            let newRow = {
                ...currentRow,
                hasChanged: true
            };
            c.searchUpdateFieldNames?.forEach(f => {
                newRow = { ...newRow, [f.fieldName]: selected[f.searchResultFieldName] };
            });

            processRowUpdate(newRow, currentRow);
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        return (
            <div id={c.field}>
                <Dialog open={searchDialogOpen.forColumn === c.field} onClose={handleClose}>
                    <DialogTitle>Search</DialogTitle>
                    <DialogContent>
                        <Search
                            autoFocus
                            propertyName={`${c.field}-searchTerm`}
                            label=""
                            resultsInModal
                            resultLimit={100}
                            value={searchTerm}
                            loading={c.searchLoading}
                            handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                            search={c.search}
                            searchResults={c.searchResults?.map(r => ({
                                ...r,
                                id: r[c.field]
                            }))}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleSearchResultSelect}
                            clearSearch={() => {}}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Close</Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    };

    const {
        send: getStorageLocations,
        storageLocationsLoading,
        result: storageLocationsResult,
        clearData: clearStorageLocations
    } = useGet(itemTypes.storagePlaces.url);

    const {
        send: updateStockPool,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.carriers.url, true);

    const stockPoolColumns = [
        { field: 'stockPoolCode', headerName: 'Code', width: 100 },
        { field: 'stockPoolDescription', headerName: 'Description', width: 225 },
        {
            field: 'dateInvalid',
            headerName: 'Date Invalid',
            width: 175,
            valueFormatter: value => value && moment(value).format('DD-MMM-YYYY')
        },
        { field: 'stockCategory', headerName: 'Stock Category', width: 100 },
        { field: 'accountingCompanyCode', headerName: 'Accounting Company', width: 100 },
        {
            field: 'locationCode',
            headerName: 'Default Location',
            width: 200,
            valueGetter: params => params?.row?.storageLocation?.locationCode || ''
        },
        { field: 'sequence', headerName: 'Sequence', width: 100 },
        {
            field: 'defaultLocation',
            headerName: 'Default Location',
            width: 150,
            editable: true,
            type: 'search',
            search: getStorageLocations,
            searchResults: storageLocationsResult,
            searchLoading: storageLocationsLoading,
            searchUpdateFieldNames: [
                { fieldName: 'defaultLocation', searchResultFieldName: 'locationId' },
                { fieldName: 'defaultLocationName', searchResultFieldName: 'locationCode' }
            ],
            clearSearch: clearStorageLocations,
            renderCell: searchRenderCell
        }
    ];

    const sortedStockPools = stockPools?.sort((a, b) => {
        const fa = a.sequence;
        const fb = b.sequence;
        if (fa === null && fb !== null) {
            return 1;
        }
        if (fa !== null && fb === null) {
            return -1;
        }
        if (fa === null && fb === null) {
            return 0;
        }
        return fa - fb;
    });

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            {stockPoolColumns.filter(c => c.type === 'search').map(c => renderSearchDialog(c))}
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Stock Pools</Typography>
                </Grid>
                <Grid size={12}>
                    <CreateButton createUrl="/stores2/stock-pools/create" />
                </Grid>
                {isLoading && (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                )}
                <Grid size={12}>
                    <DataGrid
                        getRowId={row => row.stockPoolCode}
                        rows={sortedStockPools}
                        editMode="cell"
                        processRowUpdate={processRowUpdate}
                        columns={stockPoolColumns}
                        rowHeight={34}
                        loading={false}
                        hideFooter
                    />
                </Grid>
                <Grid item xs={4}>
                    <Button onClick={addNewRow} variant="outlined">
                        Add Stock Pool
                    </Button>
                </Grid>
                <Grid item xs={4}>
                    <Button
                        onClick={sp => {
                            updateStockPool(sp.stockPoolCode, stockPools);
                        }}
                        variant="outlined"
                        // eslint-disable-next-line eqeqeq
                        disabled={result == stockPools}
                    >
                        Save
                    </Button>
                </Grid>
                {updateError && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={updateError?.details} />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default StockPools;
