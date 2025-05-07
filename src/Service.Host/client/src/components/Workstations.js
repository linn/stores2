import React, { useEffect, useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';

import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Loading,
    ErrorCard,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import Page from './Page';

function Workstations() {
    const { isLoading } = useInitialise(itemTypes.workStations.url);
    const [creating, setCreating] = useState(false);
    const [workStations, setWorkStations] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);
    const [rowUpdated, setRowUpdated] = useState();

    const [workStationCodeSearch, setWorkstationSearchTerm] = useState('');
    const [citCodeSearch, setCitCodeSearchTerm] = useState('');

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'workStationCodeSearch') {
            setWorkstationSearchTerm(newValue);
        } else if (propertyName === 'citCode') {
            setCitCodeSearchTerm(newValue);
        }
    };

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
        send: getWorkStations,
        workStationsLoading,
        result: workStationsResult
    } = useGet(itemTypes.workStations.url);

    const [hasFetched, setHasFetched] = useState(false);

    if (!hasFetched) {
        setHasFetched(true);
        getWorkStations();
    }

    useEffect(() => {
        setWorkStations(workStationsResult);
    }, [workStationsResult]);

    useEffect(() => {
        if (!!updateResult || !!createWorkStationResult) {
            getWorkStations();
            setSnackbarVisible(true);
            clearCreateWorkStation();
            clearUpdateResult();
        }
    }, [
        clearCreateWorkStation,
        updateResult,
        getWorkStations,
        createWorkStationResult,
        clearUpdateResult
    ]);

    const addNewRow = () => {
        setWorkStations([
            ...workStations,
            {
                workStationCode: '',
                description: '',
                citName: '',
                citCode: '',
                zoneType: '',
                workStationElements: null,
                creating: true
            }
        ]);
        setCreating(true);
    };

    const handleCancelSelect = () => {
        const oldRow = workStationsResult.find(st => st.workStationCode === rowUpdated);

        setWorkStations(prevRows =>
            prevRows.map(row => (row.workStationCode === oldRow.workStationCode ? oldRow : row))
        );

        setRowUpdated(null);
    };

    const processRowUpdate = newRow => {
        const updatedRow = { ...newRow, updated: true };
        setWorkStations(prevRows =>
            prevRows.map(row =>
                row.workStationCode === newRow.workStationCode || row.workStationCode === ''
                    ? updatedRow
                    : row
            )
        );
        setRowUpdated(newRow.workStationCode);
        return newRow;
    };

    const workStationColumns = [
        {
            field: 'workStationCode',
            headerName: 'Workstation Code',
            width: 300,
            renderCell: params => (
                <Link
                    component={RouterLink}
                    variant="body2"
                    to={`/stores2/work-stations/${params.row.workStationCode}`}
                >
                    {params.row.workStationCode}
                </Link>
            )
        },
        {
            field: 'description',
            headerName: 'Description',
            width: 400
        },
        {
            field: 'citName',
            headerName: 'CIT Name',
            width: 200
        },
        {
            field: 'citCode',
            headerName: 'CIT Code',
            width: 200
        },
        {
            field: 'zoneType',
            headerName: 'Zone Type',
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
                        value={workStationCodeSearch}
                        fullWidth
                        label="Workstation Code"
                        propertyName="workStationCode"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        value={citCodeSearch}
                        fullWidth
                        label="CIT Code"
                        propertyName="citCode"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={6}>
                    <Button
                        disabled={isLoading}
                        variant="outlined"
                        onClick={() =>
                            getWorkStations(
                                null,
                                `?workStationCode=${workStationCodeSearch || ''}&citCode=${citCodeSearch || ''}`
                            )
                        }
                    >
                        Search
                    </Button>
                </Grid>
                <Grid size={12}>
                    {workStationsLoading && <Loading />}
                    {workStationsResult && (
                        <DataGrid
                            editMode="cell"
                            processRowUpdate={processRowUpdate}
                            getRowId={row => row.workStationCode}
                            rows={workStations}
                            columns={workStationColumns}
                            density="compact"
                            loading={false}
                            rowSelectionModel={[rowUpdated]}
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
                    )}
                </Grid>
                <Grid size={4}>
                    <Button onClick={addNewRow} variant="outlined" disabled={creating}>
                        Add new Workstation
                    </Button>
                </Grid>
                <Grid size={4}>
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
                        disabled={workStationsResult === workStations}
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

export default Workstations;
