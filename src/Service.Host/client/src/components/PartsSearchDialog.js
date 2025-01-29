import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import { Search } from '@linn-it/linn-form-components-library';
import useSearch from '../hooks/useSearch';
import itemTypes from '../itemTypes';

function PartsSearchDialog({ searchDialogOpen, setSearchDialogOpen, handleSearchResultSelect }) {
    const handleClose = () => {
        setSearchDialogOpen(false);
    };
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
    const { search, results, loading, clear } = useSearch(
        itemTypes.parts.url,
        'partNumber',
        'partNumber',
        'description'
    );
    return (
        <Dialog open={searchDialogOpen} onClose={handleClose}>
            <DialogTitle>Search</DialogTitle>
            <DialogContent>
                <Search
                    autoFocus
                    propertyName="partSearchTerm"
                    label="Part Number"
                    // resultsInModal
                    resultLimit={100}
                    value={searchTerm}
                    loading={loading}
                    handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                    search={search}
                    searchResults={results}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={r => {
                        setSearchDialogOpen(false);
                        handleSearchResultSelect(r);
                    }}
                    clearSearch={clear}
                />
            </DialogContent>
        </Dialog>
    );
}

PartsSearchDialog.propTypes = {
    searchDialogOpen: PropTypes.bool.isRequired,
    setSearchDialogOpen: PropTypes.func.isRequired,
    handleSearchResultSelect: PropTypes.func.isRequired
};

export default PartsSearchDialog;
