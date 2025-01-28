import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import Dialog from '@mui/material/Dialog';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import { DataGrid, GridSearchIcon } from '@mui/x-data-grid';
import { Loading, Search, ErrorCard, SnackbarMessage } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import usePut from '../hooks/usePut';
import useInitialise from '../hooks/useInitialise';
import usePost from '../hooks/usePost';
import useSearch from '../hooks/useSearch';

function StockPools() {
    const { isLoading, result: stockPoolResult } = useInitialise(itemTypes.stockPools.url);
    const { isLoading: accountingCompanyLoading, result: accountingCompany } = useInitialise(
        itemTypes.accountingCompany.url
    );
    const [creating, setCreating] = useState(false);
    const [stockPools, setStockPools] = useState();
    const [searchTerm, setSearchTerm] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);

    const {
        search: searchStorageLocations,
        results: storageLocationsSearchResults,
        loading: storageLocationsSearchLoading,
        clear: clearStorageLocation
    } = useSearch(itemTypes.storageLocations.url, 'locationId', 'locationCode', 'description');

    const {
        send: updateStockPool,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.stockPools.url, true);

    const {
        send: createStockPool,
        createStockPoolLoading,
        errorMessage: createStockPoolError,
        postResult: createStockPoolResult,
        clearPostResult: clearCreateStockPool
    } = usePost(itemTypes.stockPools.url);

    useEffect(() => {
        setSnackbarVisible(!!updateResult || !!createStockPoolResult);
    }, [createStockPoolResult, updateResult]);

    useEffect(() => {
        setStockPools(stockPoolResult);
    }, [stockPoolResult]);

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
        setStockPools([
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
                stockPoolCode: '',
                stockPoolDescription: null,
                storageLocation: null,
                creating: true,
                id: stockPools.length + 1
            }
        ]);
        setCreating(true);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };
        setStockPools(prevRows => prevRows.map(row => (row.id === newRow.id ? updatedRow : row)));
        return newRow;
    };

    const renderSearchDialog = c => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = stockPools?.find(r => r.id === searchDialogOpen.forRow);
            let newRow = {
                ...currentRow,
                updated: true,
                defaultLocation: selected.locationId,
                defaultLocationName: selected.locationCode
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
                            label="defaultLocation"
                            resultsInModal
                            resultLimit={100}
                            value={searchTerm}
                            loading={storageLocationsSearchLoading}
                            handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                            search={searchStorageLocations}
                            searchResults={storageLocationsSearchResults}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleSearchResultSelect}
                            clearSearch={clearStorageLocation}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Close</Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    };

    const stockPoolColumns = [
        {
            field: 'stockPoolCode',
            headerName: 'Code',
            editable: params => params.row?.creating === true,
            width: 100
        },
        { field: 'stockPoolDescription', editable: true, headerName: 'Description', width: 225 },
        {
            field: 'dateInvalid',
            headerName: 'Date Invalid',
            editable: true,
            width: 175,
            valueFormatter: value => value && moment(value).format('DD-MMM-YYYY')
        },
        { field: 'stockCategory', editable: true, headerName: 'Stock Category', width: 130 },
        {
            field: 'accountingCompanyCode',
            editable: true,
            headerName: 'Accounting Company',
            width: 175,
            defaultFormat: true,
            valueOptions: accountingCompany?.map(ac => ac.name),
            type: 'singleSelect'
        },
        {
            field: 'defaultLocationName',
            headerName: 'Default Location',
            width: 150,
            editable: true,
            type: 'search',
            searchResults: storageLocationsSearchResults,
            searchLoading: storageLocationsSearchLoading,
            searchUpdateFieldNames: [
                { fieldName: 'defaultLocation', searchResultFieldName: 'locationId' },
                { fieldName: 'defaultLocationName', searchResultFieldName: 'locationCode' }
            ],
            clearSearch: clearStorageLocation,
            renderCell: searchRenderCell
        },
        { field: 'availableToMrp', editable: true, headerName: 'Available to Mrp', width: 175 },
        { field: 'sequence', editable: true, headerName: 'Sequence', width: 100 }
    ];

    const sortedStockPools = stockPools?.slice().sort((a, b) => {
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
            {stockPoolColumns?.filter(c => c.type === 'search').map(c => renderSearchDialog(c))}
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Stock Pools</Typography>
                </Grid>
                {(isLoading ||
                    accountingCompanyLoading ||
                    updateLoading ||
                    createStockPoolLoading) && (
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
                        isCellEditable={params => {
                            if (params.field === 'stockPoolCode') {
                                return params.row?.creating === true;
                            }
                            return true;
                        }}
                    />
                </Grid>
                <Grid item xs={4}>
                    <Button onClick={addNewRow} variant="outlined" disabled={creating}>
                        Add Stock Pool
                    </Button>
                </Grid>
                <Grid item xs={4}>
                    <Button
                        onClick={() => {
                            stockPools
                                .filter(sp => sp.updated === true)
                                .forEach(sp => {
                                    if (sp.creating) {
                                        clearCreateStockPool();
                                        createStockPool(null, sp);
                                        setCreating(false);
                                    } else {
                                        updateStockPool(sp.stockPoolCode, sp);
                                    }
                                });
                        }}
                        variant="outlined"
                        disabled={stockPoolResult === stockPools}
                    >
                        Save
                    </Button>
                </Grid>
                <Grid>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() => setSnackbarVisible(false)}
                        message="Save Successful"
                    />
                </Grid>
                {(updateError || createStockPoolError) && (
                    <Grid item xs={12}>
                        <ErrorCard
                            errorMessage={updateError ? updateError?.details : createStockPoolError}
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default StockPools;
