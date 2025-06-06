import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Dialog from '@mui/material/Dialog';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import Button from '@mui/material/Button';
import { DataGrid, GridSearchIcon } from '@mui/x-data-grid';
import {
    Dropdown,
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons,
    Search,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import useSearch from '../hooks/useSearch';
import Page from './Page';

function Workstation({ creating }) {
    const { isLoading } = useInitialise(itemTypes.workStations.url);
    const [workStation, setWorkStation] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);
    const [rowUpdated, setRowUpdated] = useState();
    const [changesMade, setChangesMade] = useState(false);
    const [employeeSearchTerm, setEmployeeSearchTerm] = useState('');
    const [storageLocationSearchTerm, setStorageLocationSearchTerm] = useState('');

    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const code = queryParams.get('code');

    const navigate = useNavigate();

    const {
        search: searchStorageLocations,
        results: storageLocationsSearchResults,
        loading: storageLocationsSearchLoading,
        clear: clearStorageLocation
    } = useSearch(itemTypes.storageLocations.url, 'locationId', 'locationCode', 'description');

    const {
        send: updateWorkStation,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult,
        clearPutResult: clearUpdateResult
    } = usePut(itemTypes.workStations.url, true);

    const {
        send: createWorkStation,
        createWorkStationLoading,
        errorMessage: createWorkStationError,
        postResult: createWorkStationResult,
        clearPostResult: clearCreateWorkStation
    } = usePost(itemTypes.workStations.url);

    const {
        send: getNewWorkStations,
        isNewWorkStationsLoading,
        result: newWorkStationsGetResult
    } = useGet(itemTypes.workStations.url);

    const {
        send: getCitCodes,
        isCitCodesLoading,
        result: citCodesGetResult
    } = useGet(itemTypes.citCodes.url);

    const {
        search: searchEmployees,
        results: employeesSearchResults,
        loading: employeesSearchLoading,
        clear: clearEmployeesSearch
    } = useSearch(itemTypes.historicEmployees.url, 'id', 'fullName', 'fullName', false, true);

    const [hasFetched, setHasFetched] = useState(false);

    const handleFieldChange = (propertyName, newValue) => {
        setWorkStation(current => ({ ...current, [propertyName]: newValue }));
        setChangesMade(true);
    };

    useEffect(() => {
        if (updateResult) {
            setWorkStation(updateResult);
        }
    }, [updateResult]);

    useEffect(() => {
        if (updateResult || createWorkStationResult) {
            getNewWorkStations();
            setSnackbarVisible(true);
            clearCreateWorkStation();
            clearUpdateResult();
        }
    }, [
        clearCreateWorkStation,
        clearUpdateResult,
        createWorkStationResult,
        getNewWorkStations,
        updateResult
    ]);

    useEffect(() => {
        if (!hasFetched) {
            setHasFetched(true);

            if (!creating) {
                getNewWorkStations(encodeURI(code));
            }

            getCitCodes();
        }
    }, [creating, hasFetched, getNewWorkStations, code, getCitCodes]);

    useEffect(() => {
        if (!workStation && newWorkStationsGetResult) {
            setWorkStation(newWorkStationsGetResult);
        }
    }, [workStation, newWorkStationsGetResult]);

    const addNewRow = () => {
        setWorkStation(prev => ({
            ...prev,
            workStationElements: [
                ...(prev.workStationElements || []),
                {
                    workStationElementId: workStation.workStationElements.length + 1,
                    workStationCode: workStation?.workStationCode || '',
                    createdBy: null,
                    createdByName: '',
                    dateCreated: new Date(),
                    locationId: 0,
                    palletNumber: null,
                    isAddition: true
                }
            ]
        }));
        setChangesMade(true);
    };

    const handleDeleteRow = params => {
        setWorkStation(prev => ({
            ...prev,
            workStationElements: prev.workStationElements.filter(
                element => element.workStationElementId !== params.workStationElementId
            )
        }));
        setChangesMade(true);
    };

    const handleCancelSelect = () => {
        const oldRow = newWorkStationsGetResult?.workStationElements?.find(
            ws => ws.workStationElementId === rowUpdated
        );

        setWorkStation(prev => ({
            ...prev,
            workStationElements: prev?.workStationElements.map(e =>
                e.workStationElementId === oldRow?.workStationElementId ? oldRow : e
            )
        }));

        setRowUpdated(null);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };

        setWorkStation(prev => ({
            ...prev,
            workStationElements: prev.workStationElements.map(row =>
                row.workStationElementId === newRow.workStationElementId ? updatedRow : row
            )
        }));

        setRowUpdated(newRow.workStationElementId);
        setChangesMade(true);

        return newRow;
    };

    const [searchDialogOpen, setSearchDialogOpen] = useState({
        forRow: null,
        forColumn: null
    });

    const workStationElementColumns = [
        {
            field: 'locationId',
            headerName: 'Storage Location',
            width: 150,
            editable: true,
            type: 'search',
            search: searchStorageLocations,
            searchResults: storageLocationsSearchResults,
            searchLoading: storageLocationsSearchLoading,
            searchUpdateFieldNames: [
                { fieldName: 'locationId', searchResultFieldName: 'locationId' },
                { fieldName: 'locationCode', searchResultFieldName: 'locationCode' },
                { fieldName: 'locationDescription', searchResultFieldName: 'description' }
            ],
            clearSearch: clearStorageLocation,
            renderCell: params => (
                <>
                    <GridSearchIcon
                        style={{ cursor: 'pointer' }}
                        onClick={() =>
                            setSearchDialogOpen({
                                forRow: params.id,
                                forColumn: params.field
                            })
                        }
                    />
                    {params.row?.locationCode}
                </>
            )
        },
        {
            field: 'locationDescription',
            headerName: 'Location Description',
            width: 200
        },
        {
            field: 'palletNumber',
            headerName: 'Pallet Number',
            editable: true,
            width: 200
        },
        {
            field: 'dateCreated',
            headerName: 'Date Created',
            type: 'date',
            editable: true,
            width: 150,
            valueGetter: value => {
                return new Date(value);
            }
        },
        {
            field: 'createdById',
            headerName: 'Created By',
            width: 200,
            editable: true,
            type: 'search',
            search: searchEmployees,
            searchResults: employeesSearchResults,
            searchLoading: employeesSearchLoading,
            searchUpdateFieldNames: [
                { fieldName: 'createdByName', searchResultFieldName: 'fullName' },
                { fieldName: 'createdById', searchResultFieldName: 'id' }
            ],
            clearSearch: clearEmployeesSearch,
            renderCell: params => (
                <>
                    <GridSearchIcon
                        style={{ cursor: 'pointer' }}
                        onClick={() =>
                            setSearchDialogOpen({
                                forRow: params.id,
                                forColumn: params.field
                            })
                        }
                    />
                    {params.row?.createdById}
                </>
            )
        },
        {
            field: 'createdByName',
            headerName: 'Name',
            width: 150
        },
        {
            field: 'delete',
            headerName: '',
            width: 120,
            renderCell: params => (
                <Tooltip title="Delete">
                    <div>
                        {
                            <IconButton
                                aria-label="delete"
                                size="small"
                                onClick={() => handleDeleteRow(params.row)}
                            >
                                <DeleteIcon fontSize="inherit" />
                            </IconButton>
                        }
                    </div>
                </Tooltip>
            )
        }
    ];

    const renderStorageLocationSearchDialog = s => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = workStation?.workStationElements.find(
                r => r.workStationElementId === searchDialogOpen.forRow
            );

            let newRow = {
                ...currentRow,
                [s.field]: selected.id
            };

            s.searchUpdateFieldNames?.forEach(f => {
                newRow = { ...newRow, [f.fieldName]: selected[f.searchResultFieldName] };
            });
            processRowUpdate(newRow, currentRow);
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        return (
            <div id={s.field}>
                <Dialog open={searchDialogOpen.forColumn === s.field} onClose={handleClose}>
                    <DialogTitle>Search</DialogTitle>
                    <DialogContent>
                        <Search
                            autoFocus
                            propertyName={`${s.field}-searchTerm`}
                            label=""
                            resultsInModal
                            resultLimit={100}
                            value={storageLocationSearchTerm}
                            handleValueChange={(_, newVal) => setStorageLocationSearchTerm(newVal)}
                            search={s.search}
                            searchResults={s.searchResults?.map(r => ({
                                ...r,
                                id: r[s.field]
                            }))}
                            searchLoading={s.loading}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleSearchResultSelect}
                            clearSearch={() => {}}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Close</Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    };

    const renderEmployeeSearchDialog = e => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = workStation?.workStationElements.find(
                r => r.workStationElementId === searchDialogOpen.forRow
            );

            let newRow = {
                ...currentRow,
                [e.field]: selected.id
            };

            e.searchUpdateFieldNames?.forEach(f => {
                newRow = { ...newRow, [f.fieldName]: selected[f.searchResultFieldName] };
            });
            processRowUpdate(newRow, currentRow);
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        return (
            <div id={e.field}>
                <Dialog open={searchDialogOpen.forColumn === e.field} onClose={handleClose}>
                    <DialogTitle>Search</DialogTitle>
                    <DialogContent>
                        <Search
                            autoFocus
                            propertyName={`${e.field}-searchTerm`}
                            label=""
                            resultsInModal
                            resultLimit={100}
                            value={employeeSearchTerm}
                            handleValueChange={(_, newVal) => setEmployeeSearchTerm(newVal)}
                            search={e.search}
                            searchResults={e.searchResults?.map(r => ({
                                ...r,
                                id: r[e.field]
                            }))}
                            searchLoading={e.loading}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleSearchResultSelect}
                            clearSearch={() => {}}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Close</Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {(updateError || createWorkStationError) && (
                    <Grid size={12}>
                        <ErrorCard
                            errorMessage={
                                updateError ? updateError?.details : createWorkStationError
                            }
                        />
                    </Grid>
                )}
                <Grid size={12}>
                    <Typography variant="h4">Workstation Utility</Typography>
                </Grid>
                {(isLoading ||
                    isNewWorkStationsLoading ||
                    updateLoading ||
                    createWorkStationLoading) && (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                )}
                <Grid size={4}>
                    <InputField
                        value={workStation?.workStationCode}
                        fullWidth
                        label="Workstation Code"
                        propertyName="workStationCode"
                        onChange={handleFieldChange}
                        disabled={!creating}
                    />
                </Grid>
                <Grid size={8}>
                    <InputField
                        value={workStation?.description}
                        fullWidth
                        label="Description"
                        propertyName="description"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={3}>
                    <Dropdown
                        fullWidth
                        label="CIT Code"
                        items={
                            citCodesGetResult?.map(c => ({
                                id: c.code,
                                displayText: `${c.code} - ${c.name}`
                            })) || []
                        }
                        value={workStation?.citCode}
                        onChange={handleFieldChange}
                        propertyName="citCode"
                        loading={isCitCodesLoading}
                    />
                </Grid>
                <Grid size={6}>
                    <Dropdown
                        fullWidth
                        label="Zone Type"
                        items={[
                            { id: 'AKU/DORIK', displayText: 'Dorik' },
                            { id: 'AKU/MAJIK FLEX ZONE', displayText: 'Aku/Majik Flex Zone' },
                            { id: 'FLEXIBLE', displayText: 'Flexible' },
                            { id: 'MAJIK SPEAKERS', displayText: 'Majik Speakers' },
                            { id: 'PERMANENT', displayText: 'Permanent' },
                            { id: 'SPEAKERS', displayText: 'Speakers' }
                        ]}
                        value={workStation?.zoneType}
                        onChange={handleFieldChange}
                        propertyName="zoneType"
                    />
                </Grid>
                <Grid size={12}>
                    {workStationElementColumns
                        .filter(e => e.type === 'search' && e.headerName === 'Added By')
                        .map(e => renderEmployeeSearchDialog(e))}
                    {workStationElementColumns
                        .filter(s => s.type === 'search' && s.headerName === 'Storage Location')
                        .map(s => renderStorageLocationSearchDialog(s))}
                    <DataGrid
                        getRowId={row => row?.workStationElementId}
                        rows={workStation?.workStationElements}
                        columns={workStationElementColumns}
                        density="compact"
                        editMode="cell"
                        processRowUpdate={processRowUpdate}
                        rowSelectionModel={
                            rowUpdated
                                ? { type: 'include', ids: new Set([rowUpdated]) }
                                : { type: 'include', ids: new Set() }
                        }
                        loading={false}
                    />
                </Grid>
                <Grid size={4}>
                    <Button onClick={addNewRow} variant="outlined">
                        Add new Workstation
                    </Button>
                </Grid>
                <Grid size={12}>
                    <SaveBackCancelButtons
                        backClick={() => navigate('/stores2/work-stations')}
                        saveClick={() => {
                            const submitBody = {
                                ...workStation,
                                workStationElements: (workStation.workStationElements || []).map(
                                    e => (e.isAddition ? { ...e, workStationElementId: null } : e)
                                )
                            };

                            if (creating) {
                                createWorkStation(null, submitBody);
                            } else {
                                updateWorkStation(submitBody.workStationCode, submitBody);
                            }

                            setRowUpdated(null);
                        }}
                        saveDisabled={!changesMade}
                        cancelClick={handleCancelSelect}
                    />
                </Grid>
                <Grid>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() => setSnackbarVisible(false)}
                        message="Save Successful"
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default Workstation;
