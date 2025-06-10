import React from 'react';
import { LinkField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import itemTypes from '../../../itemTypes';

function Document3({ document3, storesFunction }) {
    if (!document3) {
        return '';
    }

    // only other recent func code with document 3 is DINVOICE and it is a SOS job id who cares!!
    if (storesFunction?.code != 'RETSU') {
        return <Grid size={4} />;
    }

    return (
        <Grid size={4}>
            <LinkField
                value={document3}
                label="Orig Order No"
                to={`${itemTypes.purchaseOrders.url}/${document3}`}
                external
                openLinksInNewTabs
            />
        </Grid>
    );
}

export default Document3;
