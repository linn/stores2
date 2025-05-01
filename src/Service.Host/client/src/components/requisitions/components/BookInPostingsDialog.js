import React, { useEffect, useState } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import SnackbarContent from '@mui/material/SnackbarContent';
import Snackbar from '@mui/material/Snackbar';
import moment from 'moment';
import { InputField } from '@linn-it/linn-form-components-library';
import itemTypes from '../../../itemTypes';
import useGet from '../../../hooks/useGet';

function BookInPostingsDialog({
    open,
    setOpen,
    handleSelect,
    documentType,
    documentNumber,
    documentLine,
    orderDetail
}) {
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

    const detailPostingColumns = [
        {
            field: 'qty',
            headerName: 'Qty',
            width: 110
        },
        {
            field: 'department',
            headerName: 'Department',
            width: 110
        },
        {
            field: 'departmentDescription',
            headerName: 'Desc',
            width: 210
        },
        {
            field: 'nominal',
            headerName: 'Nominal',
            width: 110
        },
        {
            field: 'nominalDescription',
            headerName: 'Desc',
            width: 210
        }
    ];

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

    const postingRows = [
        {
            ...orderDetail.orderPosting,
            id: orderDetail.orderPosting.lineNumber,
            department: orderDetail.orderPosting.nominalAccount.department.departmentCode,
            departmentDescription:
                orderDetail.orderPosting.nominalAccount.department.departmentDescription,
            nominal: orderDetail.orderPosting.nominalAccount.nominal.nominalCode,
            nominalDescription: orderDetail.orderPosting.nominalAccount.nominal.description
        }
    ];
    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xl">
            <DialogTitle>
                Book In Postings for {documentType} {documentNumber} / {documentLine}
            </DialogTitle>
            <DialogContent>
                <Grid container spacing={3}>
                    <Grid size={12}>
                        <Typography variant="h6">Order Line</Typography>
                    </Grid>
                    <Grid size={2}>
                        <InputField
                            fullWidth
                            value={orderDetail.orderNumber}
                            onChange={() => {}}
                            label="Order Number"
                            disabled
                            propertyName="orderNumber"
                        />
                    </Grid>
                    <Grid size={1}>
                        <InputField
                            fullWidth
                            value={orderDetail.line}
                            onChange={() => {}}
                            label="Line"
                            disabled
                            propertyName="line"
                        />
                    </Grid>
                    <Grid size={1}>
                        <InputField
                            fullWidth
                            value={orderDetail.ourQty}
                            onChange={() => {}}
                            label="Qty"
                            disabled
                            propertyName="ourQty"
                        />
                    </Grid>
                    <Grid size={2}>
                        <InputField
                            fullWidth
                            value={orderDetail.line}
                            onChange={() => {}}
                            label="Booked In"
                            disabled
                            propertyName="bookedIn"
                        />
                    </Grid>
                    <Grid size={2}>
                        <InputField
                            fullWidth
                            value={orderDetail.partNumber}
                            onChange={() => {}}
                            label="Part"
                            disabled
                            propertyName="partNumber"
                        />
                    </Grid>
                    <Grid size={4}>
                        <InputField
                            fullWidth
                            value={orderDetail.partDescription}
                            onChange={() => {}}
                            label="Description"
                            disabled
                            propertyName="partDescription"
                        />
                    </Grid>
                    <Grid size={12}>
                        <DataGrid
                            rows={postingRows ?? []}
                            columns={detailPostingColumns}
                            hideFooter
                            density="compact"
                        />
                    </Grid>
                    <Grid size={12}>
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
                    </Grid>
                </Grid>
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

export default BookInPostingsDialog;
