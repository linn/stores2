import React from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import moment from 'moment';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

function AuthBy({
    dateAuthorised,
    authorisedByName,
    authoriseUrl = null,
    onAuthorise,
    shouldRender = true
}) {
    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={2}>
                <InputField
                    value={dateAuthorised ? moment(dateAuthorised).format('DD-MMM-YYYY') : null}
                    disabled
                    label="Date Authd"
                    propertyName="dateBooked"
                />
            </Grid>
            <Grid size={4}>
                <InputField
                    fullWidth
                    value={authorisedByName}
                    onChange={() => {}}
                    label="Auth By"
                    propertyName="authorisedByName"
                />
            </Grid>
            <Grid size={6}>
                {authoriseUrl && !dateAuthorised && (
                    <Button variant="contained" sx={{ marginTop: '30px' }} onClick={onAuthorise}>
                        Authorise Req
                    </Button>
                )}
            </Grid>
        </>
    );
}

export default AuthBy;
