import React from 'react';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import moment from 'moment';
import { Loading, CreateButton, utilities } from '@linn-it/linn-form-components-library';
import DataGrid from '@mui/x-data-grid';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';

function StockPools() {
    const { isLoading, result } = useInitialise(itemTypes.stockPools.url);
    const stockPools = result;

    const columns = [
        { field: 'StockPoolCode', headerName: 'Code', width: 100 },
        { field: 'StockPoolDescription', headerName: 'Description', width: 225 },
        {
            field: 'DateInvalid',
            headerName: 'Date Invalid',
            width: 175,
            valueFormatter: ({ value }) => value && moment(value).format('DD-MMM-YYYY')
        },
        {
            field: 'AccountingCompany.Name',
            headerName: 'Accounting Company',
            width: 200,
            valueGetter: params => params.row.AccountingCompany?.Name || ''
        },
        { field: 'StockCategory', headerName: 'Stock Category', width: 225 },
        {
            field: 'StorageLocation.Name',
            headerName: 'Storage Location',
            width: 200,
            valueGetter: params => params.row.StorageLocation?.LocationCode || ''
        },
        { field: 'Sequence', headerName: 'Sequence', width: 225 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
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
                <Grid item xs={12}>
                    <DataGrid
                        rows={stockPools}
                        getRowId={row => row?.StockPoolCode}
                        columns={columns}
                        editMode="cell"
                        onRowClick={clicked => {
                            utilities.getSelfHref(clicked.row);
                        }}
                        autoHeight
                        columnBuffer={8}
                        density="comfortable"
                        rowHeight={34}
                        loading={isLoading}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default StockPools;
