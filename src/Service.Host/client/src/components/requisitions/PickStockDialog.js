import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Dialog from '@mui/material/Dialog';
import { DataGrid } from '@mui/x-data-grid';

import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import itemTypes from '../../itemTypes';
import useInitialise from '../../hooks/useInitialise';

function PickStockDialog({ open, setOpen, handleConfirm, partNumber }) {
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
            setMoves(result);
        }
    }, [result, moves]);

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
                />
            </DialogContent>
            <DialogActions>
                <Button
                    disabled={!picks?.length}
                    onClick={() => {
                        console.log(moves);
                        handleClose();
                        handleConfirm(picks);
                    }}
                >
                    Confirm
                </Button>
            </DialogActions>
        </Dialog>
    );
}

PickStockDialog.propTypes = {
    open: PropTypes.bool.isRequired,
    setOpen: PropTypes.func.isRequired,
    handleConfirm: PropTypes.func.isRequired,
    partNumber: PropTypes.string.isRequired
};

export default PickStockDialog;
