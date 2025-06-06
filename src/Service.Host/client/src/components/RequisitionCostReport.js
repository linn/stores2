import React, { useState, useMemo } from 'react';
import { useParams } from 'react-router-dom';
import { Loading, InputField, LinkField, ErrorCard } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';
import ReportDataGrids from './ReportDataGrids';

function RequisitionCostReport() {
    const { reqNumber } = useParams();

    const [reqNo, setReqNo] = useState(null);
    const [hasFetched, setHasFetched] = useState(false);

    const {
        send: getReport,
        isLoading,
        result: reportResult,
        errorMessage: reportError
    } = useGet(itemTypes.requisitionCostReport.url);

    if (reqNumber && !hasFetched) {
        setHasFetched(true);
        setReqNo(reqNumber);
        getReport(null, `?reqNumber=${reqNumber}`);
    }

    const reports = useMemo(
        () => (
            <ReportDataGrids
                perReportExport
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
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Requisition Cost Report</Typography>
                </Grid>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={reqNo}
                        onChange={(_, value) => setReqNo(value)}
                        label="Req Number"
                        propertyName="reqNo"
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        style={{ marginTop: '30px' }}
                        disabled={isLoading || !reqNo}
                        variant="contained"
                        onClick={() => {
                            setHasFetched(true);
                            getReport(null, `?reqNumber=${reqNo}`);
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                <Grid size={6} />
                <Grid size={2}>
                    <LinkField
                        to={`/requisitions/${reqNo}`}
                        disabled={!reqNo}
                        external={false}
                        value={`Back To Requisition ${reqNo}`}
                        label=""
                    />
                </Grid>
                {reportError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={reportError} />
                    </Grid>
                )}
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
}

export default RequisitionCostReport;
