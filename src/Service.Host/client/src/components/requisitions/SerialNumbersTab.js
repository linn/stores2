import React from 'react';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';

function SerialNumbersTab({
    serialNumbers = [],
    addSerialNumber,
    updateSerialNumber,
    deleteSerialNumber
}) {
    const columns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 100
        },
        {
            field: 'serialNumber',
            headerName: 'Serial#',
            width: 150,
            type: 'number',
            valueFormatter: params => {
                // prevent sernos 1234 displaying as 1,234
                return params != null ? params.toString() : '';
            },
            editable: true
        },
        {
            field: 'delete',
            headerName: '',
            width: 120,
            renderCell: params => (
                <Tooltip title="Delete">
                    <div>
                        {addSerialNumber && (
                            <IconButton
                                aria-label="delete"
                                size="small"
                                onClick={() => deleteSerialNumber(params.row.seq)}
                            >
                                <DeleteIcon fontSize="inherit" />
                            </IconButton>
                        )}
                    </div>
                </Tooltip>
            )
        }
    ];

    const processRowUpdate = updated => {
        updateSerialNumber(updated);
        return updated;
    };

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
                        processRowUpdate={processRowUpdate}
                        loading={false}
                    />
                </Grid>
                <Grid size={6} />
                <Grid size={12}>
                    {addSerialNumber && (
                        <Tooltip title="Add Line">
                            <IconButton onClick={addSerialNumber} color="primary" size="large">
                                <AddIcon />
                            </IconButton>
                        </Tooltip>
                    )}
                </Grid>
            </Grid>
        </>
    );
}

export default SerialNumbersTab;
