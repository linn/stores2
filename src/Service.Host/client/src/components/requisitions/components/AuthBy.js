import React from 'react';
import { InputField, DatePicker } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';

function AuthBy({ dateAuthorised, authorisedByName, shouldRender = true }) {
    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={2}>
                <InputField
                    fullWidth
                    value={authorisedByName}
                    onChange={() => {}}
                    label="Auth By"
                    propertyName="authorisedByName"
                />
            </Grid>
            <Grid size={2}>
                <DatePicker
                    value={dateAuthorised}
                    onChange={() => {}}
                    label="Date Authd"
                    propertyName="dateAuthorised"
                />
            </Grid>
            <Grid size={8} />
        </>
    );
}

export default AuthBy;
