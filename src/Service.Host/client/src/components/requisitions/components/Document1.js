import React, { useEffect } from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import itemTypes from '../../../itemTypes';
import useGet from '../../../hooks/useGet';

function Document1({
    document1,
    document1Text,
    handleFieldChange,
    shouldRender,
    shouldEnter,
    onSelect,
    partSource
}) {
    const {
        send: fetchPurchaseOrder,
        isLoading: fetchPurchaseLoading,
        result: purchaseOrder,
        clearData: clearPurchaseOrder
    } = useGet(itemTypes.purchaseOrder.url, true);

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
                            if (data.keyCode == 13) {
                                if (partSource === 'PO') {
                                    fetchPurchaseOrder(document1);
                                }
                            }
                        }
                    }}
                />
            </Grid>
            <Grid size={8} />
        </>
    );
}

export default Document1;
