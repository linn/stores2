import React from 'react';
import Grid from '@mui/material/Grid2';
import { DataGrid } from '@mui/x-data-grid';
import { utilities } from '@linn-it/linn-form-components-library';
import { Link } from 'react-router';

function TransactionsTab({ transactions = [] }) {
    const columns = [
        {
            field: 'budgetId',
            headerName: 'Budget Id',
            width: 200,
            renderCell: params => (
                <Link to={utilities.getSelfHref(params.row)}>{params.row.budgetId}</Link>
            )
        },
        { field: 'partNumber', headerName: 'Part', width: 150 },
        { field: 'transactionCode', headerName: 'Trans Code', width: 150 },
        { field: 'reference', headerName: 'Ref', width: 150 },
        { field: 'partPrice', headerName: 'Part Price', width: 150 },
        { field: 'materialPrice', headerName: 'Mat. Price', width: 150 },
        { field: 'machinePrice', headerName: 'Machine Price', width: 150 },
        { field: 'overheadPrice', headerName: 'Overhead Price', width: 150 }
    ];

    return (
        <Grid container spacing={3}>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.budgetId}
                    rows={transactions}
                    columns={columns}
                    hideFooter
                    density="compact"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
        </Grid>
    );
}

export default TransactionsTab;
