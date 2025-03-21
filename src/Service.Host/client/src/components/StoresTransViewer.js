import React, { useState, useMemo } from 'react';
import {
    DatePicker,
    Dropdown,
    ExportButton,
    InputField,
    Loading,
    Search
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import useInitialise from '../hooks/useInitialise';
import useSearch from '../hooks/useSearch';
import Page from './Page';
import ReportDataGrids from './ReportDataGrids';

function StoresTransViewer() {
    const [partNumber, setPartNumber] = useState(null);
    const [transactionCode, setTransactionCode] = useState(null);
    const [functionCodes, setFunctionCodes] = useState([]); 
    const defaultStartDate = moment().subtract(1, 'days');
    const [fromDate, setFromDate] = useState(defaultStartDate);
    const [toDate, setToDate] = useState(new Date());

    const { result: functionCodesResult } = useInitialise(itemTypes.functionCodes.url);

    const {
        search: searchParts,
        results: partsSearchResults,
        loading: partsSearchLoading,
        clear: clearPartsSearch
    } = useSearch(itemTypes.parts.url, 'id', 'partNumber', 'description');

    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.storesTransViewer.url);

    const detailColumns = [
        {
            field: 'id',
            headerName: 'Selected',
            width: 220
        }
    ];

    const getQueryString = () => {
        let queryString = '?';

        if (fromDate) {
            queryString += `fromDate=${fromDate.toISOString()}&`;
        }

        if (toDate) {
            queryString += `toDate=${toDate.toISOString()}&`;
        }

        if (functionCodes?.length) {
            functionCodes.forEach(e => {
                queryString += `functionCodeList=${e}&`;
            });
        }

        if (transactionCode?.length) {
            queryString += `transactionCode=${transactionCode}&`;
        }

        if (partNumber?.length) {
            queryString += `partNumber=${partNumber}&`;
        }

        return queryString;
    };

    const addToFunctionCodeList = newVal => {
        if (newVal && !functionCodes.includes(newVal)) {
            setFunctionCodes(prev => [...prev, newVal]);
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

    const notReadyToRun = () => !fromDate && !toDate;

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={2}>
                <Grid size={11}>
                    <Typography variant="h4">Stores Transaction Viewer</Typography>
                </Grid>
                <Grid size={1}>
                    <ExportButton
                        buttonText="CSV"
                        disabled={isLoading || notReadyToRun()}
                        fileName="stores-trans-viewer-export.csv"
                        tooltipText="Download report as CSV"
                        href={`${
                            config.appRoot
                        }/stores2/stores-trans-viewer/report${getQueryString()}`}
                    />
                </Grid>
                {isLoading ||
                    (partsSearchLoading && (
                        <Grid size={12}>
                            <Loading />
                        </Grid>
                    ))}
                <Grid size={3}>
                    <DatePicker
                        label="From Date"
                        value={fromDate}
                        maxDate={toDate}
                        onChange={setFromDate}
                    />
                </Grid>
                <Grid size={3}>
                    <DatePicker
                        label="To Date"
                        value={toDate}
                        minDate={fromDate}
                        onChange={setToDate}
                    />
                </Grid>
                <Grid size={3}>
                    <Search
                        propertyName="partNumber"
                        label="Part"
                        resultsInModal
                        resultLimit={100}
                        helperText="Enter a search term and press enter"
                        value={partNumber}
                        handleValueChange={(_, newVal) => setPartNumber(newVal)}
                        search={searchParts}
                        loading={partsSearchLoading}
                        searchResults={partsSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={r => {
                            setPartNumber(r.name);
                        }}
                        clearSearch={clearPartsSearch}
                        autoFocus={false}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        value={transactionCode}
                        onChange={(_, newValue) => setTransactionCode(newValue)}
                        label="Transaction Code"
                        propertyName="transactionCode"
                    />
                </Grid>
                <Grid size={3}>
                    {functionCodesResult && (
                        <Dropdown
                            value={null}
                            fullWidth
                            label="Function Code"
                            propertyName="functionCode"
                            helperText="Select function codes to add to search parameters"
                            allowNoValue
                            items={functionCodesResult.map(c => ({
                                id: c.code,
                                displayText: `${c.code} - ${c.description}`
                            }))}
                            onChange={(_, newVal) => addToFunctionCodeList(newVal)}
                        />
                    )}
                </Grid>
                <Grid size={3}>
                    <DataGrid
                        rows={
                            functionCodes?.map(a => ({
                                id: a
                            })) ?? []
                        }
                        columns={detailColumns}
                        density="compact"
                        rowHeight={45}
                        hideFooter
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        color="secondary"
                        variant="outlined"
                        onClick={() => {
                            setFunctionCodes([]);
                        }}
                    >
                        Clear
                    </Button>
                </Grid>
                <Grid size={2} />
                <Grid size={4}>
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

export default StoresTransViewer;
