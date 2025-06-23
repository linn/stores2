import React, { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import {
    CreateButton,
    InputField,
    Loading,
    PermissionIndicator,
    utilities
} from '@linn-it/linn-form-components-library';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';

function Workstations() {
    const [workStationCodeSearch, setWorkstationSearchTerm] = useState('');
    const [citCodeSearch, setCitCodeSearchTerm] = useState('');

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'workStationCodeSearch') {
            setWorkstationSearchTerm(newValue);
        } else if (propertyName === 'citCodeSearch') {
            setCitCodeSearchTerm(newValue);
        }
    };

    const hasPermission = utilities.getHref(workStationsResult?.[0], 'workstation-admin');

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

    const workStationColumns = [
        {
            field: 'workStationCode',
            headerName: 'Workstation Code',
            width: 300,
            renderCell: params => (
                <Link
                    component={RouterLink}
                    variant="body2"
                    to={`/stores2/work-stations/detail?code=${encodeURIComponent(params.row.workStationCode)}`}
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
                <Grid size={11}>
                    <Typography variant="h4">Workstation Utility</Typography>
                </Grid>
                <Grid size={1}>
                    <PermissionIndicator
                        hasPermission={hasPermission}
                        hasPermissionMessage="You have create/update workstation permissions"
                        noPermissionMessage="You do not have create/update workstation permissions"
                    />
                </Grid>
                <Grid size={12}>
                    <CreateButton
                        createUrl="/stores2/work-stations/create"
                        disabled={!hasPermission}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        value={workStationCodeSearch}
                        fullWidth
                        label="Workstation Code"
                        propertyName="workStationCodeSearch"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        value={citCodeSearch}
                        fullWidth
                        label="CIT Code"
                        propertyName="citCodeSearch"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={6}>
                    <Button
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
                            getRowId={row => row.workStationCode}
                            rows={workStationsResult}
                            columns={workStationColumns}
                            rowHeight={34}
                            loading={false}
                        />
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default Workstations;
