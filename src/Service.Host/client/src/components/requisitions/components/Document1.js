import React, { useEffect } from 'react';
import { InputField, utilities, LinkField } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
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
    partSource,
    onSelectPart = null,
    document1Details = null,
    storesFunction = null
}) {
    const displayDetails1 =
        document1Details && partSource === 'WO'
            ? { render: true, label: 'Work Station', value: document1Details.workStationCode }
            : { render: false, label: null, value: null };
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

    const {
        send: fetchWorksOrder,
        result: worksOrder,
        clearData: clearWorksOrder
    } = useGet(itemTypes.worksOrder.url, true);

    const {
        send: fetchPart,
        result: document1Part,
        clearData: clearDocument1Part
    } = useGet(itemTypes.parts.url, true);

    function toIntId(href) {
        if (href) {
            const segments = href.split('/');
            return Number(segments[segments.length - 1]);
        }
        return null;
    }

    useEffect(() => {
        if (purchaseOrder) {
            const docType = purchaseOrder.documentType.name;

            if (storesFunction?.code === 'RETSU') {
                const debitNote = toIntId(
                    utilities.getHref(purchaseOrder, 'ret-credit-debit-note')
                );

                onSelect({
                    partNumber: purchaseOrder.details[0].partNumber,
                    partDescription: purchaseOrder.details[0].partDescription,
                    batchRef: purchaseOrder.details[0].originalOrderNumber
                        ? `P${purchaseOrder.details[0].originalOrderNumber}`
                        : null,
                    document1Line: 1,
                    document2: debitNote,
                    document3: purchaseOrder.details[0].originalOrderNumber,
                    docType,
                    quantity: purchaseOrder.details[0].ourQty,
                    canReverse: 'Y'
                });
            } else {
                onSelect({
                    partNumber: purchaseOrder.details[0].partNumber,
                    partDescription: purchaseOrder.details[0].partDescription,
                    batchRef:
                        storesFunction?.batchRequired === 'Y'
                            ? `${docType.charAt(0)}${purchaseOrder.orderNumber}`
                            : null,
                    document1: purchaseOrder.orderNumber,
                    document1Line: 1,
                    toLocationCode:
                        storesFunction?.toLocationRequired === 'Y'
                            ? `S-SU-${purchaseOrder.supplier.id}`
                            : null,
                    docType,
                    orderDetail: purchaseOrder.details[0]
                });
            }

            clearPurchaseOrder();
        }
    }, [purchaseOrder, onSelect, clearPurchaseOrder, storesFunction]);

    useEffect(() => {
        if (creditNote && document1Line) {
            let line = creditNote.details.find(f => f.lineNumber === document1Line);
            if (line) {
                onSelect({
                    partNumber: line.articleNumber,
                    partDescription: line.description,
                    document1Line,
                    canReverse: 'Y'
                });
                clearCreditNote();
            }
        }
    }, [creditNote, document1Line, onSelect, clearCreditNote]);

    useEffect(() => {
        if (worksOrder) {
            onSelect({
                partNumber: worksOrder.partNumber,
                partDescription: worksOrder.partDescription,
                workStationCode: worksOrder.workStationCode,
                batchNumber: worksOrder.batchNumber,
                docType: worksOrder.docType,
                orderNumber: worksOrder.orderNumber,
                outstanding: worksOrder.outstanding,
                quantity: worksOrder.quantity,
                quantityBuilt: worksOrder.quantityBuilt,
                dateCancelled: worksOrder.dateCancelled,
                canReverse: 'Y'
            });

            fetchPart(null, `?searchTerm=${worksOrder.partNumber}&exactOnly=true`);
            clearWorksOrder();
        }
    }, [worksOrder, onSelect, clearWorksOrder, fetchPart]);

    useEffect(() => {
        if (document1Part && document1Part.length === 1) {
            onSelectPart(document1Part[0]);

            clearDocument1Part();
        }
    }, [document1Part, onSelectPart, clearDocument1Part]);

    if (!shouldRender) {
        return '';
    }

    const href = () => {
        switch (partSource) {
            case 'C':
                return itemTypes.creditNotes.url;
            case 'WO':
                return itemTypes.worksOrder.url;
            case 'PO':
                return itemTypes.purchaseOrder.url;
            case 'RO':
                return itemTypes.purchaseOrder.url;
            case 'CO':
                return itemTypes.purchaseOrder.url;
            default:
                return '';
        }
    };

    return (
        <>
            <Grid size={2}>
                {!shouldEnter ? (
                    <LinkField
                        value={document1}
                        label={document1Text}
                        to={`${href()}/${document1}`}
                        external
                        openLinksInNewTabs
                    />
                ) : (
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
                                    if (partSource === 'PO' || partSource === 'RO') {
                                        fetchPurchaseOrder(document1);
                                    } else if (partSource == 'C') {
                                        fetchCreditNote(document1);
                                    } else if (partSource == 'WO') {
                                        fetchWorksOrder(document1);
                                    }
                                }
                            }
                        }}
                    />
                )}
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
            <Grid size={2}>
                {displayDetails1.render && (
                    <InputField
                        value={displayDetails1.value}
                        disabled
                        label={displayDetails1.label}
                        onChange={() => {}}
                        propertyName="displayDetails1"
                    />
                )}
            </Grid>
            <Grid size={6} />
        </>
    );
}

export default Document1;
