import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { DatePicker, Loading, FileUploader } from '@linn-it/linn-form-components-library';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';

import Page from './Page';

function ImportBookComparer() {
    const [fromDate, setFromDate] = useState(null);
    const [toDate, setToDate] = useState(null);

    return (
        <Page>
            <Grid container spacing={2}>
                <Grid size={10}>
                    <Typography color="primary" variant="h4">
                        Import Book Comparer
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
                <Grid size={12}>
                    <FileUploader
                        // doUpload={handleUploadClick}
                        // snackbarVisible={snackbarVisible}
                        // setSnackbarVisible={setSnackbarVisible}
                        // result={result}
                        // loading={loading}
                        initiallyExpanded
                        title="Upload Invoice"
                        helperText=""
                    />
                </Grid>

                <Box sx={{ width: '100%' }} />

                <Grid size={2}>
                    <Button
                        variant="contained"
                        // onClick={() => {
                        //     setShowRunResult(true);
                        //     getOpenRsns(null, `?${buildSearchRequestsQuery}`);
                        // }}
                    >
                        Run
                    </Button>
                </Grid>

                {/* <Grid size={12}>
                    <Loading />
                </Grid> */}
                {/* {errorMessage && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={errorMessage} />
                    </Grid>
                )} */}
            </Grid>
        </Page>
    );
}

export default ImportBookComparer;
