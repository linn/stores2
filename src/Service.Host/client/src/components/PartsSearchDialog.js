import React, { useEffect, useState } from 'react';
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

    const [isSelectingPart, setIsSelectingPart] = useState(false);

    const handlePartSelect = () => {
        if (searchTerm) {
            setIsSelectingPart(true);
            search(searchTerm.trim().toUpperCase());
        }
    };

    useEffect(() => {
        if (results?.length === 1 && isSelectingPart) {
            handleSearchResultSelect(results[0]);
            clear();
            setIsSelectingPart(false);
            setSearchDialogOpen(false);
        }
    }, [
        results,
        handleSearchResultSelect,
        clear,
        isSelectingPart,
        setIsSelectingPart,
        setSearchDialogOpen
    ]);

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
                    helperText="<Enter> to search or <Tab> to select if you have entered a known part number"
                    onKeyPressFunctions={[{ keyCode: 9, action: handlePartSelect }]}
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

export default PartsSearchDialog;
