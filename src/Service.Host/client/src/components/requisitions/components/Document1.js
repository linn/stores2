import React, { useEffect } from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import itemTypes from '../../../itemTypes';
import useGet from '../../../hooks/useGet';

function Document1({
    document1,
    document1Text,
    document1Line,
    document1LineRequired,
    handleFieldChange,
    shouldRender,
    shouldEnter,
    onSelect,
    partSource
}) {
    const {
        send: fetchPurchaseOrder,
        result: purchaseOrder,
        clearData: clearPurchaseOrder
    } = useGet(itemTypes.purchaseOrder.url, true);

    const {
        send: fetchCreditNote,
        result: creditNote,
        clearData: clearCreditNote
    } = useGet(itemTypes.creditNotes.url, true);

    useEffect(() => {
        if (purchaseOrder) {
            onSelect({
                partNumber: purchaseOrder.details[0].partNumber,
                partDescription: purchaseOrder.details[0].partDescription,
                batchRef: `P${purchaseOrder.orderNumber}`,
                document1Line: 1
            });
            clearPurchaseOrder();
        }
    }, [purchaseOrder, onSelect, clearPurchaseOrder]);

    useEffect(() => {
        if (creditNote && document1Line) {
            let line = creditNote.details.find(f => f.lineNumber === document1Line);
            if (line) {
                onSelect({
                    partNumber: line.articleNumber,
                    partDescription: line.description,
                    document1Line
                });
                clearPurchaseOrder();
            }
        }
    }, [creditNote, document1Line, onSelect, clearPurchaseOrder]);

    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={4}>
                <InputField
                    value={document1}
                    type="number"
                    disabled={!shouldEnter}
                    label={document1Text}
                    onChange={handleFieldChange}
                    propertyName="document1"
                    textFieldProps={{
                        onKeyDown: data => {
                            if (data.keyCode == 13 || data.keyCode == 9) {
                                if (partSource === 'PO') {
                                    fetchPurchaseOrder(document1);
                                } else if (partSource == 'C') {
                                    fetchCreditNote(document1);
                                }
                            }
                        }
                    }}
                />
            </Grid>
            <Grid size={2}>
                {document1LineRequired !== 'N' && (
                    <InputField
                        value={document1Line}
                        type="number"
                        disabled={!shouldEnter}
                        label="Line"
                        onChange={handleFieldChange}
                        propertyName="document1Line"
                    />
                )}
            </Grid>
            <Grid size={6} />
        </>
    );
}

export default Document1;
