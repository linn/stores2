import React, { useState, useMemo, useEffect } from 'react';
import queryString from 'query-string';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import {
    DatePicker,
    Loading,
    useGet,
    ReportDataGrid,
    ExportButton
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import itemTypes from '../itemTypes';
import config from '../config';
import Page from './Page';

function DailyEuRsnImportReport() {
    const [fromDate, setFromDate] = useState(new Date());
    const [toDate, setToDate] = useState(new Date());
    const [returnIds, setReturnIds] = useState([]);

    const options = () => ({
        fromDate: fromDate.toISOString(),
        toDate: toDate.toISOString()
    });

    const {
        send: getDailyEuRsnImportReport,
        isLoading,
        result
    } = useGet(itemTypes.dailyEuRsnImportReport.url, true);

    useEffect(() => {
        if (!result || !result.reportResults || result.reportResults.length === 0) {
            setReturnIds([]);
        } else {
            var resultValues = result.reportResults[0].results
                .map(a => a.values)
                .map(row => row[0]?.textDisplayValue);

            if (!resultValues || resultValues.length === 0) {
                setReturnIds([]);
                return;
            }

            setReturnIds([...new Set(resultValues)]);
        }
    }, [result]);

    const report = useMemo(
        () => (
            <ReportDataGrid
                report={result?.reportResults[0]}
                fixedRowHeight
                showHeader={true}
                renderZeroes
                showTotals={false}
            />
        ),
        [result]
    );

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Daily EU RSN Import">
            <Grid container spacing={2}>
                <Grid size={10}>
                    <Typography color="primary" variant="h4">
                        Daily EU RSN Import Report
                    </Typography>
                </Grid>
                <Grid size={2}>
                    <ExportButton
                        href={`${itemTypes.dailyEuRsnImportReport.url}?${queryString.stringify(options())}`}
                        fileName="DailyEuRsnImportReport.csv"
                        tooltipText="Download as CSV"
                    />
                </Grid>
                <Grid size={3}>
                    <DatePicker
                        label="From Date"
                        value={fromDate}
                        onChange={value => {
                            setFromDate(value);
                        }}
                    />
                </Grid>
                <Grid size={3}>
                    <DatePicker
                        label="To Date"
                        value={toDate}
                        onChange={value => {
                            setToDate(value);
                        }}
                    />
                </Grid>
                <Grid size={2} sx={{ marginTop: 4 }}>
                    <Button
                        variant="contained"
                        onClick={() => {
                            getDailyEuRsnImportReport(null, `?${queryString.stringify(options())}`);
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                <Grid size={2} sx={{ marginTop: 4 }}>
                    <ExportButton
                        href={`${itemTypes.downloadExpbookInvoices.url}?documentType=R&${returnIds.map(id => `documentNumber=${id}`).join('&')}`}
                        fileName="DailyInvoices.pdf"
                        accept="application/pdf"
                        tooltipText="Download as PDF"
                        disabled={!returnIds?.length}
                        buttonText="Download Invoices"
                    />
                </Grid>
                <Grid size={2} />
                {isLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {result && report}
            </Grid>
        </Page>
    );
}

export default DailyEuRsnImportReport;
