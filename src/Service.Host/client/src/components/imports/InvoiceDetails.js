import React, { useState, useCallback } from 'react';
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
import { InputField } from '@linn-it/linn-form-components-library';

function InvoiceDetails({ invoiceDetails, canChange, handleFieldChange }) {
    const [dialogOpen, setDialogOpen] = useState({
        forRow: null
    });

    const handleInvoiceChange = useCallback(
        (propertyName, newValue) => {
            if (!dialogOpen.forRow) {
                return;
            }

            const updatedInvoiceDetails = invoiceDetails?.map(detail =>
                detail.lineNumber === dialogOpen.forRow
                    ? { ...detail, [propertyName]: newValue }
                    : detail
            );

            if (updatedInvoiceDetails) {
                handleFieldChange('importBookInvoiceDetails', updatedInvoiceDetails);
            }
        },
        [dialogOpen.forRow, invoiceDetails, handleFieldChange]
    );

    const invoiceDetail = dialogOpen.forRow
        ? invoiceDetails?.find(od => od.lineNumber === dialogOpen.forRow)
        : null;

    const addInvoiceDetail = () => {
        const lineNumber =
            invoiceDetails?.length > 0
                ? invoiceDetails[invoiceDetails.length - 1].lineNumber + 1
                : 1;

        setDialogOpen({
            forRow: lineNumber
        });

        if (invoiceDetails?.length > 0) {
            handleFieldChange('importBookInvoiceDetails', [
                ...invoiceDetails,
                { lineNumber: lineNumber }
            ]);
        } else {
            handleFieldChange('importBookInvoiceDetails', [
                {
                    lineNumber: lineNumber
                }
            ]);
        }
    };

    const deleteInvoiceDetail = lineNumber => {
        const updatedInvoiceDetails = invoiceDetails.filter(
            detail => detail.lineNumber !== lineNumber
        );
        handleFieldChange('importBookInvoiceDetails', updatedInvoiceDetails);
    };

    const handleRowDoubleClick = params => {
        if (canChange) {
            setDialogOpen({ forRow: params.row.lineNumber });
        }
    };

    const columns = [
        { field: 'lineNumber', headerName: 'Line', width: 50 },
        { field: 'invoiceNumber', headerName: 'Invoice Number', width: 150 },
        { field: 'invoiceValue', headerName: 'Invoice Value', width: 140 },
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
                          <Tooltip title="Delete Invoice Detail Line">
                              <IconButton
                                  onClick={() => deleteInvoiceDetail(params.row.lineNumber)}
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

    return (
        <>
            {dialogOpen.forRow && invoiceDetail && (
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
                    <DialogTitle>Edit Invoice Line {invoiceDetail.lineNumber} Detail</DialogTitle>
                    <DialogContent>
                        <Grid container spacing={1}>
                            <Grid size={12}>
                                <InputField
                                    value={invoiceDetail?.invoiceNumber}
                                    disabled={!canChange}
                                    fullWidth
                                    label="Invoice Number"
                                    propertyName="invoiceNumber"
                                    onChange={handleInvoiceChange}
                                />
                            </Grid>
                            <Grid size={4}>
                                <InputField
                                    value={invoiceDetail?.invoiceValue}
                                    disabled={!canChange}
                                    fullWidth
                                    type="number"
                                    label="Invoice Value"
                                    propertyName="invoiceValue"
                                    onChange={handleInvoiceChange}
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
                <Typography variant="subtitle2">Invoice Details</Typography>
            </Grid>
            <Grid size={12}>
                <DataGrid
                    getRowId={r => r.lineNumber}
                    rows={invoiceDetails}
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
                        <Tooltip title="Add Invoice Detail Line">
                            <IconButton onClick={addInvoiceDetail} color="primary" size="large">
                                <AddIcon />
                            </IconButton>
                        </Tooltip>
                    </Grid>
                    <Grid size={9}>
                        <Typography variant="subtitle2">
                            Note: Double Click on invoice details lines to edit them.
                        </Typography>
                    </Grid>
                </>
            )}
        </>
    );
}

export default InvoiceDetails;
