import React, { useEffect, useState, useCallback } from 'react';
import moment from 'moment';
import Grid from '@mui/material/Grid';
import { Search, utilities } from '@linn-it/linn-form-components-library';
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
import UnpickMoveDialog from './components/UnpickMoveDialog';

function MovesTab({
    moves = [],
    addMoveOnto = null,
    addMove = null,
    updateMoveOnto = null,
    stockPools = [],
    stockStates = [],
    locationCodeRoot = null,
    unpick,
    changesMade = false
}) {
    const [searchDialogOpen, setSearchDialogOpen] = useState({
        forRow: null
    });

    const [unpickDialogVisible, setUnpickDialogVisible] = useState(false);
    const [selectedMove, setSelectedMove] = useState(null);

    const palletFormat = Intl.NumberFormat('en-GB', {
        useGrouping: false
    });

    const {
        search: searchLocations,
        results: locationsSearchResults,
        loading: locationsSearchLoading,
        clear: clearLocationsSearch
    } = useSearch(itemTypes.storageLocations.url, 'locationId', 'locationCode', 'description');

    const [searchTerm, setSearchTerm] = useState(locationCodeRoot);

    const [isSelectingLocation, setIsSelectingLocation] = useState(false);

    const handleLocationSelect = () => {
        if (searchTerm) {
            setIsSelectingLocation(true);
            searchLocations(searchTerm.trim().toUpperCase());
        }
    };

    const showUnpickDialog = move => {
        setSelectedMove(move);
        setUnpickDialogVisible(true);
    };

    const handleSearchResultSelect = useCallback(
        selected => {
            const currentRow = moves.find(r => r.seq === searchDialogOpen.forRow);
            const newRow = {
                ...currentRow,
                toLocationCode: selected.name,
                toLocationDescription: selected.description
            };
            updateMoveOnto(newRow);
            setSearchDialogOpen({ forRow: null });
        },
        [moves, searchDialogOpen, setSearchDialogOpen, updateMoveOnto]
    );

    useEffect(() => {
        if (locationsSearchResults?.length === 1 && isSelectingLocation) {
            handleSearchResultSelect(locationsSearchResults[0]);
            clearLocationsSearch();
            setIsSelectingLocation(false);
            setSearchDialogOpen({ forRow: null });
        }
    }, [
        locationsSearchResults,
        handleSearchResultSelect,
        clearLocationsSearch,
        isSelectingLocation,
        setIsSelectingLocation
    ]);

    const renderSearchDialog = () => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null });
        };

        return (
            <Dialog open={searchDialogOpen.forRow} onClose={handleClose}>
                <DialogTitle>Search</DialogTitle>
                <DialogContent>
                    <Search
                        autoFocus
                        propertyName="defaultLocation"
                        label="Search Location"
                        resultLimit={100}
                        value={searchTerm}
                        onKeyPressFunctions={[{ keyCode: 9, action: handleLocationSelect }]}
                        helperText="<Enter> to search or <Tab> to select if you have entered a known location code"
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
            width: 150,
            renderCell: params =>
                params.row.dateBooked ? moment(params.row.dateBooked).format('DD-MMM-YYYY') : ''
        },
        {
            field: 'dateCancelled',
            headerName: 'Cancelled?',
            width: 100,
            renderCell: params =>
                params.row.dateCancelled
                    ? moment(params.row.dateCancelled).format('DD-MMM-YYYY')
                    : ''
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
            width: 150
        },
        {
            field: 'fromLocationDescription',
            headerName: 'Loc Desc',
            width: 150
        },
        {
            field: 'fromPalletNumber',
            headerName: 'Pallet',
            width: 120,
            type: 'number',
            valueFormatter: value => {
                if (value == null) {
                    return '';
                }

                return palletFormat.format(value);
            }
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
            renderCell: params =>
                params.row.fromBatchDate
                    ? moment(params.row.fromBatchDate).format('DD-MMM-YYYY')
                    : ''
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
        },
        {
            field: 'unpick',
            headerName: '',
            width: 80,
            renderCell: params => (
                <Button
                    size="small"
                    disabled={!utilities.getHref(params.row, 'unpick')}
                    onClick={() => showUnpickDialog(params.row)}
                >
                    Unpick
                </Button>
            )
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
            width: 150,
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
            width: 120,
            type: 'number',
            valueFormatter: value => {
                if (value == null) {
                    return '';
                }

                return palletFormat.format(value);
            }
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
            {unpickDialogVisible && (
                <UnpickMoveDialog
                    visible={unpickDialogVisible}
                    move={selectedMove}
                    closeDialog={() => setUnpickDialogVisible(false)}
                    warningText={
                        changesMade
                            ? 'Warning: there are changes on this req!  Unpicking this move will discard changes so please save your changes first if you want to keep them.'
                            : ''
                    }
                    doUnpick={unpick}
                />
            )}
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
                <Grid size={3}>
                    <Tooltip title="Add Line">
                        <IconButton
                            disabled={!addMoveOnto && !addMove}
                            onClick={addMoveOnto ? addMoveOnto : addMove}
                            color="primary"
                            size="large"
                        >
                            <AddIcon />
                        </IconButton>
                    </Tooltip>
                </Grid>
                <Grid size={9}>
                    <Typography variant="subtitle1">
                        Note: adding moves will update the line qty
                    </Typography>
                </Grid>
            </Grid>
        </>
    );
}

export default MovesTab;
