import React, { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';

import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    CreateButton,
    Dropdown,
    ErrorCard,
    InputField,
    Loading
} from '@linn-it/linn-form-components-library';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import Page from './Page';

function StorageLocations() {
    const { isLoading, result } = useInitialise(itemTypes.storageSites.url);
    const [selectedSiteCode, setSelectedSiteCode] = useState(null);
    const [selectedAreaCode, setSelectedAreaCode] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'storageSite') {
            setSelectedSiteCode(newValue);
        } else if (propertyName === 'storageArea') {
            setSelectedAreaCode(newValue);
        } else if (propertyName === 'searchTerm') {
            setSearchTerm(newValue);
        }
    };

    const selectedSite = () => {
        if (selectedSiteCode) {
            return result.find(s => s.siteCode === selectedSiteCode);
        }
        return null;
    };

    const selectedArea = () => {
        if (selectedSite() && selectedAreaCode) {
            return selectedSite().storageAreas.find(s => s.storageAreaCode === selectedAreaCode);
        }
        return null;
    };

    const locationCodePrefix = () => {
        const site = selectedSite();
        if (!site) {
            return '';
        }
        const area = site.storageAreas.find(s => s.storageAreaCode === selectedAreaCode);
        return area ? `${site.sitePrefix}-${area.areaPrefix}-` : '';
    };

    const {
        send: getStorageLocations,
        storageLocationsLoading,
        result: storageLocationsResult
    } = useGet(itemTypes.storageLocations.url);

    const {
        send: updateLocation,
        isLoading: updateLoading,
        errorMessage: updateError
    } = usePut(itemTypes.storageLocations.url, true);

    const makeInvalid = row => {
        var updatedLoc = row;
        updatedLoc.dateInvalid = new Date();
        updateLocation(row.locationId, updatedLoc);
    };

    const locationsColumns = [
        {
            field: 'locationCode',
            headerName: 'Code',
            width: 150,
            renderCell: params => (
                <Link
                    component={RouterLink}
                    variant="body2"
                    to={`/stores2/storage/locations/${params.row.locationId}`}
                >
                    {params.row.locationCode}
                </Link>
            )
        },
        {
            field: 'description',
            headerName: 'Description',
            width: 300
        },
        {
            field: 'siteCode',
            headerName: 'Site',
            width: 140
        },
        {
            field: 'storageAreaCode',
            headerName: 'Area',
            width: 140
        },
        {
            field: 'locationId',
            headerName: 'Id',
            width: 100
        },
        {
            field: 'valid',
            headerName: '',
            width: 160,
            renderCell: params => (
                <Button
                    disabled={params.row.dateInvalid || updateLoading}
                    size="small"
                    onClick={() => makeInvalid(params.row)}
                >
                    {params.row.dateInvalid ? 'Invalid' : 'Make Invalid'}
                </Button>
            )
        }
    ];

    const createUrl = () => {
        if (selectedSiteCode && selectedAreaCode) {
            return `/stores2/storage/locations/create?siteCode=${selectedSiteCode}&storageAreaCode=${selectedAreaCode}&prefix=${locationCodePrefix()}`;
        }
        if (selectedSiteCode) {
            return `/stores2/storage/locations/create?siteCode=${selectedSiteCode}&prefix=${locationCodePrefix()}`;
        }
        return '/stores2/storage/locations/create';
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Storage Locations</Typography>
                </Grid>
                {updateError && (
                    <Grid size={12}>
                        <List>
                            <ErrorCard errorMessage={updateError} />
                        </List>
                    </Grid>
                )}
                {isLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    <>
                        <Grid size={5}>
                            <Dropdown
                                value={selectedSiteCode}
                                fullWidth
                                label="Site"
                                propertyName="storageSite"
                                allowNoValue
                                items={result?.map(c => ({
                                    id: c.siteCode,
                                    displayText: c.description
                                }))}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid size={3}>
                            {selectedSiteCode && (
                                <InputField
                                    fullWidth
                                    label="Code"
                                    value={selectedSite()?.siteCode}
                                    disabled
                                    propertyName="siteCode"
                                />
                            )}
                        </Grid>
                        <Grid size={2}>
                            {selectedSiteCode && (
                                <InputField
                                    fullWidth
                                    label="Prefix"
                                    value={selectedSite()?.sitePrefix}
                                    disabled
                                    propertyName="sitePrefix"
                                />
                            )}
                        </Grid>
                        <Grid size={2} />
                        <Grid size={5}>
                            <Dropdown
                                value={selectedAreaCode}
                                fullWidth
                                label="Area"
                                propertyName="storageArea"
                                allowNoValue
                                items={selectedSite()?.storageAreas?.map(c => ({
                                    id: c.storageAreaCode,
                                    displayText: c.description
                                }))}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid size={3}>
                            {selectedAreaCode && (
                                <InputField
                                    fullWidth
                                    label="Code"
                                    value={selectedArea()?.storageAreaCode}
                                    disabled
                                    propertyName="storageAreaCode"
                                />
                            )}
                        </Grid>
                        <Grid size={2}>
                            {selectedAreaCode && (
                                <InputField
                                    fullWidth
                                    label="Prefix"
                                    value={selectedArea()?.areaPrefix}
                                    disabled
                                    propertyName="areaPrefix"
                                />
                            )}
                        </Grid>
                        <Grid size={2} />
                        <Grid size={5}>
                            <InputField
                                value={searchTerm}
                                fullWidth
                                label="Location Code"
                                propertyName="searchTerm"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid size={7} />
                        <Grid size={6}>
                            <Button
                                disabled={isLoading}
                                variant="outlined"
                                onClick={() =>
                                    getStorageLocations(
                                        null,
                                        `?searchTerm=${searchTerm}&siteCode=${selectedSiteCode || ''}&storageAreaCode=${selectedAreaCode || ''}`
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
                            {storageLocationsLoading && <Loading />}
                            {storageLocationsResult && (
                                <DataGrid
                                    getRowId={r => r.locationId}
                                    rows={storageLocationsResult}
                                    columns={locationsColumns}
                                    hideFooter
                                    density="compact"
                                    loading={storageLocationsLoading}
                                />
                            )}
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default StorageLocations;
