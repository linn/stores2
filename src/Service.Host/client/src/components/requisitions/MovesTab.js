import React from 'react';
import Grid from '@mui/material/Grid2';
import { DataGrid } from '@mui/x-data-grid';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import AddIcon from '@mui/icons-material/Add';

function MovesTab({ moves = [], addMoveOnto = null, updateMoveOnto = null }) {
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
            width: 100
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
            width: 100
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
            width: 100
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
            width: 100
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 100,
            editable: true,
            type: 'number'
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
            editable: true,
            width: 150
        },
        {
            field: 'stockPool',
            headerName: 'Stock Pool',
            width: 100
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

    const processRowUpdate = newRow => {
        console.log(newRow);
        updateMoveOnto(newRow);
        return newRow;
    };

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
                    rows={moves.filter(x => x.from).map(m => ({ seq: m.seq, ...m.from }))}
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
                    rows={moves.filter(x => x.to).map(m => ({ seq: m.seq, ...m.to, qty: m.qty }))}
                    hideFooter
                    columns={toColumns}
                    processRowUpdate={processRowUpdate}
                    density="compact"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
            <Grid size={12}>
                <Tooltip title="Add Line">
                    <IconButton
                        disabled={!addMoveOnto}
                        onClick={addMoveOnto}
                        color="primary"
                        size="large"
                    >
                        <AddIcon />
                    </IconButton>
                </Tooltip>
            </Grid>
        </Grid>
    );
}

export default MovesTab;
