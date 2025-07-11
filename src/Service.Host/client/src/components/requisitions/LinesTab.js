import React, { useState } from 'react';
import Grid from '@mui/material/Grid';
import { DataGrid, GridSearchIcon, useGridApiRef } from '@mui/x-data-grid';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
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
    fromStockPool,
    removeLine,
    reqHeader,
    transactionOptions = null,
    locationCodeRoot = null,
    changesMade = false
}) {
    const [cancelDialogVisible, setCancelDialogVisible] = useState(false);
    const [pickStockDialogVisible, setPickStockDialogVisible] = useState(false);
    const apiRef = useGridApiRef();

    const [partsSearchDialogOpen, setPartsSearchDialogOpen] = useState();

    const linesColumns = [
        { field: 'lineNumber', headerName: '#', width: 50 },
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
                        <span>{params.row.part?.partNumber}</span>
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
        {
            field: 'transactionCode',
            headerName: 'Trans Code',
            width: 100,
            editable: !!transactionOptions
        },
        {
            field: 'pick',
            headerName: 'Pick',
            width: 60,
            disableColumnMenu: true,
            renderCell: params => {
                const canCancel =
                    !params.row.dateBooked &&
                    params.row.cancelled === 'N' &&
                    params.row.part?.description &&
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
                    </>
                );
            }
        },
        { field: 'transactionCodeDescription', headerName: 'Trans Desc', width: 200 },
        { field: 'document1Type', headerName: 'Doc1', width: 80 },
        { field: 'document1Number', headerName: 'Number', width: 80 },
        { field: 'document1Line', headerName: 'Line', width: 60, editable: true },
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
            disableColumnMenu: true,
            renderCell: params => {
                const canCancel =
                    !params.row.dateBooked &&
                    params.row.cancelled === 'N' &&
                    !params.row.isAddition;
                const canRemove = params.row.isAddition;
                return (
                    <>
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
                        {canRemove && (
                            <Tooltip title="Remove Line">
                                <IconButton
                                    onClick={() => {
                                        removeLine(params.row.lineNumber);
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

    const processRowUpdate = (updatedLine, oldLine) => {
        if (updatedLine.qty !== oldLine.qty) {
            updateLine(updatedLine.lineNumber, 'qty', updatedLine.qty);
        }

        if (updatedLine.document1Line !== oldLine.document1Line) {
            updateLine(updatedLine.lineNumber, 'document1Line', updatedLine.document1Line);
        }

        if (
            (updatedLine.transactionCode &&
                updatedLine.transactionCode !== oldLine.transactionCode) ||
            updatedLine.qty !== oldLine.qty
        ) {
            let newCode = updatedLine.transactionCode.toUpperCase();

            if (newCode.length === 1) {
                if (newCode === 'a' || newCode === 'A') {
                    newCode = 'ADSTI';
                }

                if (newCode === 's' || newCode === 'S') {
                    newCode = 'STADI';
                }
            }

            const selectedOption = transactionOptions.find(
                a => a.transactionDefinition === newCode
            );
            if (selectedOption) {
                updatedLine.transactionCode = selectedOption.transactionDefinition;
                updatedLine.transactionCodeDescription = selectedOption.transactionDescription;
                updateLine(
                    updatedLine.lineNumber,
                    'transactionCode',
                    selectedOption.transactionDefinition
                );
                updateLine(
                    updatedLine.lineNumber,
                    'transactionCodeDescription',
                    selectedOption.transactionDescription
                );

                if (selectedOption.stockAllocations) {
                    updateLine(updatedLine.lineNumber, 'stockAllocations', true);
                    updatedLine.stockAllocations = true;
                    pickStock(updatedLine.lineNumber, [
                        {
                            partNumber: updatedLine.part?.partNumber,
                            isFrom: true,
                            isTo: false,
                            quantityToPick: updatedLine.qty,
                            palletNumber: reqHeader.fromPalletNumber,
                            locationName: reqHeader.fromLocationCode,
                            stockPoolCode: reqHeader.fromStockPool ?? 'LINN',
                            state: reqHeader.fromState ?? selectedOption.fromStates[0]
                        }
                    ]);
                } else {
                    updateLine(updatedLine.lineNumber, 'stockAllocations', false);
                    updatedLine.stockAllocations = false;
                    pickStock(updatedLine.lineNumber, [
                        {
                            partNumber: updatedLine.part?.partNumber,
                            isFrom: false,
                            isTo: true,
                            quantityToPick: updatedLine.qty,
                            palletNumber: reqHeader.fromPalletNumber,
                            locationName: reqHeader.fromLocationCode,
                            stockPoolCode: reqHeader.fromStockPool ?? 'LINN',
                            state: reqHeader.fromState ?? selectedOption.fromStates[0]
                        }
                    ]);
                }
            } else {
                updatedLine.transactionCode = oldLine.transactionCode;
            }
        }

        return updatedLine;
    };

    const handleCellKeyDown = (params, event) => {
        if (event.keyCode === 13) {
            if (params.colDef.field === 'partNumber') {
                setPartsSearchDialogOpen(true);
                apiRef.current.stopCellEditMode({ id: params.id, field: params.field });
            } else {
                return;
            }
        }
        if (event.key !== 'Tab') {
            return;
        }

        const rowIds = apiRef.current.getAllRowIds();

        const visibleColumns = apiRef.current.getVisibleColumns();
        const nextCell = {
            rowIndex: rowIds.findIndex(id => id === params.id),
            colIndex: apiRef.current.getColumnIndex(params.field)
        };

        if (
            nextCell.colIndex === visibleColumns.length - 1 &&
            nextCell.rowIndex === rowIds.length - 1 &&
            !event.shiftKey
        ) {
            // Do nothing if we are at the last cell of the last row
            return;
        }

        if (nextCell.colIndex === 0 && nextCell.rowIndex === 0 && event.shiftKey) {
            // Do nothing if we are at the first cell of the first row
            return;
        }

        event.preventDefault();

        if (!event.shiftKey) {
            if (nextCell.colIndex < visibleColumns.length - 1) {
                nextCell.colIndex += 1;
            } else {
                nextCell.rowIndex += 1;
                nextCell.colIndex = 0;
            }
        } else if (nextCell.colIndex > 0) {
            nextCell.colIndex -= 1;
        } else {
            nextCell.rowIndex -= 1;
            nextCell.colIndex = visibleColumns.length - 1;
        }
        apiRef.current.scrollToIndexes(nextCell);

        const { field } = visibleColumns[nextCell.colIndex];
        const id = rowIds[nextCell.rowIndex];
        apiRef.current.setCellFocus(id, field);
    };

    return (
        <Grid container spacing={3}>
            {cancelDialogVisible && (
                <CancelWithReasonDialog
                    visible={cancelDialogVisible}
                    title={`Enter a reason to cancel LINE ${selected}`}
                    closeDialog={() => setCancelDialogVisible(false)}
                    warningText={
                        changesMade
                            ? 'Warning: there are changes on this req!  Cancelling this req will discard changes so please save your changes first if you want to keep them.'
                            : ''
                    }
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
                    updateLine(partsSearchDialogOpen, 'qty', 0);
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
                locationCodeRoot={locationCodeRoot}
            />
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={lines}
                    columns={linesColumns}
                    processRowUpdate={processRowUpdate}
                    onCellKeyDown={handleCellKeyDown}
                    rowSelectionModel={
                        selected ? { type: 'include', ids: new Set([selected]) } : []
                    }
                    onRowClick={row => {
                        setSelected(row.id);
                    }}
                    loading={false}
                    apiRef={apiRef}
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
