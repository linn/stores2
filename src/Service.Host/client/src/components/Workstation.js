import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    Dropdown,
    InputField,
    Loading,
    ErrorCard,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import Page from './Page';

function Workstation({ creating }) {
    const { isLoading } = useInitialise(itemTypes.workStations.url);
    const [workStation, setWorkStation] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);
    const [rowUpdated, setRowUpdated] = useState();

    const { code } = useParams();

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
        send: getWorkstation,
        getWorkStationLoading,
        result: getWorkStationResult
    } = useGet(itemTypes.workStations.url);

    const [hasFetched, setHasFetched] = useState(false);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getWorkstation(code);
    }

    const navigate = useNavigate();

    const [changesMade, setChangesMade] = useState(false);

    if (getWorkStationResult && !workStation) {
        setWorkStation(getWorkStationResult);
    }

    const handleFieldChange = (propertyName, newValue) => {
        setWorkStation(current => ({ ...current, [propertyName]: newValue }));
        setChangesMade(true);
    };

    useEffect(() => {
        if (updateResult) {
            setWorkStation(updateResult);
        }
    }, [updateResult]);

    const addNewRow = () =>
        setWorkStation(w => ({
            ...w,
            workStationElements: [
                ...w.workStationElements,
                {
                    workStationCode: workStation.workStationCode,
                    createdBy: 0,
                    createdByName: '',
                    dateCreated: new Date(),
                    locationId: 0,
                    palletNumber: '',
                    workStationElements: null
                }
            ]
        }));

    const handleCancelSelect = () => {
        const oldRow = getWorkStationResult.find(st => st.workStationCode === rowUpdated);

        setWorkStation(prevRows =>
            prevRows.map(row => (row.workStationCode === oldRow.workStationCode ? oldRow : row))
        );

        setRowUpdated(null);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };
        setWorkStation(prevRows =>
            prevRows.map(row =>
                row.workStationCode === newRow.workStationCode || row.workStationCode === ''
                    ? updatedRow
                    : row
            )
        );
        setRowUpdated(newRow.workStationCode);
        return newRow;
    };

    const workStationElementColumns = [
        {
            field: 'workstationElementId',
            headerName: 'WSE Id',
            width: 300
        },
        {
            field: 'dateCreated',
            headerName: 'CIT Name',
            width: 200
        },
        {
            field: 'createdBy',
            headerName: 'CIT Code',
            width: 200
        },
        {
            field: 'createdByName',
            headerName: 'createdByName',
            width: 150
        }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Workstation Utility</Typography>
                </Grid>
                {isLoading ||
                    createWorkStationLoading ||
                    (updateLoading && (
                        <Grid size={12}>
                            <List>
                                <Loading />
                            </List>
                        </Grid>
                    ))}
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
                            { id: 'FLEXIBLE', displayText: 'FLEXIBLE' },
                            { id: 'MAJIK SPEAKERS', displayText: 'Majik Speakers' },
                            { id: 'PERMANENT', displayText: 'Permanent' },
                            { id: 'SPEAKERS', displayText: 'Speakers' }
                        ]}
                        value={workStation?.zoneType}
                        onChange={() => {}}
                        propertyName="zoneType"
                    />
                </Grid>
                <Grid size={12}>
                    <DataGrid
                        getRowId={row => row.workstationElementId}
                        rows={workStation?.workStationElements}
                        editMode="cell"
                        processRowUpdate={processRowUpdate}
                        columns={workStationElementColumns}
                        rowHeight={34}
                        rowSelectionModel={[rowUpdated]}
                        loading={false}
                        isCellEditable={params => {
                            if (params.field === 'workStationCode' && params.row.creating) {
                                return true;
                            }
                            if (
                                (!rowUpdated || params.id === rowUpdated) &&
                                params.field !== 'workStationCode'
                            ) {
                                return true;
                            }
                            return false;
                        }}
                    />
                </Grid>
                <Grid size={4}>
                    <Button onClick={addNewRow} variant="outlined" disabled={creating}>
                        Add new Workstation
                    </Button>
                </Grid>
                {/* <Grid size={4}>
                    <Button
                        onClick={() => {
                            const updatedWorkStation = workStations.find(w => w.updated === true);
                            if (updatedWorkStation.creating) {
                                clearCreateWorkStation();
                                createWorkStation(null, updatedWorkStation);
                            } else {
                                updateWorkStation(
                                    updatedWorkStation.workStationCode,
                                    updatedWorkStation
                                );
                            }
                            setRowUpdated(null);
                        }}
                        variant="outlined"
                        disabled={getWorkStationResult === workStation}
                    >
                        Save
                    </Button>
                </Grid> */}
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
