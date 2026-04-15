import React from 'react';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import { DatePicker, InputField } from '@linn-it/linn-form-components-library';

function HistoryTab({ importBook, canChange, handleFieldChange }) {
    return (
        <>
            <Grid container spacing={1}>
                <Grid size={4}>
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
                <Grid size={4}>
                    <InputField
                        disabled
                        value={importBook.createdByName}
                        fullWidth
                        label="Created By"
                        propertyName="createdByName"
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        disabled
                        value={importBook.status}
                        fullWidth
                        label="Status"
                        propertyName="status"
                    />
                </Grid>
                <Grid size={4}>
                    <DatePicker
                        value={importBook.dateInstructionSent}
                        label="Date Instruction Sent"
                        propertyName="dateInstructionSent"
                        onChange={date => handleFieldChange('dateInstructionSent', date)}
                        disabled={!canChange}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        disabled
                        value={
                            importBook.customsEntryCodeDate
                                ? moment(importBook.customsEntryCodeDate).format('DD-MMM-YY HH:mm')
                                : ''
                        }
                        fullWidth
                        label="Customs Entry Date"
                        propertyName="customsEntryCodeDate"
                    />
                </Grid>
                <Grid size={4}>
                    <DatePicker
                        value={importBook.dateReceived}
                        label="Date Received"
                        propertyName="dateReceived"
                        onChange={date => handleFieldChange('dateReceived', date)}
                        disabled={!canChange}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        disabled
                        value={
                            importBook.dateCancelled
                                ? moment(importBook.dateCancelled).format('DD-MMM-YY HH:mm')
                                : ''
                        }
                        fullWidth
                        label="Date Cancelled"
                        propertyName="dateCancelled"
                    />
                </Grid>
            </Grid>
        </>
    );
}

export default HistoryTab;
