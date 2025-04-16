import React, { useEffect, useState } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import SnackbarContent from '@mui/material/SnackbarContent';
import Snackbar from '@mui/material/Snackbar';
import moment from 'moment';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';

function PickRequisitionDialog({ open, setOpen, handleSelect, documentType, documentNumber }) {
    const [snackbar, setSnackbar] = useState(null);
    const handleCloseSnackbar = () => setSnackbar(null);
    const [rowSelectionModel, setRowSelectionModel] = useState([]);
    const [requisitions, setRequisitions] = useState([]);

    const {
        send: searchReqs,
        isLoading: searchLoading,
        result: searchResult,
        clearData: clearSearch
    } = useGet(itemTypes.requisitions.url);

    useEffect(() => {
        if (open && documentType && documentNumber) {
            searchReqs(
                null,
                `?documentName=${documentType}&documentNumber=${documentNumber}&includeCancelled=false`
            );
        }
    }, [documentNumber, documentType, open, searchReqs]);

    useEffect(() => {
        if (searchResult) {
            setRequisitions(
                searchResult
                    .filter(a => a.isReversed === 'N' && a.isReverseTransaction === 'N')
                    .map(a => ({ ...a, id: a.reqNumber }))
            );
        }
    }, [searchResult]);

    const handleClose = () => {
        clearSearch();
        setRequisitions([]);
        setOpen(false);
    };

    const handleConfirmClick = () => {
        handleClose();
        if (rowSelectionModel && rowSelectionModel.length) {
            const selected = requisitions.find(a => a.id === rowSelectionModel[0]);
            handleSelect(selected);
        } else {
            handleSelect({ reqNumber: null });
        }
    };

    const columns = [
        {
            field: 'reqNumber',
            headerName: 'Req Number',
            width: 110
        },
        {
            field: 'dateBooked',
            headerName: 'Date Booked',
            width: 110,
            renderCell: params =>
                params.row.dateBooked ? moment(params.row.dateBooked).format('DD-MMM-YYYY') : ''
        },
        {
            field: 'document1',
            headerName: 'Doc 1',
            width: 110
        },
        {
            field: 'document1Line',
            headerName: 'Line',
            width: 100
        },
        {
            field: 'quantity',
            headerName: 'Qty',
            width: 110
        },
        {
            field: 'reference',
            headerName: 'Reference',
            width: 190
        }
    ];

    const handleRowSelection = rowSelectionModel => {
        setRowSelectionModel(rowSelectionModel);
    };

    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xl">
            <DialogTitle>
                Requistions for {documentType} {documentNumber}
            </DialogTitle>
            <DialogContent>
                <DataGrid
                    rows={requisitions ?? []}
                    columns={columns}
                    hideFooter
                    density="compact"
                    disableMultipleRowSelection
                    rowSelectionModel={rowSelectionModel}
                    onRowSelectionModelChange={newRowSelectionModel => {
                        handleRowSelection(newRowSelectionModel);
                    }}
                    loading={searchLoading}
                    checkboxSelection
                />
                {!!snackbar && (
                    <Snackbar
                        open
                        autoHideDuration={6000}
                        onClose={handleCloseSnackbar}
                        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
                    >
                        <SnackbarContent
                            style={{
                                backgroundColor: snackbar.backgroundColour,
                                color: 'black'
                            }}
                            message={snackbar.message}
                        />
                    </Snackbar>
                )}
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        handleConfirmClick();
                    }}
                >
                    Confirm
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default PickRequisitionDialog;
