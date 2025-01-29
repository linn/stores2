import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { DataGrid } from '@mui/x-data-grid';
import { Loading, ErrorCard, SnackbarMessage } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import usePut from '../hooks/usePut';
import useInitialise from '../hooks/useInitialise';
import usePost from '../hooks/usePost';
import useGet from '../hooks/useGet';

function StorageTypes() {
    const { isLoading, result: storageTypesResult } = useInitialise(itemTypes.storageTypes.url);
    const [creating, setCreating] = useState(false);
    const [storageTypes, setStorageTypes] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);
    const [rowUpdated, setRowUpdated] = useState();

    const {
        send: getNewStorageTypes,
        isNewStorageTypesLoading,
        result: newStorageTypesGetResult
    } = useGet(itemTypes.storageTypes.url);

    const {
        send: updateStorageType,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult,
        clearPutResult: clearUpdateResult
    } = usePut(itemTypes.storageTypes.url, true);

    const {
        send: createStorageType,
        createStorageTypeLoading,
        errorMessage: createStorageTypeError,
        postResult: createStorageTypeResult,
        clearPostResult: clearCreateStorageType
    } = usePost(itemTypes.storageTypes.url);

    useEffect(() => {
        setStorageTypes(storageTypesResult);
    }, [storageTypesResult]);

    useEffect(() => {
        if (!!updateResult || !!createStorageTypeResult) {
            getNewStorageTypes();
            setSnackbarVisible(true);
            clearCreateStorageType();
            clearUpdateResult();
        }
    }, [
        clearCreateStorageType,
        updateResult,
        getNewStorageTypes,
        createStorageTypeResult,
        clearUpdateResult
    ]);

    useEffect(() => {
        if (newStorageTypesGetResult) {
            setStorageTypes(newStorageTypesGetResult);
        } else {
            setStorageTypes(storageTypesResult);
        }
    }, [newStorageTypesGetResult, storageTypesResult]);

    const addNewRow = () => {
        setStorageTypes([
            ...storageTypes,
            {
                storageTypeCode: '',
                description: '',
                creating: true
            }
        ]);
        setCreating(true);
    };

    const handleCancelSelect = () => {
        const oldRow = newStorageTypesGetResult
            ? newStorageTypesGetResult.find(st => st.storageTypeCode === rowUpdated)
            : storageTypesResult.find(st => st.storageTypeCode === rowUpdated);

        setStorageTypes(prevRows =>
            prevRows.map(row => (row.storageTypeCode === oldRow.storageTypeCode ? oldRow : row))
        );

        setRowUpdated(null);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };
        setStorageTypes(prevRows =>
            prevRows.map(row =>
                row.storageTypeCode === newRow.storageTypeCode || row.storageTypeCode === ''
                    ? updatedRow
                    : row
            )
        );
        setRowUpdated(newRow.storageTypeCode);
        return newRow;
    };

    const StorageTypeColumns = [
        {
            field: 'storageTypeCode',
            headerName: 'Code',
            editable: true,
            width: 100
        },
        { field: 'description', editable: true, headerName: 'Description', width: 400 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Storage Types</Typography>
                </Grid>
                <Grid size={12}>
                    <Typography>
                        Please Note: You can only update/create 1 Stock Pool at a time.
                    </Typography>
                </Grid>
                {(isLoading ||
                    isNewStorageTypesLoading ||
                    createStorageTypeLoading ||
                    updateLoading) && (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                )}
                <Grid size={12}>
                    <DataGrid
                        getRowId={row => row.storageTypeCode}
                        rows={storageTypes}
                        editMode="cell"
                        processRowUpdate={processRowUpdate}
                        columns={StorageTypeColumns}
                        rowHeight={34}
                        loading={false}
                        rowSelectionModel={[rowUpdated]}
                        hideFooter
                        isCellEditable={params => {
                            if (params.field === 'storageTypeCode' && params.row.creating) {
                                return true;
                            }
                            if (
                                (!rowUpdated || params.id === rowUpdated) &&
                                params.field !== 'storageTypeCode'
                            ) {
                                return true;
                            }
                            return false;
                        }}
                    />
                </Grid>
                <Grid item xs={4}>
                    <Button onClick={addNewRow} variant="outlined" disabled={creating}>
                        Add Storage Type
                    </Button>
                </Grid>
                <Grid item xs={4}>
                    <Button
                        onClick={() => {
                            const updatedStorageType = storageTypes.find(sp => sp.updated === true);
                            if (updatedStorageType.creating) {
                                clearCreateStorageType();
                                createStorageType(null, updatedStorageType);
                            } else {
                                updateStorageType(
                                    updatedStorageType.storageTypeCode,
                                    updatedStorageType
                                );
                            }
                            setRowUpdated(null);
                        }}
                        variant="outlined"
                        disabled={storageTypesResult === storageTypes}
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
                {(updateError || createStorageTypeError) && (
                    <Grid item xs={12}>
                        <ErrorCard
                            errorMessage={
                                updateError ? updateError?.details : createStorageTypeError
                            }
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default StorageTypes;
