import React from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';

function Document2({ document2, document2Text, handleFieldChange, shouldRender, shouldEnter }) {
    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={4}>
                <InputField
                    value={document2}
                    type="number"
                    disabled={!shouldEnter}
                    label={document2Text}
                    onChange={handleFieldChange}
                    propertyName="document2"
                />
            </Grid>
            <Grid size={8} />
        </>
    );
}

export default Document2;
