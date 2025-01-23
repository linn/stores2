import React, { useState } from 'react';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';
import { Typography } from '@mui/material';

function MovesTab({ movesFrom = [] }) {
    console.log(movesFrom);
    const movesColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 100
        },
        {
            field: 'state',
            headerName: 'State',
            width: 100
        }
    ];

    return (
        <Grid container spacing={3}>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.seq}
                    rows={movesFrom}
                    columns={movesColumns}
                    editMode="cell"
                    loading={false}
                />
            </Grid>
        </Grid>
    );
}

MovesTab.propTypes = { lines: PropTypes.arrayOf(PropTypes.shape({})) };

export default MovesTab;
