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
    const [formValues, setFormValues] = useState();

    const auth = useAuth();
    const token = auth.user?.access_token;

    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.storagePlaceAudit.url);

    const handleFieldChange = (propertyName, newValue) => {
        setFormValues(current => ({ ...current, [propertyName]: newValue }));
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
                <Grid size={6}>
                    <InputField
                        value={formValues?.name}
                        fullWidth
                        label="Name"
                        propertyName="name"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item size={4} />
                <Grid item xs={1}>
                    <Button
                        variant="contained"
                        onClick={() => {
                            getReport(null, '?locationList=P745');
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
                        }/stores2/reports/storage-place-audit/pdf?locationList=P275&locationList=P745`}
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
