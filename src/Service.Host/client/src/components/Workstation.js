import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import Dialog from '@mui/material/Dialog';
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
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import useSearch from '../hooks/useSearch';
import Page from './Page';

function Workstation({ creating }) {
    const [workStation, setWorkStation] = useState();
    const [originalWorkStation, setOriginalWorkStation] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);
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
        loading: storageLocationsSearchLoading
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
        loading: employeesSearchLoading
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
            setSnackbarVisible(true);
            clearCreateWorkStation();
            clearUpdateResult();

            const savedWorkStation = updateResult || createWorkStationResult;
            setOriginalWorkStation(savedWorkStation);
            setWorkStation(savedWorkStation);
            setChangesMade(false);
        }
    }, [clearCreateWorkStation, clearUpdateResult, createWorkStationResult, updateResult]);

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
        if (!creating && newWorkStationsGetResult) {
            setWorkStation(newWorkStationsGetResult);
            setOriginalWorkStation(newWorkStationsGetResult);
        }
    }, [creating, newWorkStationsGetResult]);

    const addNewRow = () => {
        setWorkStation(prev => ({
            ...prev,
            workStationElements: [
                ...(prev.workStationElements ?? []),
                {
                    workStationElementId: (prev.workStationElements?.length ?? 0) + 1,
                    workStationCode: prev.workStationCode || '',
                    createdById: null,
                    createdByName: null,
                    dateCreated: new Date(),
                    locationId: null,
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
        setWorkStation(originalWorkStation);
        setChangesMade(false);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };

        setWorkStation(prev => ({
            ...prev,
            workStationElements: prev.workStationElements.map(row =>
                row.workStationElementId === newRow.workStationElementId ? updatedRow : row
            )
        }));

        setChangesMade(true);

        return newRow;
    };

    const [employeeDialogOpenForRow, setEmployeeDialogOpenForRow] = useState(null);
    const [storageDialogOpenForRow, setStorageDialogOpenForRow] = useState(null);

    const workStationElementColumns = [
        {
            field: 'locationId',
            headerName: 'Storage Location',
            width: 150,
            editable: true,
            renderCell: params => (
                <>
                    <GridSearchIcon
                        style={{ cursor: 'pointer' }}
                        onClick={() => setStorageDialogOpenForRow(params.id)}
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
            valueGetter: value => new Date(value),
            valueFormatter: value => value && moment(value).format('DD-MMM-YYYY')
        },
        {
            field: 'createdById',
            headerName: 'Created By',
            width: 200,
            editable: true,
            renderCell: params => (
                <>
                    <GridSearchIcon
                        style={{ cursor: 'pointer' }}
                        onClick={() => setEmployeeDialogOpenForRow(params.id)}
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
                        <IconButton
                            aria-label="delete"
                            size="small"
                            onClick={() => handleDeleteRow(params.row)}
                        >
                            <DeleteIcon fontSize="inherit" />
                        </IconButton>
                    </div>
                </Tooltip>
            )
        }
    ];

    const renderStorageLocationSearchDialog = () => {
        const handleClose = () => setStorageDialogOpenForRow(null);

        const handleSearchResultSelect = selected => {
            const currentRow = workStation?.workStationElements.find(
                r => r.workStationElementId === storageDialogOpenForRow
            );

            let newRow = {
                ...currentRow,
                locationId: selected.locationId,
                locationCode: selected.locationCode,
                locationDescription: selected.description
            };

            processRowUpdate(newRow);
            handleClose();
        };

        return (
            <Dialog open={!!storageDialogOpenForRow} onClose={handleClose}>
                <DialogTitle>Search Storage Location</DialogTitle>
                <DialogContent>
                    <Search
                        autoFocus
                        propertyName="storageLocation-searchTerm"
                        label=""
                        resultsInModal
                        resultLimit={100}
                        value={storageLocationSearchTerm}
                        handleValueChange={(_, newVal) => setStorageLocationSearchTerm(newVal)}
                        search={searchStorageLocations}
                        searchResults={storageLocationsSearchResults?.map(r => ({
                            ...r,
                            id: r.locationId
                        }))}
                        searchLoading={storageLocationsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleSearchResultSelect}
                        clearSearch={() => {}}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Close</Button>
                </DialogActions>
            </Dialog>
        );
    };

    const renderEmployeeSearchDialog = () => {
        const handleClose = () => setEmployeeDialogOpenForRow(null);

        const handleSearchResultSelect = selected => {
            const currentRow = workStation?.workStationElements.find(
                r => r.workStationElementId === employeeDialogOpenForRow
            );

            let newRow = {
                ...currentRow,
                createdById: selected.id,
                createdByName: selected.fullName
            };

            processRowUpdate(newRow);
            handleClose();
        };

        return (
            <Dialog open={!!employeeDialogOpenForRow} onClose={handleClose}>
                <DialogTitle>Search Employee</DialogTitle>
                <DialogContent>
                    <Search
                        autoFocus
                        propertyName="employee-searchTerm"
                        label=""
                        resultsInModal
                        resultLimit={100}
                        value={employeeSearchTerm}
                        handleValueChange={(_, newVal) => setEmployeeSearchTerm(newVal)}
                        search={searchEmployees}
                        searchResults={employeesSearchResults?.map(r => ({ ...r, id: r.id }))}
                        searchLoading={employeesSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleSearchResultSelect}
                        clearSearch={() => {}}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Close</Button>
                </DialogActions>
            </Dialog>
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
                {(isNewWorkStationsLoading || updateLoading || createWorkStationLoading) && (
                    <Grid size={12}>
                        <Loading />
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
                    {renderEmployeeSearchDialog()}
                    {renderStorageLocationSearchDialog()}
                    <DataGrid
                        getRowId={row => row?.workStationElementId}
                        rows={workStation?.workStationElements}
                        columns={workStationElementColumns}
                        density="compact"
                        editMode="cell"
                        processRowUpdate={processRowUpdate}
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
