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
    const [stockPools, setStockPools] = useState();
    const [searchTerm, setSearchTerm] = useState();
    const [rowUpdated, setRowUpdated] = useState();
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
        forRow: null
    });
    const searchRenderCell = params => (
        <>
            <GridSearchIcon
                style={{ cursor: 'pointer' }}
                onClick={() =>
                    setSearchDialogOpen({
                        forRow: params.id
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
    };

    const handleCancelSelect = () => {
        const oldRow = stockPoolResult.find(sp => sp.stockPoolCode === rowUpdated);

        setStockPools(prevRows =>
            prevRows.map(row => (row.stockPoolCode === oldRow.stockPoolCode ? oldRow : row))
        );

        setRowUpdated(null);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };
        setStockPools(prevRows =>
            prevRows.map(row =>
                row.stockPoolCode === newRow.stockPoolCode || row.stockPoolCode === ''
                    ? updatedRow
                    : row
            )
        );
        setRowUpdated(newRow.stockPoolCode);
        return newRow;
    };

    const renderSearchDialog = () => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = stockPools?.find(r => r.stockPoolCode === searchDialogOpen.forRow);
            const newRow = {
                ...currentRow,
                updated: true,
                defaultLocation: selected.locationId,
                defaultLocationName: selected.locationCode
            };

            processRowUpdate(newRow, currentRow);
            setSearchDialogOpen({ forRow: null });
        };

        return (
            <Dialog open={searchDialogOpen.forRow} onClose={handleClose}>
                <DialogTitle>Search</DialogTitle>
                <DialogContent>
                    <Search
                        autoFocus
                        propertyName="defaultLocation"
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
        );
    };

    const stockPoolColumns = [
        {
            field: 'stockPoolCode',
            headerName: 'Code',
            editable: true,
            width: 100
        },
        {
            field: 'stockPoolDescription',
            headerName: 'Description',
            editable: true,
            width: 225
        },
        {
            field: 'dateInvalid',
            headerName: 'Date Invalid',
            width: 175,
            editable: true,
            valueFormatter: value => value && moment(value).format('DD-MMM-YYYY')
        },
        {
            field: 'stockCategory',
            headerName: 'Stock Category',
            editable: true,
            width: 130
        },
        {
            field: 'accountingCompanyCode',
            headerName: 'Accounting Company',
            width: 175,
            defaultFormat: true,
            editable: true,
            valueOptions: accountingCompany?.map(ac => ac.name),
            type: 'singleSelect'
        },
        {
            field: 'defaultLocationName',
            headerName: 'Default Location',
            width: 150,
            editable: true,
            renderCell: searchRenderCell
        },
        {
            field: 'availableToMrp',
            headerName: 'Available to Mrp',
            width: 175,
            valueOptions: ['Y', 'N'],
            editable: true,
            type: 'singleSelect'
        },
        {
            field: 'sequence',
            headerName: 'Sequence',
            editable: true,
            width: 100
        }
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
            {searchDialogOpen.forRow && renderSearchDialog()}
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Stock Pools</Typography>
                </Grid>
                <Grid size={12}>
                    <Typography>
                        Please Note: You can only update/create 1 Stock Pool at a time.
                    </Typography>
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
                        rowSelectionModel={[rowUpdated]}
                        loading={false}
                        hideFooter
                        isCellEditable={params => {
                            if (params.field === 'stockPoolCode' && params.row.creating) {
                                return true;
                            }
                            if (!rowUpdated || params.id === rowUpdated) {
                                return true;
                            }
                            return false;
                        }}
                    />
                </Grid>
                <Grid item xs={4}>
                    <Button onClick={addNewRow} variant="outlined" disabled={rowUpdated}>
                        Add Stock Pool
                    </Button>
                </Grid>
                <Grid item xs={4}>
                    <Button
                        onClick={() => {
                            const updatedStockPool = stockPools.find(sp => sp.updated === true);
                            if (updatedStockPool.creating) {
                                clearCreateStockPool();
                                createStockPool(null, updatedStockPool);
                            } else {
                                updateStockPool(updatedStockPool.stockPoolCode, updatedStockPool);
                            }
                            setRowUpdated(null);
                        }}
                        variant="outlined"
                        disabled={stockPoolResult === stockPools}
                    >
                        Save
                    </Button>
                </Grid>
                <Grid size={4}>
                    <Button onClick={handleCancelSelect} variant="outlined" disabled={!rowUpdated}>
                        Cancel Select
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
