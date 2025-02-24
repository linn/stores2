import React, { useEffect, useState } from 'react';
import { DataGrid } from '@mui/x-data-grid';

import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import { Typography } from '@mui/material';
import SnackbarContent from '@mui/material/SnackbarContent';
import Snackbar from '@mui/material/Snackbar';
import itemTypes from '../../itemTypes';
import useInitialise from '../../hooks/useInitialise';

function PickStockDialog({ open, setOpen, handleConfirm, partNumber, quantity }) {
    const [snackbar, setSnackbar] = useState(null);
    const handleCloseSnackbar = () => setSnackbar(null);
    const handleClose = () => {
        setOpen(false);
    };
    const { isLoading, result } = useInitialise(
        itemTypes.stockLocators.url,
        null,
        `?partNumber=${partNumber}`,
        true
    );
    const [moves, setMoves] = useState([]);
    useEffect(() => {
        if (result?.length && !moves?.length) {
            setMoves(result.map((x, i) => ({ ...x, id: i })));
        }
    }, [result, moves]);

    const handleConfirmClick = picks => {
        if (quantity) {
            var totalQtyPicked = picks.reduce((a, b) => {
                return a + b.quantityToPick;
            }, 0);
            if (Number(totalQtyPicked) !== Number(quantity)) {
                setSnackbar({
                    message: `Quantity required is ${quantity}. Qty picked is ${totalQtyPicked}`,
                    backgroundColour: 'red'
                });
            } else {
                handleClose();
                handleConfirm(picks);
            }
        } else {
            handleClose();
            handleConfirm(picks);
        }
    };

    const columns = [
        {
            field: 'partNumber',
            headerName: 'Part',
            width: 100
        },
        {
            field: 'partDescription',
            headerName: 'Desc',
            width: 200
        },
        {
            field: 'partUnitOfMeasure',
            headerName: 'UOM',
            width: 100
        },
        {
            field: 'locationName',
            headerName: 'Location',
            width: 120
        },
        {
            field: 'palletNumber',
            headerName: 'Pallet',
            width: 100
        },
        {
            field: 'quantity',
            headerName: 'Qty',
            width: 100,
            type: 'number'
        },
        {
            field: 'quantityToPick',
            headerName: 'Qty To Pick',
            width: 100,
            type: 'number',
            editable: true
        },
        {
            field: 'quantityAllocated',
            headerName: 'Allocated',
            width: 100
        },
        {
            field: 'state',
            headerName: 'State',
            width: 100
        },
        {
            field: 'stockPoolCode',
            headerName: 'Stock Pool',
            width: 110
        }
    ];

    const processRowUpdate = newRow => {
        setMoves(m => m.map(x => (x.id === newRow.id ? newRow : x)));
        return newRow;
    };

    const picks = moves.filter(m => m.quantityToPick);

    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xl">
            <DialogTitle>Stock Locations</DialogTitle>
            <DialogContent>
                <DataGrid
                    rows={moves ?? []}
                    processRowUpdate={processRowUpdate}
                    columns={columns}
                    hideFooter
                    density="compact"
                    editMode="cell"
                    loading={isLoading}
                    initialState={{
                        columns: {
                            columnVisibilityModel: {
                                partUnitOfMeasure: false
                            }
                        }
                    }}
                />
                <Typography>
                    Note: your stock will not be allocated until you click save on the main form
                </Typography>
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
                    disabled={!picks?.length}
                    onClick={() => {
                        handleConfirmClick(picks);
                    }}
                >
                    Confirm
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default PickStockDialog;
