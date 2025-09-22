import React, { useMemo, useState } from 'react';
import moment from 'moment';
import {
    Loading,
    ReportDataGrids,
    Dropdown,
    ExportButton,
    InputField,
    CheckboxWithLabel
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import queryString from 'query-string';
import Button from '@mui/material/Button';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';

const LabourHoursInStockReport = () => {
    const [options, setOptions] = useState({
        jobref: null,
        accountingCompany: 'LINN',
        includeObsolete: true
    });
    const [firstTime, setFirstTime] = useState(true);
    const {
        send: getReport,
        isLoading,
        result: reportResult
    } = useGet(itemTypes.labourHoursInStockReport.url);

    const {
        send: getTotal,
        isLoading: totalLoading,
        result: totalResult
    } = useGet(itemTypes.labourHoursInStockTotal.url);

    const {
        send: getJobrefs,
        isLoading: isJobrefsLoading,
        result: jobrefsResult
    } = useGet(itemTypes.tqmsJobrefs.url);

    const [showDetails, setShowDetails] = useState(true);

    if (firstTime) {
        getJobrefs();
        setFirstTime(false);
    }

    const reports = useMemo(
        () => (
            <>
                <Grid size={3}>{totalResult && <Typography variant="body">Total</Typography>}</Grid>
                <Grid size={9}>
                    {totalResult && (
                        <Typography variant="h4">
                            {totalResult?.total.toLocaleString()} hours
                        </Typography>
                    )}
                </Grid>
                <ReportDataGrids
                    perReportExport={false}
                    reportData={reportResult?.reportResults ?? []}
                    repeatHeaders
                    fixedRowHeight
                    showHeader
                    renderZeroes
                    showTotals={false}
                />
            </>
        ),
        [reportResult, totalResult]
    );

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Labour Hours In Stock Report">
            <Grid container spacing={3}>
                <Grid size={6}>
                    <Typography variant="h6">Labour Hours In Stock Report</Typography>
                </Grid>
                <Grid size={6}>
                    <ExportButton
                        href={`${itemTypes.labourHoursInStockReport.url}?${queryString.stringify(options)}`}
                        fileName="labourHoursExport.csv"
                        tooltipText="Download report as CSV"
                        disabled={!options.jobref}
                    />
                </Grid>
                <Grid size={4}>
                    {jobrefsResult && (
                        <Dropdown
                            value={options.jobref}
                            fullWidth
                            label="Jobref"
                            propertyName="jobref"
                            allowNoValue
                            items={jobrefsResult
                                .sort((a, b) => b.jobRef.localeCompare(a.jobRef))
                                .map(c => ({
                                    id: c.jobRef,
                                    displayText: `${moment(c.dateOfRun).format('DDMMMYY')} - ${c.jobRef}`
                                }))}
                            onChange={(propertyName, newVal) =>
                                setOptions(o => ({ ...o, [propertyName]: newVal }))
                            }
                        />
                    )}
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
                <Grid size={4}>
                    {options.includeObsolete}
                    <CheckboxWithLabel
                        label="Include Obsolete"
                        checked={options.includeObsolete}
                        onChange={(propertyName, newVal) =>
                            setOptions(o => ({ ...o, [propertyName]: newVal }))
                        }
                    />
                </Grid>
                <Grid size={3}>
                    <Button
                        variant="outlined"
                        disabled={!options.jobref}
                        onClick={() => {
                            getTotal(null, `?${queryString.stringify(options)}`);
                            if (showDetails) {
                                getReport(null, `?${queryString.stringify(options)}`);
                            }
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                <Grid size={9}>
                    <CheckboxWithLabel
                        label="Show detail"
                        checked={showDetails}
                        onChange={(propertyName, newVal) => setShowDetails(newVal)}
                    />
                </Grid>
                {isLoading || totalLoading || isJobrefsLoading ? (
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
