import React from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import { DatePicker, InputField } from '@linn-it/linn-form-components-library';

function HistoryTab({ importBook, canChange, handleFieldChange }) {
    const doCancel = () => {
        if (!importBook.cancelled) {
            handleFieldChange('cancelled', true);
            handleFieldChange('dateCancelled', new Date());
            handleFieldChange('cancelledByName', 'You');
        }
    };

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
                <Grid size={4}>
                    {importBook.cancelled ? (
                        <InputField
                            disabled
                            value={importBook.cancelledByName}
                            fullWidth
                            label="Cancelled By"
                            propertyName="cancelledByName"
                        />
                    ) : (
                        <Button
                            onClick={doCancel}
                            variant="outlined"
                            style={{ marginTop: '29px' }}
                            disabled={!canChange}
                        >
                            Cancel
                        </Button>
                    )}
                </Grid>
                <Grid size={4}>
                    {importBook.cancelled && (
                        <InputField
                            value={importBook.cancelledReason}
                            fullWidth
                            label="Cancelled Reason"
                            propertyName="cancelledReason"
                            onChange={handleFieldChange}
                            disabled={!canChange}
                        />
                    )}
                </Grid>
            </Grid>
        </>
    );
}

export default HistoryTab;
