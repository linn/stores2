import React from 'react';
import { useState } from 'react';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { 
    Dropdown,
    InputField,
    Loading
} from '@linn-it/linn-form-components-library';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';

function StorageLocations() {
    const {  isLoading, result } = useInitialise(itemTypes.storageSites.url);
    const [selectedSiteCode, setSelectedSiteCode] = useState(null);
    const [selectedAreaCode, setSelectedAreaCode] = useState(null);

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === "storageSite") {
            setSelectedSiteCode(newValue);
        }
        else if (propertyName === "storageArea") {
            setSelectedAreaCode(newValue);
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
                    </>
                )
                }

            </Grid>
        </Page>
    );
}

export default StorageLocations;
