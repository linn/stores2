import React, { useState } from 'react';
import {
    InputField,
    Loading,
    ErrorCard,
    ExportButton
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';

function PackingListViewer() {
    const [consignmentNumber, setConsignmentNumber] = useState('');

    const {
        send: getConsignment,
        isLoading,
        result: consignment,
        errorMessage
    } = useGet(itemTypes.localConsignments.url);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">View / Download Packing List</Typography>
                </Grid>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={consignmentNumber}
                        onChange={(_, value) => setConsignmentNumber(value)}
                        label="Consignment Number"
                        propertyName="consignmentNumber"
                        textFieldProps={{
                            onKeyDown: data => {
                                if (data.keyCode === 9 || data.keyCode === 13) {
                                    getConsignment(consignmentNumber);
                                }
                            }
                        }}
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        style={{ marginTop: '30px' }}
                        variant="contained"
                        disabled={isLoading || !consignmentNumber}
                        onClick={() => getConsignment(consignmentNumber)}
                    >
                        Go
                    </Button>
                </Grid>
                <Grid size={8} style={{ marginTop: '30px' }}>
                    {isLoading && <Loading />}
                    {errorMessage && <ErrorCard errorMessage={errorMessage} />}
                    {consignment && (
                        <Typography variant="h6">
                            Found consignment {consignmentNumber} for {consignment.salesAccountName}
                        </Typography>
                    )}
                </Grid>
                <Grid size={1} style={{ marginTop: '8px' }}>
                    <Button
                        variant="contained"
                        disabled={!consignment}
                        onClick={() =>
                            window.open(
                                `${config.appRoot}/stores2/consignments/${consignmentNumber}/packing-list/view`,
                                '_blank'
                            )
                        }
                    >
                        View
                    </Button>
                </Grid>
                <Grid size={2} style={{ marginTop: '8px' }}>
                    <ExportButton
                        buttonText="Download PDF"
                        disabled={!consignment}
                        accept="application/pdf"
                        fileName={`packing-list-${consignmentNumber}.pdf`}
                        tooltipText="Download packing list as PDF"
                        href={`${config.appRoot}/stores2/consignments/${consignmentNumber}/packing-list/pdf`}
                    />
                </Grid>
                <Grid size={9} />
            </Grid>
        </Page>
    );
}

export default PackingListViewer;
