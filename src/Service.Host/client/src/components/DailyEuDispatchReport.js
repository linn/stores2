import React, { useState, useMemo, useEffect } from 'react';
import queryString from 'query-string';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import {
    DatePicker,
    Loading,
    useGet,
    ExportButton,
    ReportDataGrid
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import itemTypes from '../itemTypes';
import config from '../config';
import Page from './Page';

function DailyEuDispatchReport() {
    const [fromDate, setFromDate] = useState(new Date());
    const [toDate, setToDate] = useState(new Date());
    const [exportBookIds, setExportBookIds] = useState([]);
    const [consignmentNumbers, setConsignmentNumbers] = useState([]);

    const options = () => ({
        fromDate: fromDate.toISOString(),
        toDate: toDate.toISOString()
    });

    const {
        send: getDailyEuDispatchReport,
        isLoading,
        result
    } = useGet(itemTypes.dailyEuDispatchReport.url, true);

    useEffect(() => {
        if (!result || !result.reportResults || result.reportResults.length === 0) {
            setExportBookIds([]);
            setConsignmentNumbers([]);
        } else {
            var resultExportBookValues = result.reportResults[0].results
                .map(a => a.values)
                .map(row => row[2]?.textDisplayValue);

            if (!resultExportBookValues || resultExportBookValues.length === 0) {
                setExportBookIds([]);
                return;
            }

            setExportBookIds([...new Set(resultExportBookValues)]);

            var resultConsignmentValues = result.reportResults[0].results
                .map(a => a.values)
                .map(row => row[19]?.textDisplayValue);

            if (!resultConsignmentValues || resultConsignmentValues.length === 0) {
                setConsignmentNumbers([]);
                return;
            }

            setConsignmentNumbers([...new Set(resultConsignmentValues)]);
        }
    }, [result]);

    const report = useMemo(() => {
        return (
            <ReportDataGrid
                report={result?.reportResults[0]}
                fixedRowHeight
                showHeader={true}
                renderZeroes
                showTotals={false}
            />
        );
    }, [result]);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Daily EU Dispatch Report">
            <Grid container spacing={2}>
                <Grid size={9}>
                    <Typography color="primary" variant="h4">
                        Daily EU Dispatch Report
                    </Typography>
                </Grid>
                <Grid size={3}>
                    <ExportButton
                        href={`${itemTypes.dailyEuDispatchReport.url}?${queryString.stringify(options())}`}
                        fileName="DailyEuDispatchReport.csv"
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
                            getDailyEuDispatchReport(null, `?${queryString.stringify(options())}`);
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                <Grid size={2} sx={{ marginTop: 4 }}>
                    <ExportButton
                        href={`${itemTypes.downloadExpbookInvoices.url}?documentType=E&${exportBookIds.map(id => `documentNumber=${id}`).join('&')}`}
                        fileName="DailyInvoices.pdf"
                        accept="application/pdf"
                        tooltipText="Download export book invoices as PDF"
                        disabled={!exportBookIds?.length}
                        buttonText="Download Invoices"
                    />
                </Grid>
                <Grid size={2} sx={{ marginTop: 4 }}>
                    <ExportButton
                        href={`${itemTypes.localConsignments.url}/multiple-packing-lists/pdf?${consignmentNumbers.map(id => `consignmentNumber=${id}`).join('&')}`}
                        fileName="PackingLists.pdf"
                        accept="application/pdf"
                        tooltipText="Download packing lists as PDF"
                        disabled={!consignmentNumbers?.length}
                        buttonText="Packing Lists"
                    />
                </Grid>
                {isLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {result != null && <Grid size={12}>{report}</Grid>}
            </Grid>
        </Page>
    );
}

export default DailyEuDispatchReport;
