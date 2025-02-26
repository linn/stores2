import React from 'react';
import Grid from '@mui/material/Grid2';
import { DataGrid } from '@mui/x-data-grid';
import { Typography } from '@mui/material';

function MovesTab({ moves = [] }) {
    const headerColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 100
        },
        {
            field: 'part',
            headerName: 'Part',
            width: 150
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 150
        },
        {
            field: 'dateBooked',
            headerName: 'Booked?',
            width: 150
        },
        {
            field: 'dateCancelled',
            headerName: 'Cancelled?',
            width: 100
        }
    ];

    const fromColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 80
        },
        {
            field: 'locationCode',
            headerName: 'Loc Code',
            width: 100
        },
        {
            field: 'locationDescription',
            headerName: 'Loc Desc',
            width: 150
        },
        {
            field: 'palletNumber',
            headerName: 'Pallet',
            width: 150
        },
        {
            field: 'stockPool',
            headerName: 'Stock Pool',
            width: 120
        },
        {
            field: 'state',
            headerName: 'State',
            width: 100
        },
        {
            field: 'batchRef',
            headerName: 'Batch Ref',
            width: 100
        },
        {
            field: 'batchDate',
            headerName: 'Batch Date',
            width: 120
        },
        {
            field: 'qtyAtLocation',
            headerName: 'At Location',
            width: 100
        },
        {
            field: 'qtyAllocated',
            headerName: 'Allocated',
            width: 100
        }
    ];

    const toColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 80
        },
        {
            field: 'locationCode',
            headerName: 'Loc Code',
            width: 100
        },
        {
            field: 'locationDescription',
            headerName: 'Loc Desc',
            width: 150
        },
        {
            field: 'palletNumber',
            headerName: 'Pallet',
            width: 150
        },
        {
            field: 'stockPool',
            headerName: 'Stock Pool',
            width: 120
        },
        {
            field: 'state',
            headerName: 'State',
            width: 100
        },
        {
            field: 'serialNumber',
            headerName: 'Serial Num',
            width: 100
        },
        {
            field: 'remarks',
            headerName: 'Remarks',
            width: 100
        }
    ];

    return (
        <Grid container spacing={3}>
            <Grid size={6}>
                <DataGrid
                    getRowId={r => r.seq}
                    rows={moves}
                    hideFooter
                    columns={headerColumns}
                    density="compact"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
            <Grid size={6} />
            <Grid size={12}>
                <Typography variant="h6">From</Typography>
            </Grid>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.seq}
                    rows={moves.filter(x => x.from).map(m => m.from)}
                    hideFooter
                    columns={fromColumns}
                    density="compact"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h6">Onto</Typography>
            </Grid>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.seq}
                    rows={moves.filter(x => x.to).map(m => m.to)}
                    hideFooter
                    columns={toColumns}
                    density="compact"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
        </Grid>
    );
}

export default MovesTab;
