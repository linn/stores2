import React, { useState, useMemo } from 'react';
import queryString from 'query-string';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { DatePicker, Loading, useGet, ReportDataGrid } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import itemTypes from '../itemTypes';
import Page from './Page';

function DailyEuDispatchReport() {
    const [fromDate, setFromDate] = useState(new Date());
    const [toDate, setToDate] = useState(new Date());

    const options = () => ({
        fromDate: fromDate.toISOString(),
        toDate: toDate.toISOString()
    });

    const {
        send: getDailyEuDespatchReport,
        isLoading,
        result
    } = useGet(itemTypes.DailyEuDispatchReport.url, true);

    const report = useMemo(() => {
        if (!result || !Array.isArray(result.reportResults) || !result.reportResults[0]) {
            return null;
        }

        const reportData = result.reportResults[0];

        // Sanitize the data to prevent undefined/null values
        const sanitizedReport = {
            ...reportData,
            rows: (reportData.rows || []).map(row => {
                const sanitizedRow = {};
                Object.keys(row).forEach(key => {
                    // Replace undefined/null with empty string or 0 for numbers
                    const value = row[key];
                    if (value === null || value === undefined) {
                        sanitizedRow[key] = '';
                    } else if (typeof value === 'number' && isNaN(value)) {
                        sanitizedRow[key] = 0;
                    } else {
                        sanitizedRow[key] = value;
                    }
                });
                return sanitizedRow;
            }),
            // Also sanitize column definitions if they exist
            columns:
                reportData.columns?.map(col => ({
                    ...col,
                    valueFormatter: col.valueFormatter || (params => params?.value ?? '')
                })) || []
        };

        console.log(sanitizedReport ?? '');

        return (
            <ReportDataGrid
                report={sanitizedReport?.results}
                fixedRowHeight
                showHeader={true}
                renderZeroes
                showTotals={false}
            />
        );
    }, [result]);

    return (
        <Page>
            <Grid container spacing={2}>
                <Grid size={12}>
                    <Typography color="primary" variant="h4">
                        Daily Eu Dispatch Report
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
                            getDailyEuDespatchReport(null, `?${queryString.stringify(options())}`);
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
                {result && <Grid size={12}>{report}</Grid>}
            </Grid>
        </Page>
    );
}

export default DailyEuDispatchReport;
