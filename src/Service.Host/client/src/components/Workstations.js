import React, { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import { CreateButton, InputField, Loading } from '@linn-it/linn-form-components-library';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useGet from '../hooks/useGet';
import Page from './Page';

function Workstations() {
    const { isLoading } = useInitialise(itemTypes.workStations.url);
    const [workStationCodeSearch, setWorkstationSearchTerm] = useState('');
    const [citCodeSearch, setCitCodeSearchTerm] = useState('');

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'workStationCodeSearch') {
            setWorkstationSearchTerm(newValue);
        } else if (propertyName === 'citCodeSearch') {
            setCitCodeSearchTerm(newValue);
        }
    };

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
                <Grid size={12}>
                    <Typography variant="h4">Workstation Utility</Typography>
                </Grid>
                <Grid size={12}>
                    <CreateButton createUrl="/stores2/work-stations/create" />
                </Grid>
                {isLoading && (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                )}
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
