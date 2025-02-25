import React, { useState, useMemo } from 'react';
import {
    DatePicker,
    Dropdown,
    InputField,
    Loading,
    Search
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import useInitialise from '../hooks/useInitialise';
import ReportDataGrids from './ReportDataGrids';

function StoresTransViewer() {
    const [partNumber, setPartNumber] = useState(null);
    const [transactionCode, setTransactionCode] = useState(null);
    const [functionCode, setFunctionCode] = useState(null);
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

    const getQueryString = () => {
        let queryString = '?';

        if (fromDate) {
            queryString += `fromDate=${fromDate.toISOString()}&`;
        }

        if (toDate) {
            queryString += `toDate=${toDate.toISOString()}&`;
        }

        if (functionCode?.length) {
            queryString += `functionCode=${functionCode}&`;
        }

        if (transactionCode?.length) {
            queryString += `transactionCode=${transactionCode}&`;
        }

        if (partNumber?.length) {
            queryString += `partNumber=${partNumber}&`;
        }

        return queryString;
    };

    const handleFunctionCodeChange = (propertyName, newValue) => {
        setFunctionCode(current => ({ ...current, [propertyName]: newValue }));
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

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={2}>
                <Grid size={12}>
                    <Typography variant="h4">Goods In Log</Typography>
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
                    {functionCodesResult && (
                        <Dropdown
                            value={functionCode}
                            fullWidth
                            label="Function Code"
                            propertyName="functionCode"
                            allowNoValue
                            items={functionCodesResult.map(c => ({
                                id: c.code,
                                displayText: `${c.code} - ${c.description}`
                            }))}
                            onChange={handleFunctionCodeChange}
                        />
                    )}
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="orderNumber"
                        label="Order Number"
                        type="number"
                        value={orderNumber}
                        onChange={(_, newValue) => setOrderNumber(newValue)}
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
                <Grid size={2}>
                    <Search
                        propertyName="partNumber"
                        label="Part"
                        resultsInModal
                        resultLimit={100}
                        helperText="Enter a search term and press enter or TAB"
                        value={partNumber}
                        disabled={disabled}
                        handleValueChange={(_, newVal) => {
                            setPartNumber({ partNumber: newVal });
                        }}
                        search={searchParts}
                        loading={partsSearchLoading}
                        searchResults={partsSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onKeyPressFunctions={[
                            {
                                keyCode: 9,
                                action: () => setPartNumber({ partNumber: partNumber?.toUpperCase() })
                            }
                        ]}
                        onResultSelect={r => {
                            setPartNumber(r);
                        }}
                        clearSearch={clearPartsSearch}
                        autoFocus={false}
                    />
                </Grid>
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
