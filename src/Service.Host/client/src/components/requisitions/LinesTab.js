import React, { useState } from 'react';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import CancelIcon from '@mui/icons-material/Cancel';
import AddIcon from '@mui/icons-material/Add';

import Typography from '@mui/material/Typography';
import BudgetPostings from '../BudgetPostings';
import CancelWithReasonDialog from '../CancelWithReasonDialog';

function LinesTab({
    lines = [],
    selected = null,
    setSelected,
    cancelLine,
    canAdd,
    addLine,
    showPostings
}) {
    const [cancelDialogVisible, setCancelDialogVisible] = useState(false);

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
        },
        {
            field: 'actions',
            headerName: 'Actions',
            width: 150,
            renderCell: params => (
                <Tooltip title="Cancel Line">
                    <IconButton
                        disabled={params.row.cancelled === 'Y' || params.row.isAddition}
                        onClick={() => {
                            setSelected(params.row.lineNumber);
                            setCancelDialogVisible(true);
                        }}
                    >
                        <CancelIcon />
                    </IconButton>
                </Tooltip>
            )
        }
    ];

    return (
        <Grid container spacing={3}>
            {cancelDialogVisible && (
                <CancelWithReasonDialog
                    visible={cancelDialogVisible}
                    title={`Enter a reason to cancel LINE ${selected}`}
                    closeDialog={() => setCancelDialogVisible(false)}
                    onConfirm={reason => {
                        cancelLine(null, {
                            reason,
                            reqNumber: lines[0].reqNumber,
                            lineNumber: selected
                        });
                    }}
                />
            )}
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={lines}
                    rowSelectionModel={selected ? [selected] : []}
                    onRowClick={row => {
                        setSelected(row.id);
                    }}
                    columns={linesColumns}
                    hideFooter
                    density="compact"
                    editMode="cell"
                    loading={false}
                />
            </Grid>
            <Grid size={12}>
                <Tooltip title="Add Line">
                    <IconButton disabled={!canAdd} onClick={addLine} color="primary" size="large">
                        <AddIcon />
                    </IconButton>
                </Tooltip>
            </Grid>
            <Grid size={12}>
                <Typography variant="h6">
                    {!selected ? 'Click a row to view postings' : `Postings for line ${selected}`}
                </Typography>
            </Grid>
            <Grid size={10}>
                {selected && showPostings && (
                    <BudgetPostings
                        budgetPostings={lines.find(l => l.lineNumber === selected)?.postings ?? []}
                    />
                )}
            </Grid>
            <Grid size={2} />
        </Grid>
    );
}

LinesTab.propTypes = {
    lines: PropTypes.arrayOf(PropTypes.shape({})).isRequired,
    selected: PropTypes.number,
    cancelLine: PropTypes.func.isRequired,
    setSelected: PropTypes.func.isRequired,
    canAdd: PropTypes.bool.isRequired,
    addLine: PropTypes.func.isRequired,
    showPostings: PropTypes.func.isRequired
};

export default LinesTab;
