import React, { useMemo } from 'react';
import { Loading, ReportDataGrids, ExportButton } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';

const LabourHoursInStockReport = () => {
    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.labourHoursInLoansReport.url);

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
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Labour Hours In Loans Report">
            <Grid container spacing={3}>
                <Grid size={6}>
                    <Typography variant="h6">Labour Hours In Stock Report</Typography>
                </Grid>
                <Grid size={3}></Grid>
                <Grid size={3}>
                    <ExportButton
                        href={itemTypes.labourHoursInLoansReport.url}
                        fileName="labourHoursExport.csv"
                        tooltipText="Download report as CSV"
                    />
                </Grid>
                <Grid size={3}>
                    <Button
                        variant="outlined"
                        onClick={() => {
                            getReport(null, null);
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

export default LabourHoursInStockReport;
