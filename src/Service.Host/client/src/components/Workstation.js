import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Dialog from '@mui/material/Dialog';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import {
    Dropdown,
    ErrorCard,
    InputField,
    Loading,
    Search,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import { DataGrid, GridSearchIcon } from '@mui/x-data-grid';
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

    const { code } = useParams();

    const {
        send: getStorageLocation,
        isStorageLocationLoading,
        result: storageLocationGetResult
    } = useGet(itemTypes.storageLocations.url);

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
        if (!creating && !hasFetched) {
            setHasFetched(true);
            getNewWorkStations(encodeURI(code));
        }
    }, [creating, hasFetched, getNewWorkStations, code]);

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
                    createdBy: 0,
                    createdByName: '',
                    dateCreated: new Date(),
                    locationId: 0,
                    palletNumber: '',
                    creating: true
                }
            ]
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
                e.workStationElementId === oldRow.workStationElementId ? oldRow : e
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
    const searchRenderCell = params => (
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
            {params.row?.createdByName}
        </>
    );

    const workStationElementColumns = [
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
            field: 'createdBy',
            headerName: 'Added By',
            width: 200,
            editable: true,
            type: 'search',
            search: searchEmployees,
            searchResults: employeesSearchResults,
            searchLoading: employeesSearchLoading,
            searchUpdateFieldNames: [
                { fieldName: 'createdBy', searchResultFieldName: 'id' },
                { fieldName: 'createdByName', searchResultFieldName: 'fullName' }
            ],
            clearSearch: clearEmployeesSearch,
            renderCell: searchRenderCell
        },
        {
            field: 'createdByName',
            headerName: 'Name ',
            width: 150
        }
    ];

    const renderEmployeeSearchDialog = c => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = workStation?.workStationElements.find(
                r => r.workStationElementId === searchDialogOpen.forRow
            );

            let newRow = {
                ...currentRow,
                [c.field]: selected.id
            };

            c.searchUpdateFieldNames?.forEach(f => {
                newRow = { ...newRow, [f.fieldName]: selected[f.searchResultFieldName] };
            });
            processRowUpdate(newRow, currentRow);
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        return (
            <div id={c.field}>
                <Dialog open={searchDialogOpen.forColumn === c.field} onClose={handleClose}>
                    <DialogTitle>Search</DialogTitle>
                    <DialogContent>
                        <Search
                            autoFocus
                            propertyName={`${c.field}-searchTerm`}
                            label=""
                            resultsInModal
                            resultLimit={100}
                            value={employeeSearchTerm}
                            handleValueChange={(_, newVal) => setEmployeeSearchTerm(newVal)}
                            search={c.search}
                            searchResults={c.searchResults?.map(r => ({
                                ...r,
                                id: r[c.field]
                            }))}
                            searchLoading={c.loading}
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
                    <InputField
                        value={workStation?.citCode}
                        fullWidth
                        label="Code"
                        propertyName="citCode"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        value={workStation?.citName}
                        fullWidth
                        label="Name"
                        propertyName="citName"
                        onChange={handleFieldChange}
                        disabled
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
                        .filter(c => c.type === 'search')
                        .map(c => renderEmployeeSearchDialog(c))}
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

                <Grid size={4}>
                    <Button
                        onClick={() => {
                            if (creating) {
                                createWorkStation(null, workStation);
                            } else {
                                updateWorkStation(workStation.workStationCode, workStation);
                            }
                            setRowUpdated(null);
                        }}
                        variant="outlined"
                        disabled={newWorkStationsGetResult === workStation}
                    >
                        Save
                    </Button>
                </Grid>
                <Grid>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() => setSnackbarVisible(false)}
                        message="Save Successful"
                    />
                </Grid>
                <Grid size={4}>
                    <Button onClick={handleCancelSelect} variant="outlined" disabled={!rowUpdated}>
                        Cancel
                    </Button>
                </Grid>
                {(updateError || createWorkStationError) && (
                    <Grid size={12}>
                        <ErrorCard
                            errorMessage={
                                updateError ? updateError?.details : createWorkStationError
                            }
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default Workstation;
