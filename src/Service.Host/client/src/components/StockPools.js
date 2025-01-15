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
    const { isLoading, result: stockPools } = useInitialise(itemTypes.stockPools.url);

    const stockPoolColumns = [
        { field: 'stockPoolCode', headerName: 'Code', width: 100 },
        { field: 'stockPoolDescription', headerName: 'Description', width: 225 },
        { field: 'sequence', headerName: 'Sequence', width: 100 },
        { field: 'dateInvalid', headerName: 'Date Invalid', width: 225 }
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
