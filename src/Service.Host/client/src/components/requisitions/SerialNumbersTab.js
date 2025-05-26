import React from 'react';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import AddIcon from '@mui/icons-material/Add';

function SerialNumbersTab({ serialNumbers = [], addSerialNumbers = null }) {
    const columns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 100
        },
        {
            field: 'serialNumber',
            headerName: 'Serial#',
            width: 150
        }
    ];

    return (
        <>
            <Grid container spacing={3}>
                <Grid size={6}>
                    <DataGrid
                        getRowId={r => r.seq}
                        rows={serialNumbers}
                        hideFooter
                        columns={columns}
                        density="compact"
                        editMode="cell"
                        loading={false}
                    />
                </Grid>
                <Grid size={6} />
                <Grid size={12}>
                    <Tooltip title="Add Line">
                        <IconButton onClick={addSerialNumbers} color="primary" size="large">
                            <AddIcon />
                        </IconButton>
                    </Tooltip>
                </Grid>
            </Grid>
        </>
    );
}

export default SerialNumbersTab;
