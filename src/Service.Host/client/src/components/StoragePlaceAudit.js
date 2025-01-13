import React, { useState, useMemo } from 'react';
import { useAuth } from 'react-oidc-context';
import { InputField, Loading, ExportButton } from '@linn-it/linn-form-components-library';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import ReportDataGrids from './ReportDataGrids';

function StoragePlaceAudit() {
    const [range, setRange] = useState(null);

    const auth = useAuth();
    const token = auth.user?.access_token;

    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.storagePlaceAudit.url);

    const handleRangeChange = (propertyName, newValue) => {
        if (newValue) {
            setRange(newValue.toUpperCase());
        } else {
            setRange(newValue);
        }
    };

    const getQueryString = () => {
        let queryString = '?';

        if (range) {
            queryString += `locationRange=${range}`;
        }

        return queryString;
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
                    <InputField
                        value={range}
                        label="Range"
                        propertyName="range"
                        onChange={handleRangeChange}
                    />
                </Grid>
                <Grid item size={7} />
                <Grid item xs={1}>
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
                <Grid item xs={1}>
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
