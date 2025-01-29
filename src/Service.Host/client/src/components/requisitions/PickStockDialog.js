import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import useSearch from '../../hooks/useSearch';
import itemTypes from '../../itemTypes';
import useInitialise from '../../hooks/useInitialise';

function PickStockDialog({ open, setOpen, handleConfirm, partId }) {
    const handleClose = () => {
        setOpen(false);
    };
    console.log(open, partId);
    const [searchTerm, setSearchTerm] = useState();
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
    const { isLoading, result } = useInitialise(
        itemTypes.stockLocators.url,
        null,
        `?partId=${partId}`
    );
    console.log(result);
    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xl">
            <DialogTitle>Stock Locations</DialogTitle>
            <DialogContent></DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Close</Button>
            </DialogActions>
        </Dialog>
    );
}

PickStockDialog.propTypes = {
    open: PropTypes.bool.isRequired,
    setOpen: PropTypes.func.isRequired,
    handleConfirm: PropTypes.func.isRequired
};

export default PickStockDialog;
