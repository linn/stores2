import React, { useMemo } from 'react';
import Grid from '@mui/material/Grid';
import { Loading, ReportDataGrid } from '@linn-it/linn-form-components-library';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import SearchParams from './SearchParams';

function ExportTab() {
    const {
        send: runImportReport,
        isLoading,
        result
    } = useGet(itemTypes.importBooksReport.url, true);

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
        <>
            <Grid container spacing={1}>
                <SearchParams
                    runReport={runImportReport}
                    exportUri={itemTypes.importBooksReport.url}
                    table={false}
                />
                {isLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {result != null && <Grid size={12}>{report}</Grid>}
            </Grid>
        </>
    );
}

export default ExportTab;
