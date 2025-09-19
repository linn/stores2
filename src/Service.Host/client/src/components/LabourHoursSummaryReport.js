import React, { useMemo, useState } from 'react';
import {
    DatePicker,
    Loading,
    ReportDataGrids,
    ExportButton,
    InputField
} from '@linn-it/linn-form-components-library';
import moment from 'moment';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import queryString from 'query-string';
import Button from '@mui/material/Button';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';

const LabourHoursSummaryReport = () => {
    const [options, setOptions] = useState({
        fromDate: moment('2025-08-01'),
        toDate: moment('2025-09-01'),
        accountingCompany: 'LINN'
    });
    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.labourHoursSummaryReport.url);

    const reportOptions = {
        ...options,
        fromDate: options.fromDate.toISOString(),
        toDate: options.toDate.toISOString()
    };

    const reports = useMemo(
        () => (
            <ReportDataGrids
                perReportExport={false}
                reportData={reportResult?.reportResults ?? []}
                repeatHeaders
                fixedRowHeight
                showHeader
                renderZeroes
                showTotals
            />
        ),
        [reportResult]
    );

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Labour Hours Summary Report">
            <Grid container spacing={3}>
                <Grid size={6}>
                    <Typography variant="h6">Labour Hours Summary Report</Typography>
                </Grid>
                <Grid size={6}>
                    <ExportButton
                        href={`${itemTypes.labourHoursSummaryReport.url}?${queryString.stringify(reportOptions)}`}
                        fileName="labourHoursSummary.csv"
                        tooltipText="Download report as CSV"
                    />
                </Grid>
                <Grid size={4}>
                    <DatePicker
                        label="From Date"
                        value={options.fromDate}
                        maxDate={options.toDate}
                        propertyName="fromDate"
                        onChange={value => setOptions(o => ({ ...o, fromDate: value }))}
                    />
                </Grid>
                <Grid size={4}>
                    <DatePicker
                        label="To Date"
                        value={options.toDate}
                        minDate={options.fromDate}
                        propertyName="toDate"
                        onChange={value => setOptions(o => ({ ...o, toDate: value }))}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        propertyName="accountingCompany"
                        label="Accounting Company"
                        value={options.accountingCompany}
                        disabled
                        fullWidth
                    />
                </Grid>
                <Grid size={3}>
                    <Button
                        variant="outlined"
                        onClick={() => {
                            getReport(null, `?${queryString.stringify(reportOptions)}`);
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                <Grid size={9}></Grid>
                {isLoading ? (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                ) : (
                    reports
                )}
            </Grid>
        </Page>
    );
};

export default LabourHoursSummaryReport;
