import React, { useState, useEffect, useCallback } from 'react';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import { Dropdown, InputField } from '@linn-it/linn-form-components-library';
import useGet from '../../hooks/useGet';
import itemTypes from '../../itemTypes';

function OrderDetails({ orderDetails, canChange, handleFieldChange }) {
    const [dialogOpen, setDialogOpen] = useState({
        forRow: null
    });

    const {
        send: getRsn,
        isLoading: isRsnLoading,
        result: rsnResult,
        clearData: clearRsnData
    } = useGet(itemTypes.rsns.url, true);

    const handleOrderChange = useCallback(
        (propertyName, newValue) => {
            if (!dialogOpen.forRow) {
                return;
            }

            const updatedOrderDetails = orderDetails?.map(detail =>
                detail.lineNumber === dialogOpen.forRow
                    ? { ...detail, [propertyName]: newValue }
                    : detail
            );

            if (updatedOrderDetails) {
                handleFieldChange('importBookOrderDetails', updatedOrderDetails);
            }
        },
        [dialogOpen.forRow, orderDetails, handleFieldChange]
    );

    const orderDetail = dialogOpen.forRow
        ? orderDetails?.find(od => od.lineNumber === dialogOpen.forRow)
        : null;

    useEffect(() => {
        if (rsnResult && orderDetail?.rsnNumber) {
            // Batch all updates into a single call to prevent race conditions
            const updatedOrderDetails = orderDetails?.map(detail =>
                detail.lineNumber === dialogOpen.forRow
                    ? {
                          ...detail,
                          lineDocument: rsnResult.rsnNumber,
                          orderDescription: rsnResult.invoiceDescription,
                          tariffCode: rsnResult.tariffCode
                      }
                    : detail
            );

            if (updatedOrderDetails) {
                handleFieldChange('importBookOrderDetails', updatedOrderDetails);
            }

            clearRsnData();
        }
    }, [
        rsnResult,
        orderDetail?.rsnNumber,
        clearRsnData,
        dialogOpen.forRow,
        orderDetail,
        orderDetails,
        handleFieldChange
    ]);

    const addOrderDetail = () => {
        const lineNumber =
            orderDetails?.length > 0 ? orderDetails[orderDetails.length - 1].lineNumber + 1 : 1;

        setDialogOpen({
            forRow: lineNumber
        });

        if (orderDetails?.length > 0) {
            handleFieldChange('importBookOrderDetails', [
                ...orderDetails,
                { lineNumber: lineNumber, qty: 1, lineType: orderDetails[0].lineType }
            ]);
        } else {
            handleFieldChange('importBookOrderDetails', [
                {
                    lineNumber: lineNumber,
                    qty: 1,
                    lineType: 'SUNDRY',
                    orderValue: 0,
                    dutyValue: 0,
                    vatValue: 0
                }
            ]);
        }
    };

    const deleteOrderDetail = lineNumber => {
        const updatedOrderDetails = orderDetails.filter(detail => detail.lineNumber !== lineNumber);
        handleFieldChange('importBookOrderDetails', updatedOrderDetails);
    };

    const handleRowDoubleClick = params => {
        if (canChange) {
            setDialogOpen({ forRow: params.row.lineNumber });
        }
    };

    const handleRsnSelect = () => {
        if (orderDetail.rsnNumber && !isRsnLoading) {
            getRsn(orderDetail.rsnNumber);
        }
    };

    const documentSearch = () => {
        if (orderDetail.lineType === 'RSN') {
            return (
                <>
                    <Grid size={3}>
                        <InputField
                            fullWidth
                            value={orderDetail.rsnNumber}
                            type="number"
                            onChange={handleOrderChange}
                            label="RSN Number"
                            propertyName="rsnNumber"
                        />
                    </Grid>
                    <Grid size={9}>
                        <Button
                            onClick={handleRsnSelect}
                            variant="outlined"
                            style={{ marginTop: '29px' }}
                            disabled={!orderDetail.rsnNumber || isRsnLoading}
                        >
                            Add RSN
                        </Button>
                    </Grid>
                </>
            );
        }

        return null;
    };

    const columns = [
        { field: 'lineNumber', headerName: 'Line', width: 60 },
        { field: 'lineType', headerName: 'Line Type', width: 140 },
        { field: 'lineDocument', headerName: 'Document', width: 140 },
        { field: 'orderDescription', headerName: 'Description', width: 240 },
        { field: 'tariffCode', headerName: 'Tariff no', width: 100 },
        { field: 'orderValue', headerName: 'Customs Value', width: 140 },
        { field: 'dutyValue', headerName: 'Duty Value', width: 120 },
        { field: 'qty', headerName: 'Qty', width: 100 },
        ...(canChange
            ? [
                  {
                      field: 'actions',
                      headerName: '',
                      width: 60,
                      sortable: false,
                      filterable: false,
                      disableColumnMenu: true,
                      renderCell: params => (
                          <Tooltip title="Delete Order Detail Line">
                              <IconButton
                                  onClick={() => deleteOrderDetail(params.row.lineNumber)}
                                  color="error"
                                  size="small"
                              >
                                  <DeleteIcon />
                              </IconButton>
                          </Tooltip>
                      )
                  }
              ]
            : [])
    ];

    if (!orderDetails || orderDetails.length === 0) {
        return null;
    }

    return (
        <>
            {dialogOpen.forRow && orderDetail && (
                <Dialog
                    open={dialogOpen.forRow}
                    maxWidth="lg"
                    onClose={() => setDialogOpen({ forRow: null })}
                    sx={{
                        '& .MuiDialog-paper': {
                            width: '80%',
                            maxWidth: '800px',
                            minWidth: '400px'
                        }
                    }}
                >
                    <DialogTitle>Edit Order Line {orderDetail.lineNumber} Detail</DialogTitle>
                    <DialogContent>
                        <Grid container spacing={1}>
                            <Grid size={6}>
                                <Dropdown
                                    value={orderDetail?.lineType}
                                    fullWidth
                                    label="Line Type"
                                    propertyName="lineType"
                                    allowNoValue
                                    items={['RSN', 'PO', 'RETURNS', 'SUNDRY', 'SAMPLES']}
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={orderDetail?.qty}
                                    type="number"
                                    label="Qty"
                                    propertyName="qty"
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                            {documentSearch()}
                            <Grid size={12}>
                                <InputField
                                    value={orderDetail?.orderDescription}
                                    fullWidth
                                    label="Order Description"
                                    propertyName="orderDescription"
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={orderDetail?.tariffCode}
                                    fullWidth
                                    label="Tariff Code"
                                    propertyName="tariffCode"
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={orderDetail?.orderValue}
                                    fullWidth
                                    type="number"
                                    label="Customs Value"
                                    propertyName="orderValue"
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={orderDetail?.dutyValue}
                                    fullWidth
                                    type="number"
                                    label="Duty Value"
                                    propertyName="dutyValue"
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={orderDetail?.vatValue}
                                    fullWidth
                                    type="number"
                                    label="VAT Value"
                                    propertyName="vatValue"
                                    onChange={handleOrderChange}
                                />
                            </Grid>
                        </Grid>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setDialogOpen({ forRow: null })}>Close</Button>
                    </DialogActions>
                </Dialog>
            )}
            <Grid size={12}>
                <Typography variant="subtitle2">Order Details</Typography>
            </Grid>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={orderDetails}
                    columns={columns}
                    density="compact"
                    hideFooter
                    onRowDoubleClick={handleRowDoubleClick}
                    disableColumnMenu
                    disableColumnSorting
                />
            </Grid>
            {canChange && (
                <>
                    <Grid size={3}>
                        <Tooltip title="Add Order Detail Line">
                            <IconButton onClick={addOrderDetail} color="primary" size="large">
                                <AddIcon />
                            </IconButton>
                        </Tooltip>
                    </Grid>
                    <Grid size={9}>
                        <Typography variant="subtitle2">
                            Note: Double Click on order details lines to edit them.
                        </Typography>
                    </Grid>
                </>
            )}
        </>
    );
}

export default OrderDetails;
