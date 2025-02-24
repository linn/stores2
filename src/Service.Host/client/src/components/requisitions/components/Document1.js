import React from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';

function Document1({ document1, document1Text, handleFieldChange, shouldRender, shouldEnter }) {
    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={4}>
                <InputField
                    value={document1}
                    disabled={!shouldEnter}
                    label={document1Text}
                    onChange={handleFieldChange}
                    propertyName="document1"
                />
            </Grid>
            <Grid size={8} />
        </>
    );
}

export default Document1;
