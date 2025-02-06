import React from 'react';
import { InputField, DatePicker } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';

function BookedBy({ dateBooked, bookedByName, shouldRender }) {
    if (!shouldRender) {
        return <Grid size={4} />;
    }

    return (
        <>
            <Grid size={2}>
                <DatePicker
                    value={dateBooked}
                    onChange={() => {}}
                    label="Date Booked"
                    propertyName="dateBooked"
                />
            </Grid>
            <Grid size={2}>
                <InputField
                    fullWidth
                    value={bookedByName}
                    onChange={() => {}}
                    label="Booked By"
                    propertyName="bookedByName"
                />
            </Grid>
        </>
    );
}

BookedBy.propTypes = {
    bookedByName: PropTypes.string.isRequired,
    dateBooked: PropTypes.string.isRequired,
    shouldRender: PropTypes.bool
};

BookedBy.defaultProps = {
    shouldRender: false
};

export default BookedBy;
