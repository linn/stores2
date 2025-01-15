import React from 'react';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { DataGrid } from '@mui/x-data-grid';
import { CreateButton, Loading } from '@linn-it/linn-form-components-library';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';

function StockPools() {
    const { isLoading, result } = useInitialise(itemTypes.stockPools.url);

    const stockPoolColumns = [
        { field: 'stockPoolCode', headerName: 'Code', width: 100 },
        { field: 'stockPoolDescription', headerName: 'Description', width: 225 }
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
                <Grid size={12}>
                    <DataGrid
                        getRowId={row => row.stockPoolCode}
                        rows={result}
                        columns={stockPoolColumns}
                        rowHeight={34}
                        loading={false}
                        hideFooter
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default StockPools;
