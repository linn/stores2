import React, { useState, useMemo } from 'react';
import { useAuth } from 'react-oidc-context';
import { Loading, ExportButton, Search } from '@linn-it/linn-form-components-library';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import ReportDataGrids from './ReportDataGrids';

function StoragePlaceAudit() {
    const [range, setRange] = useState(null);
    const [locationSelect, setLocationSelect] = useState(null);
    const [locations, setLocations] = useState([]);

    const auth = useAuth();
    const token = auth.user?.access_token;

    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.storagePlaceAudit.url);

    const {
        send: getStoragePlaces,
        storagePlacesLoading,
        result: storagePlacesResult,
        clearData: clearStoragePlaces
    } = useGet(itemTypes.storagePlaces.url);

    const {
        send: getAuditLocations,
        auditLocationsLoading,
        result: auditLocationsResult,
        clearData: clearAuditLocations
    } = useGet(itemTypes.auditLocations.url);

    const getQueryString = () => {
        let queryString = '?';

        if (range) {
            queryString += `locationRange=${range.toUpperCase()}&`;
        }

        if (locations?.length) {
            locations.forEach(e => {
                queryString += `locationList=${e}&`;
            });
        }

        return queryString;
    };

    const addToLocationList = selectedLocation => {
        if (selectedLocation) {
            let locationToAdd;
            if (Number.isNaN(Number(selectedLocation))) {
                locationToAdd = selectedLocation.toUpperCase();
            } else {
                locationToAdd = `P${selectedLocation}`;
            }

            if (locations.findIndex(a => a === locationToAdd) < 0) {
                const newLocations = [...locations, locationToAdd];
                setLocations(newLocations);
            }
        }
    };

    const reports = useMemo(
        () => (
            <ReportDataGrids
                perReportExport={false}
                reportData={reportResult?.reportResults ?? []}
                repeatHeaders
                fixedRowHeight
                showHeader={false}
                renderZeroes
                showTotals={false}
            />
        ),
        [reportResult]
    );

    const detailColumns = [
        {
            field: 'id',
            headerName: 'Selected',
            width: 220
        }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Storage Place Audit Report</Typography>
                </Grid>
                {isLoading && (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                )}
                <Grid size={3}>
                    <Search
                        propertyName="rangeSelect"
                        label="Range"
                        resultsInModal
                        autoFocus={false}
                        resultLimit={100}
                        value={range}
                        handleValueChange={(_, newVal) => setRange(newVal)}
                        helperText="Press ENTER to search or TAB to proceed"
                        search={() => getAuditLocations(null, `?searchTerm=${range}`)}
                        searchResults={auditLocationsResult?.map(s => ({
                            ...s,
                            id: s.storagePlace,
                            name: s.storagePlace
                        }))}
                        loading={auditLocationsLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setRange(newValue.id);
                        }}
                        clearSearch={clearAuditLocations}
                    />
                </Grid>
                <Grid size={3}>
                    <Search
                        propertyName="locationSelect"
                        label="Or Locations"
                        resultsInModal
                        autoFocus={false}
                        resultLimit={100}
                        value={locationSelect}
                        handleValueChange={(_, newVal) => setLocationSelect(newVal)}
                        helperText="Press ENTER to search or TAB to proceed"
                        search={() => getStoragePlaces(null, `?searchTerm=${locationSelect}`)}
                        onKeyPressFunctions={[
                            { keyCode: 9, action: () => addToLocationList(locationSelect) }
                        ]}
                        searchResults={storagePlacesResult?.map(s => ({
                            ...s,
                            id: s.name
                        }))}
                        loading={storagePlacesLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setLocationSelect(newValue.id);
                            addToLocationList(newValue.id);
                        }}
                        clearSearch={clearStoragePlaces}
                    />
                </Grid>
                <Grid size={2}>
                    <DataGrid
                        rows={
                            locations?.map(a => ({
                                id: a
                            })) ?? []
                        }
                        columns={detailColumns}
                        density="compact"
                        rowHeight={45}
                        hideFooter
                        autoHeight
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        color="secondary"
                        variant="outlined"
                        onClick={() => {
                            setLocations([]);
                            setRange(null);
                        }}
                    >
                        Clear
                    </Button>
                </Grid>
                <Grid size={1} />
                <Grid size={1}>
                    <Button
                        disabled={isLoading}
                        variant="contained"
                        onClick={() => {
                            getReport(null, getQueryString());
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                <Grid size={1}>
                    <ExportButton
                        buttonText="PDF"
                        accept="application/pdf"
                        fileName="export.pdf"
                        tooltipText="Download report as PDF"
                        accessToken={token}
                        href={`${
                            config.appRoot
                        }/stores2/reports/storage-place-audit/pdf${getQueryString()}`}
                    />
                </Grid>
                <Grid item size={11} />
                {isLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    reports
                )}
            </Grid>
        </Page>
    );
}

export default StoragePlaceAudit;