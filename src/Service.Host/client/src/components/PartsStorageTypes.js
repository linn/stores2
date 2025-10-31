import React from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { useNavigate } from 'react-router-dom';
import { DataGrid } from '@mui/x-data-grid';
import { Loading, CreateButton, utilities } from '@linn-it/linn-form-components-library';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import Page from './Page';

function PartsStorageTypes() {
    const { isLoading: isPartsStorageTypesLoading, result: partsStorageTypes } = useInitialise(
        itemTypes.partsStorageTypes.url
    );

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
                {isPartsStorageTypesLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid size={12}>
                    <DataGrid
                        getRowId={row => row.bridgeId}
                        rows={partsStorageTypes}
                        editMode="cell"
                        columns={StorageTypeColumns}
                        onRowClick={clicked => {
                            navigate(utilities.getSelfHref(clicked.row));
                        }}
                        rowHeight={34}
                        loading={isPartsStorageTypesLoading}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartsStorageTypes;
