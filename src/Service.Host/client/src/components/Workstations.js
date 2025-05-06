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
    const [workStationCode, setWorkstationSearchTerm] = useState('');
    const [citCode, setCitCodeSearchTerm] = useState('');

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'workStationCode') {
            setWorkstationSearchTerm(newValue);
        } else if (propertyName === 'citCode') {
            setCitCodeSearchTerm(newValue);
        }
    };

    const {
        send: getWorkStations,
        workStationsLoading,
        result: workStationsResult
    } = useGet(itemTypes.workStations.url);

    const locationsColumns = [
        {
            field: 'workStationCode',
            headerName: 'Workstation Code',
            width: 150,
            renderCell: params => (
                <Link
                    component={RouterLink}
                    variant="body2"
                    to={`/stores2/storage/locations/${params.row.workStationCode}`}
                >
                    {params.row.workStationCode}
                </Link>
            )
        },
        {
            field: 'description',
            headerName: 'Description',
            width: 300
        },
        {
            field: 'citName',
            headerName: 'CIT Name',
            width: 140
        },
        {
            field: 'citCode',
            headerName: 'CIT Code',
            width: 140
        },
        {
            field: 'zoneType',
            headerName: 'zoneType',
            width: 100
        }
    ];

    const createUrl = () => {
        return '/stores2/work-stations/locations/create';
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Workstation Utility</Typography>
                </Grid>
                {isLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    <>
                        <Grid size={5}>
                            <InputField
                                value={workStationCode}
                                fullWidth
                                label="Workstation Code"
                                propertyName="workStationCode"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid size={2} />
                        <Grid size={2} />
                        <Grid size={5}>
                            <InputField
                                value={citCode}
                                fullWidth
                                label="CIT Code"
                                propertyName="citCode"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid size={7} />
                        <Grid size={6}>
                            <Button
                                disabled={isLoading}
                                variant="outlined"
                                onClick={() =>
                                    getWorkStations(
                                        null,
                                        `?workStationCode=${workStationCode || ''}&citCode=${citCode || ''}`
                                    )
                                }
                            >
                                Search
                            </Button>
                        </Grid>
                        <Grid size={6}>
                            <CreateButton createUrl={createUrl()} />
                        </Grid>
                        <Grid size={12}>
                            {workStationsLoading && <Loading />}
                            {workStationsResult && (
                                <DataGrid
                                    getRowId={r => r.workStationCode}
                                    rows={workStationsResult}
                                    columns={locationsColumns}
                                    hideFooter
                                    density="compact"
                                    loading={workStationsLoading}
                                />
                            )}
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default Workstations;
