import React from 'react';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import { InputField } from '@linn-it/linn-form-components-library';

function HistoryTab({ importBook }) {
    return (
        <>
            <Grid container spacing={1}>
                <Grid size={6}>
                    <InputField
                        disabled
                        value={
                            importBook.dateCreated
                                ? moment(importBook.dateCreated).format('DD-MMM-YY HH:mm')
                                : ''
                        }
                        fullWidth
                        label="Date Created"
                        propertyName="dateCreated"
                    />
                </Grid>
                <Grid size={6}>
                    <InputField
                        disabled
                        value={importBook.createdByName}
                        fullWidth
                        label="Created By"
                        propertyName="createdByName"
                    />
                </Grid>
                <Grid size={6}>
                    <InputField
                        disabled
                        value={
                            importBook.dateReceived
                                ? moment(importBook.dateReceived).format('DD-MMM-YY HH:mm')
                                : ''
                        }
                        fullWidth
                        label="Date Received"
                        propertyName="dateReceived"
                    />
                </Grid>
            </Grid>
        </>
    );
}

export default HistoryTab;
