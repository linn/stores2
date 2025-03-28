import React, { useState } from 'react';
import Grid from '@mui/material/Grid2';
import { DataGrid, GridSearchIcon } from '@mui/x-data-grid';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Link from '@mui/material/Link';
import { utilities } from '@linn-it/linn-form-components-library';
import CancelIcon from '@mui/icons-material/Cancel';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import AddIcon from '@mui/icons-material/Add';
import WarehouseIcon from '@mui/icons-material/Warehouse';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import BudgetPostings from '../BudgetPostings';
import CancelWithReasonDialog from '../CancelWithReasonDialog';
import PartsSearchDialog from '../PartsSearchDialog';
import PickStockDialog from './PickStockDialog';

function LinesTab({
    lines = [],
    selected = null,
    setSelected,
    cancelLine,
    canAdd,
    addLine,
    showPostings,
    updateLine,
    pickStock,
    bookLine,
    canBook,
    fromState,
    fromStockPool
}) {
    const [cancelDialogVisible, setCancelDialogVisible] = useState(false);
    const [pickStockDialogVisible, setPickStockDialogVisible] = useState(false);

    const [partsSearchDialogOpen, setPartsSearchDialogOpen] = useState();

    const linesColumns = [
        { field: 'lineNumber', headerName: '#', width: 60 },
        {
            field: 'partNumber',
            headerName: 'Part',
            width: 150,
            renderCell: params => (
                <>
                    {params.row.isAddition ? (
                        <>
                            <GridSearchIcon
                                style={{ cursor: 'pointer' }}
                                onClick={() => setPartsSearchDialogOpen(params.id)}
                            />
                            {params.row.part?.partNumber}
                        </>
                    ) : (
                        <Link href={utilities.getHref(params.row, 'part')}>
                            {params.row.part?.partNumber}
                        </Link>
                    )}
                </>
            )
        },
        {
            field: 'partDescription',
            headerName: 'Desc',
            width: 300,
            renderCell: params => params.row.part?.description
        },
        { field: 'qty', headerName: 'Qty', width: 80, editable: true },
        { field: 'transactionCode', headerName: 'Trans Code', width: 100 },
        { field: 'transactionCodeDescription', headerName: 'Trans Desc', width: 200 },
        { field: 'document1Type', headerName: 'Doc1', width: 80 },
        { field: 'document1Number', headerName: 'Number', width: 80 },
        { field: 'document1Line', headerName: 'Line', width: 60 },
        {
            field: 'dateBooked',
            headerName: 'Booked',
            width: 110,
            renderCell: params =>
                params.row.dateBooked ? moment(params.row.dateBooked).format('DD-MMM-YYYY') : ''
        },
        { field: 'cancelled', headerName: 'Cancelled', width: 100 },
        {
            field: 'actions',
            headerName: 'Actions',
            width: 150,
            renderCell: params => {
                const canCancel =
                    !params.row.dateBooked &&
                    params.row.cancelled === 'N' &&
                    !params.row.isAddition;

                // just for now, only allowing stock pick for new rows onces
                // todo - consider other scenarions e.g. changing pick after picked initially
                const canPickStock =
                    params.row.stockAllocations &&
                    ((params.row.isAddition && !params.row.stockPicked) ||
                        (!params.row.isAdditon && !params.row.stockPicked && canCancel));
                return (
                    <>
                        {canPickStock && (
                            <Tooltip title="Pick Stock">
                                <IconButton
                                    onClick={() => {
                                        setSelected(params.row.lineNumber);
                                        setPickStockDialogVisible(true);
                                    }}
                                >
                                    <WarehouseIcon />
                                </IconButton>
                            </Tooltip>
                        )}
                        {canCancel && (
                            <Tooltip title="Cancel Line">
                                <IconButton
                                    onClick={() => {
                                        setSelected(params.row.lineNumber);
                                        setCancelDialogVisible(true);
                                    }}
                                >
                                    <CancelIcon />
                                </IconButton>
                            </Tooltip>
                        )}
                        {canBook && utilities.getHref(params.row, 'book-line') && (
                            <Tooltip title="Book Line">
                                <IconButton
                                    onClick={() => {
                                        setSelected(params.row.lineNumber);
                                        bookLine(params.row.lineNumber);
                                    }}
                                >
                                    <CheckCircleIcon />
                                </IconButton>
                            </Tooltip>
                        )}
                    </>
                );
            }
        }
    ];

    const processRowUpdate = updatedLine => {
        updateLine(updatedLine.lineNumber, 'qty', updatedLine.qty);
        return updatedLine;
    };

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

            <PartsSearchDialog
                searchDialogOpen={!!partsSearchDialogOpen}
                setSearchDialogOpen={setPartsSearchDialogOpen}
                handleSearchResultSelect={r => {
                    updateLine(partsSearchDialogOpen, 'part', r);
                }}
            />

            <PickStockDialog
                open={pickStockDialogVisible}
                setOpen={setPickStockDialogVisible}
                partNumber={lines.find(l => l.lineNumber === selected)?.part?.partNumber}
                quantity={lines.find(l => l.lineNumber === selected)?.qty}
                handleConfirm={moves => {
                    pickStock(selected, moves);
                }}
                state={fromState}
                stockPool={fromStockPool}
            />
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={lines}
                    columns={linesColumns}
                    processRowUpdate={processRowUpdate}
                    rowSelectionModel={selected ? [selected] : []}
                    onRowClick={row => {
                        setSelected(row.id);
                    }}
                    editMode="cell"
                    loading={false}
                    hideFooter
                />
            </Grid>
            <Grid size={12}>
                <Tooltip title="Add Line">
                    <IconButton disabled={!canAdd} onClick={addLine} color="primary" size="large">
                        <AddIcon />
                    </IconButton>
                </Tooltip>
            </Grid>
            {showPostings && (
                <Grid size={12}>
                    <Typography variant="h6">
                        {!selected
                            ? 'Click a row to view postings'
                            : `Postings for line ${selected}`}
                    </Typography>
                </Grid>
            )}
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

export default LinesTab;
