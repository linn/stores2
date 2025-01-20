import React from 'react';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';

function LinesTab({ lines = [1, 2, 3, 4, 5] }) {
    const columns = [
        {
            field: 'lineNumber',
            headerName: '#',
            width: 100
        },
        { field: 'partNumber', headerName: 'Part', width: 150 },
        {
            field: 'partDescription',
            headerName: 'Desc',
            width: 300
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 100
        },
        {
            field: 'transactionCode',
            headerName: 'Trans Code',
            width: 100
        },
        {
            field: 'transactionCodeDescription',
            headerName: 'Trans Desc',
            width: 200
        }
    ];
    return (
        <Grid container spacing={3}>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={lines}
                    columns={columns}
                    editMode="cell"
                    loading={false}
                />
            </Grid>
        </Grid>
    );
}

LinesTab.propTypes = { lines: PropTypes.arrayOf(PropTypes.shape({})) };

export default LinesTab;
