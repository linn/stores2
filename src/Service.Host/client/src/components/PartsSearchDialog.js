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
    const { search, results, loading, clear } = useSearch(
        itemTypes.parts.url,
        'id',
        'partNumber',
        'description'
    );
    return (
        <Dialog open={searchDialogOpen} onClose={handleClose} fullWidth maxWidth="md">
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
