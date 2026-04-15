import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Grid from '@mui/material/Grid';
import CheckIcon from '@mui/icons-material/Check';
import DeleteIcon from '@mui/icons-material/Delete';
import Tooltip from '@mui/material/Tooltip';
import { DataGrid } from '@mui/x-data-grid';
import {
    Dropdown,
    ErrorCard,
    InputField,
    Loading,
    Search,
    utilities
} from '@linn-it/linn-form-components-library';
import { useNavigate } from 'react-router-dom';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import useSearch from '../../hooks/useSearch';

function CreateTab() {
    const navigate = useNavigate();
    const [importType, setImportType] = useState('RSN');
    const [rsnNumber, setRsnNumber] = useState();
    const [purchaseOrderNumber, setPurchaseOrderNumber] = useState();
    const [error, setError] = useState();
    const [rsns, setRsns] = useState([]);
    const [purchaseOrders, setPurchaseOrders] = useState([]);
    const [supplierId, setSupplierId] = useState();
    const [supplier, setSupplier] = useState();

    const {
        send: getRsn,
        isLoading: rsnLoading,
        result: rsnResult,
        clearData: clearRsnData
    } = useGet(itemTypes.rsns.url, true);

    const {
        send: getPurchaseOrder,
        isLoading: purchaseOrderLoading,
        result: purchaseOrder,
        clearData: clearPurchaseOrderData
    } = useGet(itemTypes.purchaseOrders.url, true);

    const {
        search: searchSuppliers,
        results: supplierSearchResults,
        loading: supplierSearchLoading,
        clear: clearSupplierSearch
    } = useSearch(itemTypes.suppliers.url, 'id', 'id', 'name');

    const handleRsnChange = (property, newValue) => {
        setRsnNumber(newValue);
        if (rsnResult) {
            clearRsnData();
        }
        if (error) {
            setError(null);
        }
    };

    const handlePurchaseOrderChange = (property, newValue) => {
        setPurchaseOrderNumber(newValue);
        if (purchaseOrder) {
            clearPurchaseOrderData();
        }
        if (error) {
            setError(null);
        }
    };

    const handleSupplierSelect = selected => {
        if (selected) {
            if (selected.countryCode === 'GB') {
                setError('Not an import');
            } else {
                setSupplier({
                    id: selected.id,
                    name: selected.description,
                    country: selected.countryCode
                });
            }
        }
    };

    const handleImportTypeChange = (_, newValue) => {
        setImportType(newValue);
        setRsnNumber(null);
        setRsns([]);
        setPurchaseOrderNumber(null);
        setPurchaseOrders([]);
        setSupplierId(null);
        setSupplier(null);
        setError(null);
    };

    const addClick = () => {
        if (
            importType === 'RSN' &&
            rsnNumber &&
            !rsns.some(r => r.rsnNumber === rsnNumber) &&
            !rsnLoading
        ) {
            getRsn(rsnNumber);
        } else if (
            importType === 'PO' &&
            purchaseOrderNumber &&
            !purchaseOrders.some(p => p.orderNumber === purchaseOrderNumber) &&
            !purchaseOrderLoading
        ) {
            getPurchaseOrder(purchaseOrderNumber);
        }
    };

    const handleDeleteRow = row => {
        if (importType === 'RSN') {
            setRsns(prev => prev.filter(r => r.rsnNumber !== row.rsnNumber));
        } else if (importType === 'PO') {
            setPurchaseOrders(prev => prev.filter(p => p.orderNumber !== row.orderNumber));
        }
    };

    useEffect(() => {
        if (rsnResult && !rsns.some(r => r.rsnNumber === rsnNumber)) {
            if (rsnResult.salesOutlet) {
                if (rsnResult.salesOutlet.countryCode === 'GB') {
                    setError('Not an import');
                    clearRsnData();
                } else if (rsns.length > 0 && rsnResult.salesOutlet != rsns[0].salesOutlet) {
                    setError('Cannot mix sales outlets');
                    clearRsnData();
                } else if (utilities.getHref(rsnResult, 'import-book')) {
                    setError('Already has import book');
                    clearRsnData();
                } else {
                    setRsns([...rsns, rsnResult]);
                    setRsnNumber(null);
                    clearRsnData();
                }
            }
        }
    }, [rsnResult, rsnNumber, rsns, clearRsnData]);

    const rsnColumns = [
        { field: 'rsnNumber', headerName: 'RSN Number', width: 110 },
        { field: 'articleNumber', headerName: 'Article Number', width: 140 },
        {
            field: 'ipr',
            headerName: 'IPR',
            width: 80,
            renderCell: params => (params.row.ipr ? <CheckIcon /> : null)
        },
        {
            field: 'returnReason',
            headerName: 'Return Reason',
            width: 180,
            renderCell: params => params.row.allegedReason?.reasonCategory
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

    const salesOutlet = () => {
        if (rsns.length > 0 && rsns[0].salesOutlet) {
            return rsns[0].salesOutlet;
        }
        if (rsnResult && rsnResult.salesOutlet) {
            return rsnResult.salesOutlet;
        }
        return null;
    };

    const poColumns = [
        { field: 'orderNumber', headerName: 'Order Number', width: 110 },
        { field: 'orderDescription', headerName: 'Order Description', width: 500 },
        {
            field: 'delete',
            headerName: '',
            width: 60,
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

    const createUrl = () => {
        if (importType === 'RSN') {
            return `create?rsnNumbers=${rsns.map(r => r.rsnNumber).join(',')}`;
        } else if (importType === 'PO') {
            return `create?purchaseOrderNumbers=${purchaseOrders
                .map(p => p.orderNumber)
                .join(',')}`;
        }
        return `create?supplierId=${supplier?.id}`;
    };

    useEffect(() => {
        if (purchaseOrder && !purchaseOrders.some(r => r.orderNumber === purchaseOrderNumber)) {
            if (purchaseOrder.supplier) {
                if (purchaseOrder.supplier.country === 'GB') {
                    setError('Not an import');
                    clearPurchaseOrderData();
                } else if (
                    purchaseOrders.length > 0 &&
                    purchaseOrder.supplier != purchaseOrders[0].supplier
                ) {
                    setError('Cannot mix suppliers');
                    clearPurchaseOrderData();
                } else {
                    if (!supplier) {
                        setSupplier(purchaseOrder.supplier);
                    }
                    setPurchaseOrders([...purchaseOrders, purchaseOrder]);
                    setPurchaseOrderNumber(null);
                    clearPurchaseOrderData();
                }
            }
        }
    }, [purchaseOrder, purchaseOrderNumber, purchaseOrders, clearPurchaseOrderData, supplier]);

    return (
        <Grid container spacing={3}>
            <Grid size={3}>
                <Dropdown
                    value={importType}
                    fullWidth
                    label="Import Type"
                    propertyName="importType"
                    allowNoValue
                    items={['RSN', 'PO', 'RETURNS', 'SUNDRY', 'SAMPLES']}
                    onChange={handleImportTypeChange}
                />
            </Grid>
            <Grid size={3}>
                {importType === 'RSN' && (
                    <InputField
                        fullWidth
                        value={rsnNumber}
                        type="number"
                        onChange={handleRsnChange}
                        label="RSN Number"
                        propertyName="rsnNumber"
                    />
                )}
                {importType === 'PO' && (
                    <InputField
                        fullWidth
                        value={purchaseOrderNumber}
                        type="number"
                        onChange={handlePurchaseOrderChange}
                        label="Purchase Order Number"
                        propertyName="purchaseOrderNumber"
                    />
                )}
                {importType !== 'RSN' && importType !== 'PO' && (
                    <Search
                        propertyName="supplierId"
                        type="number"
                        label="Supplier"
                        resultsInModal
                        resultLimit={100}
                        value={supplierId}
                        loading={supplierSearchLoading}
                        handleValueChange={(_, newValue) => setSupplierId(newValue)}
                        search={searchSuppliers}
                        searchResults={supplierSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleSupplierSelect}
                        clearSearch={clearSupplierSearch}
                    />
                )}
            </Grid>
            <Grid size={2}>
                {(importType === 'RSN' || importType === 'PO') && (
                    <Button
                        onClick={addClick}
                        variant="outlined"
                        style={{ marginTop: '29px' }}
                        disabled={
                            (importType === 'RSN' && (!rsnNumber || rsnLoading)) ||
                            (importType === 'PO' && (!purchaseOrderNumber || purchaseOrderLoading))
                        }
                    >
                        Add
                    </Button>
                )}
            </Grid>
            <Grid size={4}></Grid>
            {(rsnLoading || purchaseOrderLoading) && (
                <Grid size={12}>
                    <Loading />
                </Grid>
            )}
            {salesOutlet() && (
                <>
                    <Grid size={3}>
                        <InputField
                            disabled
                            value={` ${salesOutlet().accountId}/${salesOutlet().outletNumber}`}
                            fullWidth
                            label="Outlet"
                            propertyName="outlet"
                        />
                    </Grid>
                    <Grid size={6}>
                        <InputField
                            disabled
                            value={salesOutlet().name}
                            fullWidth
                            label="Name"
                            propertyName="outletName"
                        />
                    </Grid>
                    <Grid size={3}>
                        <InputField
                            disabled
                            value={salesOutlet().countryName}
                            fullWidth
                            label="Country"
                            propertyName="outletCountry"
                        />
                    </Grid>
                </>
            )}
            {supplier && (
                <>
                    <Grid size={3}>
                        <InputField
                            disabled
                            value={supplier.id}
                            fullWidth
                            label="Supplier Id"
                            propertyName="supplierId"
                        />
                    </Grid>
                    <Grid size={6}>
                        <InputField
                            disabled
                            value={supplier.name}
                            fullWidth
                            label="Name"
                            propertyName="supplierName"
                        />
                    </Grid>
                    <Grid size={3}>
                        <InputField
                            disabled
                            value={supplier.country}
                            fullWidth
                            label="Country"
                            propertyName="supplierCountry"
                        />
                    </Grid>
                </>
            )}
            {error && (
                <Grid size={12}>
                    <ErrorCard errorMessage={error} />
                </Grid>
            )}
            {rsns.length > 0 && (
                <>
                    <Grid size={12}>
                        <DataGrid
                            getRowId={r => r.rsnNumber}
                            rows={rsns}
                            columns={rsnColumns}
                            density="compact"
                            hideFooter
                        />
                    </Grid>
                </>
            )}
            {purchaseOrders.length > 0 && (
                <>
                    <Grid size={12}>
                        <DataGrid
                            rows={purchaseOrders.map(p => ({
                                id: p.orderNumber,
                                orderNumber: p.orderNumber,
                                orderDescription: p.details[0].suppliersDesignation
                            }))}
                            columns={poColumns}
                            density="compact"
                            hideFooter
                        />
                    </Grid>
                </>
            )}
            {(rsns.length > 0 || supplier) && (
                <Grid size={6}>
                    <Button
                        onClick={() => navigate(createUrl())}
                        variant="contained"
                        color="primary"
                    >
                        Create
                    </Button>
                </Grid>
            )}
        </Grid>
    );
}

export default CreateTab;
