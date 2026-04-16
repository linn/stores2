import React from 'react';
import Grid from '@mui/material/Grid';
import { InputField } from '@linn-it/linn-form-components-library';

function CommentsTab({ importBook, canChange, handleFieldChange }) {
    return (
        <>
            <Grid container spacing={1}>
                <Grid size={12}>
                    <InputField
                        value={importBook.comments}
                        fullWidth
                        rows={6}
                        label="Comments"
                        propertyName="comments"
                        onChange={handleFieldChange}
                        disabled={!canChange}
                    />
                </Grid>
                <Grid size={12}>
                    <InputField
                        value={importBook.clearanceComments}
                        fullWidth
                        rows={6}
                        label="Clearance Comments"
                        propertyName="clearanceComments"
                        onChange={handleFieldChange}
                        disabled={!canChange}
                    />
                </Grid>
            </Grid>
        </>
    );
}

export default CommentsTab;
