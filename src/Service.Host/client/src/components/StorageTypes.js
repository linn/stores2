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

function StorageTypes() {
    const { isLoading, result: storageTypesResult } = useInitialise(itemTypes.storageTypes.url);
    const [creating, setCreating] = useState(false);
    const [storageTypes, setStorageTypes] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);

    const {
        send: updateStorageType,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.storageTypes.url, true);

    const {
        send: createStorageType,
        createStorageTypeLoading,
        errorMessage: createStorageTypeError,
        postResult: createStorageTypeResult,
        clearPostResult: clearCreateStorageType
    } = usePost(itemTypes.storageTypes.url);

    useEffect(() => {
        setSnackbarVisible(!!updateResult || !!createStorageTypeResult);
    }, [createStorageTypeResult, updateResult]);

    useEffect(() => {
        setStorageTypes(storageTypesResult);
    }, [storageTypesResult]);

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

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };
        console.log(updatedRow);
        setStorageTypes(prevRows =>
            prevRows.map(row =>
                row.storageTypeCode === newRow.storageTypeCode || row.storageTypeCode === ''
                    ? updatedRow
                    : row
            )
        );
        console.log(storageTypes);
        return newRow;
    };

    const StorageTypeColumns = [
        {
            field: 'storageTypeCode',
            headerName: 'Code',
            editable: params => params.row?.creating === true,
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
                {(isLoading || createStorageTypeLoading || updateLoading) && (
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
                        hideFooter
                        isCellEditable={params => {
                            if (params.field === 'storageTypeCode') {
                                return params.row?.creating === true;
                            }
                            return true;
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
                            storageTypes
                                .filter(st => st.updated === true)
                                .forEach(st => {
                                    if (st.creating) {
                                        clearCreateStorageType();
                                        createStorageType(null, st);
                                        setCreating(false);
                                    } else {
                                        updateStorageType(st.storageTypeCode, st);
                                    }
                                });
                        }}
                        variant="outlined"
                        disabled={storageTypesResult === storageTypes}
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
