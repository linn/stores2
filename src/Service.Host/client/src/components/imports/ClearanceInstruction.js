import React, { useEffect, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import { useSearchParams } from 'react-router-dom';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import Tooltip from '@mui/material/Tooltip';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import {
    ErrorCard,
    ExportButton,
    InputField,
    Loading
} from '@linn-it/linn-form-components-library';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import Page from '../Page';

function ClearanceInstruction() {
    const [searchParams] = useSearchParams();
    const impBookId = searchParams.get('impBookId');
    const [hasFetched, setHasFetched] = useState(false);
    const [error, setError] = useState();
    const [importBooks, setImportBooks] = useState([]);
    const [options, setOptions] = useState({
        importBookId: null,
        toEmailAddress: '',
        filename: ''
    });

    const {
        send: getImportBook,
        isLoading,
        result: importBook,
        clearData
    } = useGet(itemTypes.importBooks.url, true);

    const auth = useAuth();
    const token = auth.user?.access_token;

    if (!hasFetched && token) {
        setHasFetched(true);
        getImportBook(impBookId);
    }

    useEffect(() => {
        if (importBook && !importBooks.some(i => i.id === importBook.id)) {
            if (
                importBooks.length > 0 &&
                importBook.transportBillNumber != importBooks[0].transportBillNumber
            ) {
                setError('Cannot mix AWBs');
                clearData();
            } else {
                setImportBooks([...importBooks, importBook]);
                if (!options.filename) {
                    setOptions(o => ({
                        ...o,
                        filename: `ClearanceInstruction${importBook.transportBillNumber}.pdf`
                    }));
                }
                clearData();
                setError();
            }
        }
    }, [importBook, importBooks, clearData, options]);

    const handleOptionChange = (property, newValue) => {
        if (property === 'includeCancelled') {
            setOptions(o => ({ ...o, [property]: newValue === 'Y' }));
        } else if (newValue) {
            setOptions(o => ({ ...o, [property]: newValue }));
        } else {
            const opt = { ...options };
            delete opt[property];
            setOptions(() => opt);
        }
    };

    const addClick = () => {
        if (options.impbookId && !importBooks.some(i => i.id === options.impbookId) && !isLoading) {
            getImportBook(options.impbookId);
        }
    };

    const handleDeleteRow = row => {
        setImportBooks(prev => prev.filter(r => r.id !== row.id));
    };

    const columns = [
        { field: 'id', headerName: 'ImpBook#', width: 100 },
        { field: 'currency', headerName: 'Currency', width: 110 },
        { field: 'totalImportValue', headerName: 'Total Value', width: 130 },
        {
            field: 'invoices',
            headerName: 'Invoices',
            width: 280,
            renderCell: params =>
                params.row.importBookInvoiceDetails.map(i => i.invoiceNumber).join(',')
        },
        {
            field: 'delete',
            headerName: '',
            width: 120,
            renderCell: params => (
                <Tooltip title="Delete">
                    <div>
                        <IconButton
                            aria-label="delete"
                            size="small"
                            onClick={() => handleDeleteRow(params.row)}
                        >
                            <DeleteIcon fontSize="inherit" />
                        </IconButton>
                    </div>
                </Tooltip>
            )
        }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Import Customs Clearance Instruction</Typography>
                </Grid>
                {isLoading ? (
                    <Loading />
                ) : (
                    <>
                        <Grid size={3}>
                            <InputField
                                fullWidth
                                value={options.impbookId}
                                type="number"
                                onChange={handleOptionChange}
                                label="Import Book Id"
                                propertyName="impbookId"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Button
                                onClick={addClick}
                                variant="outlined"
                                style={{ marginTop: '29px' }}
                                disabled={!options.impbookId}
                            >
                                Add
                            </Button>
                        </Grid>
                        <Grid size={7} />
                        {error && (
                            <Grid size={12}>
                                <ErrorCard errorMessage={error} />
                            </Grid>
                        )}
                    </>
                )}
                {importBooks.length > 0 && (
                    <>
                        <Grid size={12}>
                            <DataGrid
                                rows={importBooks}
                                columns={columns}
                                density="compact"
                                hideFooter
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                disabled
                                value={`${importBooks[0].carrierName}`}
                                fullWidth
                                label="Carrier"
                                propertyName="carrier"
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                disabled
                                value={importBooks[0].transportBillNumber}
                                fullWidth
                                label="AWB"
                                propertyName="awb"
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                value={options.toEmailAddress}
                                fullWidth
                                label="To Email Address"
                                propertyName="toEmailAddress"
                                onChange={handleOptionChange}
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                value={options.filename}
                                fullWidth
                                label="PDF Filename"
                                propertyName="filename"
                                onChange={handleOptionChange}
                            />
                        </Grid>
                        <Grid size={6} />
                        <Grid size={6}>
                            <ExportButton
                                buttonText="Download Instruction as PDF"
                                accept="application/pdf"
                                fileName={options.filename}
                                tooltipText="Download Import Customs Clearance Instructions as PDF"
                                href={`${itemTypes.importBooks.url}/clearance-instruction/pdf?impbooks=${importBooks.map(i => i.id).join(',')}&toEmailAddress=${options.toEmailAddress}`}
                            />
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default ClearanceInstruction;
