import React, { useState } from 'react';
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
            width: 100
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
    // const handleSearchResultSelect = selected => {
    //     const currentRow = stockPools?.find(r => r.id === searchDialogOpen.forRow);
    //     let newRow = {
    //         ...currentRow,
    //         updated: true,
    //         defaultLocation: selected.locationId,
    //         defaultLocationName: selected.locationCode
    //     };
    //     c.searchUpdateFieldNames?.forEach(f => {
    //         newRow = { ...newRow, [f.fieldName]: selected[f.searchResultFieldName] };
    //     });

    //     processRowUpdate(newRow, currentRow);
    //     setSearchDialogOpen({ forRow: null, forColumn: null });
    // };

    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xl">
            <DialogTitle>Stock Locations</DialogTitle>
            <DialogContent>
                <DataGrid
                    rows={result}
                    // rowSelectionModel={selected ? [selected] : []}
                    // onRowClick={row => {
                    //     setSelected(row.id);
                    // }}
                    columns={columns}
                    hideFooter
                    density="compact"
                    editMode="cell"
                    loading={isLoading}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Close</Button>
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
