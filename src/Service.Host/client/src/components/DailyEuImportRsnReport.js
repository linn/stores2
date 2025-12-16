import React, { useState, useMemo } from 'react';
import queryString from 'query-string';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { DatePicker, Loading, useGet, ReportDataGrid } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import itemTypes from '../itemTypes';
import Page from './Page';

function DailyEuImportRsnReport() {
    const [fromDate, setFromDate] = useState(new Date());
    const [toDate, setToDate] = useState(new Date());

    const options = () => ({
        fromDate: fromDate.toISOString(),
        toDate: toDate.toISOString()
    });

    const {
        send: getDailyEuImportRsnReport,
        isLoading,
        result
    } = useGet(itemTypes.dailyEuImportRsnReport.url, true);

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
        <Page>
            <Grid container spacing={2}>
                <Grid size={12}>
                    <Typography color="primary" variant="h4">
                        Daily Eu Import RSN Report
                    </Typography>
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
                <Grid size={2}>
                    <Button
                        variant="contained"
                        onClick={() => {
                            getDailyEuImportRsnReport(null, `?${queryString.stringify(options())}`);
                        }}
                    >
                        Run
                    </Button>
                </Grid>
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

export default DailyEuImportRsnReport;
