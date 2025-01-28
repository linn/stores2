import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { useState } from 'react';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { 
    CreateButton,
    Dropdown,
    InputField,
    Loading
} from '@linn-it/linn-form-components-library';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useGet from '../hooks/useGet';

function StorageLocations() {
    const {  isLoading, result } = useInitialise(itemTypes.storageSites.url);
    const [selectedSiteCode, setSelectedSiteCode] = useState(null);
    const [selectedAreaCode, setSelectedAreaCode] = useState(null);
    const [searchTerm, setSearchTerm] = useState("");

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === "storageSite") {
            setSelectedSiteCode(newValue);
        }
        else if (propertyName === "storageArea") {
            setSelectedAreaCode(newValue);
        }
        else if (propertyName === "searchTerm") {
            setSearchTerm(newValue);
        }
    };

    const selectedSite = () => {
        if (selectedSiteCode) {
            return result.find(s => s.siteCode === selectedSiteCode);
        }
        return null;
    }

    const selectedArea = () => {
        if (selectedSite() && selectedAreaCode) {
            return selectedSite().storageAreas.find(s => s.storageAreaCode === selectedAreaCode);
        }
        return null;
    }

    const {
        send: getStorageLocations,
        storageLocationsLoading,
        result: storageLocationsResult,
        clearData: clearStoragePlaces
    } = useGet(itemTypes.storageLocations.url);

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
        }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Storage Locations</Typography>
                </Grid>
                {isLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) :
                (
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
                            {selectedSiteCode && 
                                 <InputField
                                    fullWidth
                                    label="Code"
                                    value={selectedSite()?.siteCode}
                                    disabled
                                    propertyName="siteCode"
                                />}
                        </Grid>
                        <Grid size={2}>
                            {selectedSiteCode && 
                                <InputField
                                    fullWidth
                                    label="Prefix"
                                    value={selectedSite()?.sitePrefix}
                                    disabled
                                    propertyName="sitePrefix"
                                />}
                        </Grid>
                        <Grid size={2}/>
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
                            {selectedAreaCode && 
                                 <InputField
                                    fullWidth
                                    label="Code"
                                    value={selectedArea()?.storageAreaCode}
                                    disabled
                                    propertyName="storageAreaCode"
                                />}
                        </Grid>
                        <Grid size={2}>
                            {selectedAreaCode && 
                                <InputField
                                    fullWidth
                                    label="Prefix"
                                    value={selectedArea()?.areaPrefix}
                                    disabled
                                    propertyName="areaPrefix"
                                />}
                        </Grid>
                        <Grid size={2}/>
                        <Grid size={5}>
                            <InputField
                                value={searchTerm}
                                fullWidth
                                label="Location Code"
                                propertyName="searchTerm"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid size={7}/>
                        <Grid size={6}>
                        <Button
                            disabled={isLoading}
                            variant="outlined"
                            onClick={() => getStorageLocations(null, `?searchTerm=${searchTerm}&siteCode=${selectedSiteCode ? selectedSiteCode : ''}&storageAreaCode=${selectedAreaCode ? selectedAreaCode : ''}`)}
                        >
                            Search
                        </Button>
                        </Grid>
                        <Grid size={6}>
                            <CreateButton createUrl="/stores2/storage/locations/create" />
                        </Grid>
                        <Grid size={12}>
                            {storageLocationsLoading && <Loading />}
                            {storageLocationsResult && <DataGrid
                                getRowId={r => r.locationId}
                                rows={storageLocationsResult}
                                columns={locationsColumns}
                                hideFooter
                                density="compact"
                                loading={storageLocationsLoading}
                            />}           
                        </Grid>
                    </>
                )
                }

            </Grid>
        </Page>
    );
}

export default StorageLocations;
