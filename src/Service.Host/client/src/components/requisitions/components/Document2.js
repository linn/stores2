import React from 'react';
import { InputField, LinkField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import itemTypes from '../../../itemTypes';

function Document2({
    document2,
    document2Text,
    document2Name,
    handleFieldChange,
    shouldRender,
    shouldEnter,
    document3
}) {
    if (!shouldRender) {
        return '';
    }

    const href = () => {
        switch (document2Name) {
            case 'DN':
                return itemTypes.debitNotes.url;
            // TODO add in case 'R' for RSN when we have a competent RSN viewer
            default:
                return '';
        }
    };

    return (
        <>
            <Grid size={4}>
                {!shouldEnter ? (
                    <LinkField
                        value={document2}
                        label={document2Text}
                        to={`${href()}/${document2}`}
                        external
                        openLinksInNewTabs
                    />
                ) : (
                    <InputField
                        value={document2}
                        type="number"
                        disabled={!shouldEnter}
                        label={document2Text}
                        onChange={handleFieldChange}
                        propertyName="document2"
                    />
                )}
            </Grid>
            {document3 ? <Grid size={4} /> : <Grid size={8} />}
        </>
    );
}

export default Document2;
