import React from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import moment from 'moment';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import PropTypes from 'prop-types';

function BookedBy({
    dateBooked = null,
    bookedByName = null,
    bookUrl = null,
    onBook,
    shouldRender = true
}) {
    if (!shouldRender) {
        return <Grid size={6} />;
    }

    return (
        <>
            <Grid size={2}>
                <InputField
                    value={dateBooked ? moment(dateBooked).format('DD-MMM-YYYY') : null}
                    disabled
                    label="Date Booked"
                    propertyName="dateBooked"
                />
            </Grid>
            <Grid size={2}>
                <InputField
                    fullWidth
                    value={bookedByName}
                    disabled
                    label="Booked By"
                    propertyName="bookedByName"
                />
            </Grid>
            <Grid size={2}>
                {bookUrl && !dateBooked && (
                    <Button variant="contained" sx={{ marginTop: '30px' }} onClick={onBook}>
                        Book Req
                    </Button>
                )}
            </Grid>
        </>
    );
}

BookedBy.propTypes = {
    bookedByName: PropTypes.string,
    dateBooked: PropTypes.string,
    bookUrl: PropTypes.string,
    onBook: PropTypes.func.isRequired,
    shouldRender: PropTypes.bool
};

BookedBy.defaultProps = {
    shouldRender: true,
    bookedByName: null,
    dateBooked: null,
    bookUrl: null
};

export default BookedBy;
