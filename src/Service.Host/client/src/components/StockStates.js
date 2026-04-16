import React from 'react';
import { useNavigate } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import { Loading } from '@linn-it/linn-form-components-library';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import Page from './Page';

function StockStates() {
    const { isLoading, result } = useInitialise(itemTypes.stockStates.url);
    const navigate = useNavigate();

    const columns = [
        { field: 'state', headerName: 'State', width: 150 },
        { field: 'description', headerName: 'Description', width: 300 },
        { field: 'qcRequired', headerName: 'QC Required', width: 120 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={11}>
                    <Typography variant="h4">Stock States</Typography>
                </Grid>
                <Grid size={1}>
                    <Button
                        variant="outlined"
                        onClick={() => navigate('/stores2/stock/states/create')}
                    >
                        Create
                    </Button>
                </Grid>
                {isLoading ? (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <Grid size={12}>
                        <DataGrid
                            rows={result ?? []}
                            columns={columns}
                            getRowId={row => row.state}
                            onRowClick={params =>
                                navigate(`/stores2/stock/states/${params.row.state}`)
                            }
                            rowHeight={34}
                            hideFooter
                            sx={{ cursor: 'pointer' }}
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default StockStates;
