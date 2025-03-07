import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { DataGrid } from '@mui/x-data-grid';
import Tooltip from '@mui/material/Tooltip';
import Typography from '@mui/material/Typography';
import CloseIcon from '@mui/icons-material/Close';
import DoneIcon from '@mui/icons-material/Done';
import EmergencyIcon from '@mui/icons-material/Emergency';
import PanoramaFishEyeIcon from '@mui/icons-material/PanoramaFishEye';
import { Dropdown, InputField, Loading } from '@linn-it/linn-form-components-library';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import Page from './Page';

function StoresFunction() {
    const [hasFetched, setHasFetched] = useState(false);
    const { code } = useParams();
    const { send: getFunctionCode, isLoading, result } = useGet(itemTypes.functionCodes.url);

    if (!hasFetched) {
        setHasFetched(true);
        getFunctionCode(code);
    }

    const requiredIcon = value => {
        if (value === 'Y') {
            return (
                <Tooltip title="Required">
                    <EmergencyIcon />
                </Tooltip>
            );
        } else if (value === 'O') {
            return (
                <Tooltip title="Optional">
                    <PanoramaFishEyeIcon />
                </Tooltip>
            );
        } else if (value === 'N') {
            return (
                <Tooltip title="Not Required">
                    <CloseIcon />
                </Tooltip>
            );
        }
        return value;
    };

    const showRequired = (fieldName, value) => {
        return (
            <>
                <Grid size={2}>
                    <Typography variant="body1">{fieldName}</Typography>
                </Grid>
                <Grid size={2}>{requiredIcon(value)}</Grid>
            </>
        );
    };

    const yesNoIcon = (fieldName, boolValue, yesText = 'yes', noText = 'no') => {
        return (
            <>
                <Grid size={2}>
                    <Typography variant="body1">{fieldName}</Typography>
                </Grid>
                <Grid size={2}>
                    {boolValue ? (
                        <Tooltip title={yesText}>
                            <DoneIcon />
                        </Tooltip>
                    ) : (
                        <Tooltip title={noText}>
                            <CloseIcon />
                        </Tooltip>
                    )}
                </Grid>
            </>
        );
    };

    const columns = [
        {
            field: 'seq',
            headerName: 'Seq',
            width: 80
        },
        { field: 'reqType', headerName: 'Req Type', width: 120 },
        { field: 'transactionDefinition', headerName: 'Transaction', width: 160 },
        { field: 'transactionDescription', headerName: 'Description', width: 460 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {isLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    result && (
                        <>
                            <Grid size={12}>
                                <Typography variant="h4">{result.code}</Typography>
                            </Grid>
                            <Grid size={2}>
                                {<Typography variant="body1">Description</Typography>}
                            </Grid>
                            <Grid size={6}>
                                {<Typography variant="body1">{result.description}</Typography>}
                            </Grid>
                            {yesNoIcon(
                                'Available',
                                result.functionAvailable,
                                'Available',
                                'Not Available'
                            )}
                            <Grid size={2}>
                                {<Typography variant="body1">Function Type</Typography>}
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    fullWidth
                                    items={[
                                        { id: 'M', displayText: 'M - Manual' },
                                        { id: 'A', displayText: 'A - Automatic' },
                                        { id: 'E', displayText: 'E - External' }
                                    ]}
                                    disabled
                                    value={result.functionType}
                                    onChange={() => {}}
                                    propertyName="functionType"
                                />
                            </Grid>
                            <Grid size={2}>
                                {<Typography variant="body1">Manual Pick</Typography>}
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    fullWidth
                                    items={[
                                        { id: 'M', displayText: 'M - Manual' },
                                        { id: 'A', displayText: 'A - Automatic' },
                                        { id: 'X', displayText: 'X' }
                                    ]}
                                    disabled
                                    value={result.functionType}
                                    onChange={() => {}}
                                    propertyName="manualPickRequired"
                                />
                            </Grid>
                            <Grid size={2}>
                                {<Typography variant="body1">Cancel Function</Typography>}
                            </Grid>
                            <Grid size={2}>
                                <InputField
                                    value={result.cancelFunction}
                                    fullWidth
                                    propertyName="cancelFunction"
                                />
                            </Grid>
                            {showRequired('Dept/Nom', result.departmentNominalRequired)}
                            <Grid size={2}>
                                <Typography variant="body1">Nominal</Typography>
                            </Grid>
                            <Grid size={2}>
                                {<Typography variant="body1">{result.nominalCode}</Typography>}
                            </Grid>
                            <Grid size={4}>
                                {
                                    <Typography variant="body1">
                                        {result.nominalDescription}
                                    </Typography>
                                }
                            </Grid>
                            {yesNoIcon(
                                'Document 1',
                                result.document1Required,
                                'Required',
                                'Not Required'
                            )}
                            {yesNoIcon(
                                'Doc1 Entered',
                                result.document1Entered,
                                'Required',
                                'Not Required'
                            )}
                            <Grid size={2}>{<Typography variant="body1">Text</Typography>}</Grid>
                            <Grid size={2}>
                                {<Typography variant="body1">{result.document1Text}</Typography>}
                            </Grid>
                            {showRequired('Part Number', result.partNumberRequired)}
                            {showRequired('Quantity', result.quantityRequired)}
                            <Grid size={2}>
                                {<Typography variant="body1">Part Source</Typography>}
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    fullWidth
                                    items={[
                                        { id: 'N', displayText: 'N - Not Required' },
                                        { id: 'PO', displayText: 'PO - Purchase Order' },
                                        { id: 'C', displayText: 'C - Credit Order' },
                                        { id: 'RO', displayText: 'RO - Returns Order' },
                                        { id: 'WO', displayText: 'WO - Works Order' },
                                        { id: 'IP', displayText: 'IP - Input' },
                                        { id: 'L', displayText: 'L - Loan' }
                                    ]}
                                    disabled
                                    value={result.partSource}
                                    onChange={() => {}}
                                    propertyName="partSource"
                                />
                            </Grid>
                            {showRequired('From Location', result.fromLocationRequired)}
                            {showRequired('From State', result.fromStateRequired)}
                            {showRequired('From Stock Pool', result.fromStockPool)}
                            {showRequired('Batch', result.batchRequired)}
                            {showRequired('Batch Date', result.batchDateRequired)}
                            <Grid size={4} />
                            {showRequired('To Location', result.toLocationRequired)}
                            {showRequired('To State', result.toStateRequired)}
                            {showRequired('To Stock Pool', result.toStockPool)}
                            <Grid size={12}>
                                {result.transactionTypes && (
                                    <DataGrid
                                        getRowId={r => r.seq}
                                        rows={result.transactionTypes}
                                        columns={columns}
                                        hideFooter
                                        density="compact"
                                    />
                                )}
                            </Grid>
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}

export default StoresFunction;
