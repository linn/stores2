import React, { Fragment, useState } from 'react';
import Grid from '@mui/material/Grid';
import Expand from '@mui/icons-material/Expand';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
import {
    ErrorCard,
    InputField,
    Loading,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import { Accordion, AccordionDetails, AccordionSummary } from '@mui/material';
import { Link } from 'react-router';
import { divide } from '../../../helpers/numberUtilities';
import itemTypes from '../../../itemTypes';
import usePost from '../../../hooks/usePost';

function QcLabelPrintScreen({
    docType,
    orderNumber,
    qcState,
    partNumber,
    partDescription,
    qtyReceived,
    unitOfMeasure,
    reqNumber,
    qcInfo,
    initialNumContainers
}) {
    const [deliveryRef, setDeliveryRef] = useState('');
    const [kardexLocation, setKardexLocation] = useState('');

    const [numContainers, setNumContainers] = useState(initialNumContainers);
    const [labelLines, setLabelLines] = useState([
        {
            id: 0,
            qty: qtyReceived
        }
    ]);
    const [labelLinesExpanded, setLabelLinesExpanded] = useState(false);
    const [printerName, setPrinterName] = useState();

    const [enteredReqNumber, setEnteredReqNumber] = useState(reqNumber);

    const qtiesInvalid = () =>
        !numContainers ||
        Number(qtyReceived) !== labelLines.reduce((a, b) => Number(a) + Number(b.qty), 0);

    const handleNumContainersChange = newValue => {
        const lines = [];
        for (let index = 0; index < newValue; index += 1) {
            lines.push({
                id: index.toString(),
                qty: divide(qtyReceived, newValue)?.toDecimalPlaces(2) ?? 0
            });
        }
        setNumContainers(newValue);
        setLabelLines(lines);
    };

    const handleLabelLineQtyChange = (propertyName, newValue) => {
        const index = propertyName.replace('line ', '');
        setLabelLines(lines =>
            lines.map(line => {
                return line.id === index ? { id: index, qty: newValue } : line;
            })
        );
    };

    const {
        send: printLabels,
        isLoading: printLabelsLoading,
        postResult: printLabelsResult,
        clearPostResult: clearResult
    } = usePost(itemTypes.printQcLabels.url, true);

    const printButton = () => (
        <Button
            variant="contained"
            color="primary"
            disabled={qtiesInvalid()}
            onClick={() => {
                clearResult();
                printLabels(null, {
                    docType,
                    kardexLocation,
                    partNumber,
                    partDescription,
                    deliveryRef,
                    qcInfo,
                    qty: qtyReceived,
                    orderNumber,
                    numContainers,
                    numberOfLines: numContainers,
                    qcState: qcState,
                    reqNumber,
                    lines: labelLines.map(x => x.qty),
                    printerName
                });
            }}
        >
            Print
        </Button>
    );

    return (
        <Grid container spacing={3}>
            <Grid size={10}>
                <Typography variant="h5" gutterBottom>
                    Label Details
                </Typography>
            </Grid>
            <Grid size={2}>
                <Link to={`/requisitions/${reqNumber}`}>
                    <Typography variant="subtitle2">Back to req {reqNumber}</Typography>
                </Link>
            </Grid>
            <Grid size={12}>
                <SnackbarMessage
                    visible={printLabelsResult?.success}
                    onClose={() => {
                        clearResult();
                    }}
                    message="Printing..."
                />
            </Grid>
            <Grid item size={3}>
                <InputField
                    fullWidth
                    disabled
                    value={docType}
                    label="Document Type"
                    propertyName="docType"
                />
            </Grid>
            <Grid item size={3}>
                <InputField
                    fullWidth
                    disabled
                    value={orderNumber}
                    label="Order Number"
                    propertyName="orderNumber"
                />
            </Grid>
            <Grid item size={3}>
                <InputField
                    fullWidth
                    value={enteredReqNumber}
                    label="Req Number"
                    propertyName="reqNumber"
                    onChange={(_, newValue) => {
                        setEnteredReqNumber(newValue);
                    }}
                />
            </Grid>
            <Grid item size={3} />
            <Grid item size={3}>
                <InputField
                    fullWidth
                    disabled
                    value={qcState}
                    label="QC State"
                    propertyName="qcState"
                />
            </Grid>
            <Grid item size={9} />
            <Grid item size={4}>
                <InputField
                    fullWidth
                    disabled
                    value={partNumber}
                    label="Part"
                    propertyName="partNumber"
                />
            </Grid>
            <Grid item size={8}>
                <InputField
                    fullWidth
                    disabled
                    value={partDescription}
                    label="Part Description"
                    propertyName="description"
                />
            </Grid>
            <Grid item size={2}>
                <InputField
                    fullWidth
                    disabled
                    value={qtyReceived}
                    label="Qty Received"
                    propertyName="qtyReceived"
                />
            </Grid>
            <Grid item size={3}>
                <InputField
                    fullWidth
                    disabled
                    value={unitOfMeasure}
                    label="Unit of Measure"
                    propertyName="unitOfMeasure"
                />
            </Grid>
            <Grid item size={7} />
            <Grid item size={4}>
                <InputField
                    fullWidth
                    value={deliveryRef}
                    onChange={(_, newValue) => setDeliveryRef(newValue)}
                    label="Delivery Ref"
                    propertyName="deliveryRef"
                />
            </Grid>
            <Grid item size={8} />
            <Grid item size={6}>
                <InputField
                    fullWidth
                    disabled
                    value={qcInfo}
                    label="QC Info"
                    propertyName="qcInfo"
                />
            </Grid>
            <Grid item size={6} />
            <Grid item size={3}>
                <InputField
                    fullWidth
                    onChange={(_, newValue) => setKardexLocation(newValue)}
                    value={kardexLocation}
                    label="Kardex Location"
                    helperText="Optional - you can provide a value to print kardex labels"
                    propertyName="kardexLocation"
                />
            </Grid>
            <Grid item size={9} />
            <Grid item size={2}>
                <InputField
                    fullWidth
                    value={numContainers}
                    onChange={(_, newValue) => handleNumContainersChange(newValue)}
                    label="# Containers"
                    type="number"
                    propertyName="numberOfContainers"
                />
            </Grid>
            <Grid item size={10} />
            {!initialNumContainers && (
                <Grid item size={12}>
                    <InputField
                        value={printerName}
                        onChange={(_, newValue) => setPrinterName(newValue)}
                        label="Printer"
                        propertyName="printerName"
                    />
                </Grid>
            )}
            <Grid item size={2}>
                {qtiesInvalid() ? (
                    <Tooltip title="Values entered don't add up to Qty Received">
                        <span>{printButton()} </span>
                    </Tooltip>
                ) : (
                    printButton()
                )}
            </Grid>

            <Grid item size={10} />
            <Grid item size={12}>
                <Accordion expanded={labelLinesExpanded} data-testid="quantitiesExpansionPanel">
                    <AccordionSummary
                        onClick={() => setLabelLinesExpanded(!labelLinesExpanded)}
                        expandIcon={<Expand />}
                        aria-controls="panel1a-content"
                        id="panel1a-header"
                    >
                        <Typography>Set Qty Split Across Labels</Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                        <Grid container spacing={3}>
                            <>
                                {numContainers &&
                                    labelLines.map(l => (
                                        <Fragment key={l.id}>
                                            <Grid item size={2}>
                                                <InputField
                                                    fullWidth
                                                    value={l.qty}
                                                    onChange={(propertyName, newValue) =>
                                                        handleLabelLineQtyChange(
                                                            propertyName,
                                                            newValue
                                                        )
                                                    }
                                                    label={Number(l.id) + 1}
                                                    type="number"
                                                    propertyName={`line ${l.id}`}
                                                />
                                            </Grid>
                                            <Grid item size={10} />
                                        </Fragment>
                                    ))}
                            </>
                        </Grid>
                    </AccordionDetails>
                </Accordion>
            </Grid>

            <Grid item size={12}>
                {printLabelsResult?.success === false && (
                    <ErrorCard errorMessage={printLabelsResult?.message} />
                )}
                {printLabelsLoading && <Loading />}
            </Grid>
        </Grid>
    );
}

export default QcLabelPrintScreen;
