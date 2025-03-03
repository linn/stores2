import React, { useState } from 'react';
import moment from 'moment';
import Grid from '@mui/material/Grid2';
import { Search } from '@linn-it/linn-form-components-library';
import { DataGrid, GridSearchIcon } from '@mui/x-data-grid';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import AddIcon from '@mui/icons-material/Add';
import Button from '@mui/material/Button';
import useSearch from '../../hooks/useSearch';
import itemTypes from '../../itemTypes';

function MovesTab({
    moves = [],
    addMoveOnto = null,
    updateMoveOnto = null,
    stockPools = [],
    stockStates = []
}) {
    const [searchDialogOpen, setSearchDialogOpen] = useState({
        forRow: null
    });

    const {
        search: searchLocations,
        results: locationsSearchResults,
        loading: locationsSearchLoading,
        clear: clearLocationsSearch
    } = useSearch(itemTypes.storageLocations.url, 'locationId', 'locationCode', 'description');

    const [searchTerm, setSearchTerm] = useState();

    const renderSearchDialog = () => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = moves.find(r => r.seq === searchDialogOpen.forRow);
            const newRow = {
                ...currentRow,
                toLocationCode: selected.name,
                toLocationDescription: selected.description
            };

            processRowUpdate(newRow);
            setSearchDialogOpen({ forRow: null });
        };

        return (
            <Dialog open={searchDialogOpen.forRow} onClose={handleClose}>
                <DialogTitle>Search</DialogTitle>
                <DialogContent>
                    <Search
                        autoFocus
                        propertyName="defaultLocation"
                        label="defaultLocation"
                        resultsInModal
                        resultLimit={100}
                        value={searchTerm}
                        loading={locationsSearchLoading}
                        handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                        search={searchLocations}
                        searchResults={locationsSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleSearchResultSelect}
                        clearSearch={clearLocationsSearch}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Close</Button>
                </DialogActions>
            </Dialog>
        );
    };

    const headerColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 100
        },
        {
            field: 'part',
            headerName: 'Part',
            width: 150
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 150
        },
        {
            field: 'dateBooked',
            headerName: 'Booked?',
            width: 150
        },
        {
            field: 'dateCancelled',
            headerName: 'Cancelled?',
            width: 100
        }
    ];

    const fromColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 80
        },
        {
            field: 'fromLocationCode',
            headerName: 'Loc Code',
            width: 100
        },
        {
            field: 'fromLocationDescription',
            headerName: 'Loc Desc',
            width: 150
        },
        {
            field: 'fromPalletNumber',
            headerName: 'Pallet',
            width: 150,
            type: 'number'
        },
        {
            field: 'fromStockPool',
            headerName: 'Stock Pool',
            width: 120
        },
        {
            field: 'fromState',
            headerName: 'State',
            width: 100
        },
        {
            field: 'fromBatchRef',
            headerName: 'Batch Ref',
            width: 100
        },
        {
            field: 'fromBatchDate',
            headerName: 'Batch Date',
            width: 120,
            renderCell: params => moment(params.row.fromBatchDate).format('DD-MMM-YYYY')
        },
        {
            field: 'qtyAtLocation',
            headerName: 'At Location',
            width: 100
        },
        {
            field: 'qtyAllocated',
            headerName: 'Allocated',
            width: 100
        }
    ];

    const toColumns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 80
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 100,
            editable: true,
            type: 'number'
        },
        {
            field: 'toLocationCode',
            headerName: 'Loc Code',
            width: 100,
            renderCell: params => (
                <>
                    <GridSearchIcon
                        style={{ cursor: 'pointer' }}
                        onClick={() =>
                            setSearchDialogOpen({
                                forRow: params.id
                            })
                        }
                    />
                    {params.value}
                </>
            )
        },
        {
            field: 'toLocationDescription',
            headerName: 'Loc Desc',
            width: 150
        },
        {
            field: 'toPalletNumber',
            headerName: 'Pallet',
            editable: true,
            width: 150,
            type: 'number'
        },
        {
            field: 'toStockPool',
            headerName: 'Stock Pool',
            width: 120,
            editable: true,
            valueOptions: stockPools?.map(s => s.stockPoolCode),
            type: 'singleSelect'
        },
        {
            field: 'toState',
            headerName: 'State',
            width: 100,
            editable: true,
            valueOptions: stockStates?.map(s => s.state),
            type: 'singleSelect'
        },
        {
            field: 'serialNumber',
            headerName: 'Serial Num',
            width: 100
        },
        {
            field: 'remarks',
            headerName: 'Remarks',
            width: 100
        }
    ];

    const processRowUpdate = updated => {
        updateMoveOnto(updated);
        return updated;
    };

    return (
        <>
            {searchDialogOpen.forRow && renderSearchDialog()}
            <Grid container spacing={3}>
                <Grid size={6}>
                    <DataGrid
                        getRowId={r => r.seq}
                        rows={moves}
                        hideFooter
                        columns={headerColumns}
                        density="compact"
                        editMode="cell"
                        loading={false}
                    />
                </Grid>
                <Grid size={6} />
                <Grid size={12}>
                    <Typography variant="h6">From</Typography>
                </Grid>
                <Grid size={12}>
                    <DataGrid
                        getRowId={r => r.seq}
                        rows={moves.filter(x => x.isFrom)}
                        hideFooter
                        columns={fromColumns}
                        density="compact"
                        editMode="cell"
                        loading={false}
                    />
                </Grid>
                <Grid size={12}>
                    <Typography variant="h6">Onto</Typography>
                </Grid>
                <Grid size={12}>
                    <DataGrid
                        getRowId={r => r.seq}
                        rows={moves.filter(x => x.isTo)}
                        hideFooter
                        columns={toColumns}
                        processRowUpdate={processRowUpdate}
                        density="compact"
                        editMode="cell"
                        loading={false}
                    />
                </Grid>
                <Grid size={12}>
                    <Tooltip title="Add Line">
                        <IconButton
                            disabled={!addMoveOnto}
                            onClick={addMoveOnto}
                            color="primary"
                            size="large"
                        >
                            <AddIcon />
                        </IconButton>
                    </Tooltip>
                </Grid>
            </Grid>
        </>
    );
}

export default MovesTab;
