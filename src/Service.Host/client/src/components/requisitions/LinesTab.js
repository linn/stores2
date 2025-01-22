import React, { useState } from 'react';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';
import { Typography } from '@mui/material';

function LinesTab({ lines = [] }) {
    const [selected, setSelected] = useState([]);
    const linesColumns = [
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
        },
        {
            field: 'document1Type',
            headerName: 'Doc1 Type',
            width: 100
        },
        {
            field: 'document1Number',
            headerName: 'Doc1 Number',
            width: 100
        },
        {
            field: 'document1Line',
            headerName: 'Doc1 Line',
            width: 100
        },
        {
            field: 'dateBooked',
            headerName: 'Booked',
            width: 100
        },
        {
            field: 'cancelled',
            headerName: 'Cancelled',
            width: 100
        }
    ];

    const postingsColumns = [
        {
            field: 'debitOrCredit',
            headerName: 'D/C',
            width: 100
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 100
        },
        {
            field: 'nominalCode',
            headerName: 'Nominal',
            width: 150
        },
        {
            field: 'departmentCode',
            headerName: 'Dept',
            width: 150
        }
    ];
    return (
        <Grid container spacing={3}>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={lines}
                    selected={selected}
                    onRowSelectionModelChange={s => {
                        setSelected(s);
                    }}
                    columns={linesColumns}
                    editMode="cell"
                    loading={false}
                />
            </Grid>
            <Grid size={12}>
                <Typography variant="h6">
                    {!selected?.length
                        ? 'Click a row to view postings'
                        : `Postings for line ${selected[0]}`}
                </Typography>
            </Grid>
            <Grid size={6}>
                <DataGrid
                    getRowId={r => r.seq}
                    rows={
                        selected?.length
                            ? (lines.find(l => l.lineNumber === selected[0])?.postings ?? [])
                            : []
                    }
                    columns={postingsColumns}
                    noRowsLabel="No Line Selected"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
        </Grid>
    );
}

LinesTab.propTypes = { lines: PropTypes.arrayOf(PropTypes.shape({})) };

export default LinesTab;
