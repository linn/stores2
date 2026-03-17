import React from 'react';
import Grid from '@mui/material/Grid';
import { DatePicker, InputField, Search } from '@linn-it/linn-form-components-library';
import itemTypes from '../../itemTypes';
import useSearch from '../../hooks/useSearch';

import InvoiceDetails from './InvoiceDetails';
import OrderDetails from './OrderDetails';

function MainTab({ importBook, countries, canChange, handleFieldChange, handleNumberFieldChange }) {
    const {
        search: searchSuppliers,
        results: supplierSearchResults,
        loading: supplierSearchLoading,
        clear: clearSupplierSearch
    } = useSearch(itemTypes.suppliers.url, 'id', 'id', 'name');

    const {
        search: searchCarriers,
        results: carrierSearchResults,
        loading: carrierSearchLoading,
        clear: clearCarrierSearch
    } = useSearch(itemTypes.approvedCarriers.url, 'id', 'id', 'name');

    const handleCarrierSelect = selected => {
        handleFieldChange('carrierId', selected.id);
        handleFieldChange('carrierName', selected.description);
    };

    const handleSupplierSelect = selected => {
        handleFieldChange('supplierId', selected.id);
        handleFieldChange('supplierName', selected.description);
        const country = countries.find(c => c.countryCode === selected.countryCode);
        handleFieldChange('supplierCountry', country);
    };

    return (
        <Grid container spacing={1}>
            <Grid size={3}>
                <InputField
                    value={importBook.transportBillNumber}
                    fullWidth
                    label="Transport Num/AWB"
                    propertyName="transportBillNumber"
                    onChange={handleFieldChange}
                    disabled={!canChange}
                />
            </Grid>
            {importBook.customsEntryCodePrefix && (
                <Grid size={2}>
                    <InputField
                        value={importBook.customsEntryCodePrefix}
                        fullWidth
                        label="Prefix"
                        propertyName="customsEntryCodePrefix"
                        onChange={handleFieldChange}
                        disabled={!canChange}
                    />
                </Grid>
            )}
            <Grid size={importBook.customsEntryCodePrefix ? 4 : 6}>
                <InputField
                    value={importBook.customsEntryCode}
                    fullWidth
                    label="Customs Entry Code"
                    propertyName="customsEntryCode"
                    onChange={handleFieldChange}
                    disabled={!canChange}
                />
            </Grid>
            <Grid size={3}>
                <DatePicker
                    value={importBook.customsEntryCodeDate}
                    label="Customs Entry Date"
                    propertyName="customsEntryCodeDate"
                    onChange={handleFieldChange}
                    disabled={!canChange}
                />
            </Grid>
            <Grid size={3}>
                {canChange ? (
                    <Search
                        propertyName="carrierId"
                        type="number"
                        label="Carrier"
                        resultsInModal
                        resultLimit={100}
                        value={importBook.carrierId}
                        loading={carrierSearchLoading}
                        handleValueChange={handleFieldChange}
                        search={searchCarriers}
                        searchResults={carrierSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleCarrierSelect}
                        clearSearch={clearCarrierSearch}
                    />
                ) : (
                    <InputField
                        disabled
                        value={importBook.carrierId}
                        fullWidth
                        label="Carrier"
                        propertyName="carrierId"
                    />
                )}
            </Grid>
            <Grid size={6}>
                <InputField
                    disabled
                    value={importBook.carrierName}
                    fullWidth
                    label="Carrier Name"
                    propertyName="carrierName"
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    value={importBook.currency}
                    fullWidth
                    label="Currency"
                    propertyName="currency"
                    onChange={handleFieldChange}
                    disabled={!canChange}
                />
            </Grid>
            <Grid size={3}>
                {canChange ? (
                    <Search
                        propertyName="supplierId"
                        type="number"
                        label="Supplier"
                        resultsInModal
                        resultLimit={100}
                        value={importBook.supplierId}
                        loading={supplierSearchLoading}
                        handleValueChange={handleFieldChange}
                        search={searchSuppliers}
                        searchResults={supplierSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handleSupplierSelect}
                        clearSearch={clearSupplierSearch}
                    />
                ) : (
                    <InputField
                        disabled
                        value={importBook.supplierId}
                        fullWidth
                        label="Supplier"
                        propertyName="supplierId"
                    />
                )}
            </Grid>
            <Grid size={6}>
                <InputField
                    disabled
                    value={importBook.supplierName}
                    fullWidth
                    label="Supplier Name"
                    propertyName="supplierName"
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    disabled
                    value={importBook.supplierCountry?.name}
                    fullWidth
                    label="Country"
                    propertyName="supplierCountry"
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    value={importBook.exchangeRate}
                    fullWidth
                    label="Exchange Rate"
                    propertyName="exchangeRate"
                    disabled
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    value={importBook.linnDuty}
                    fullWidth
                    label="Linn Duty (A00 value GBP)"
                    propertyName="linnDuty"
                    onChange={handleNumberFieldChange}
                    disabled={!canChange}
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    value={importBook.linnVat}
                    fullWidth
                    label="Linn VAT (B00 value GBP)"
                    propertyName="linnVat"
                    onChange={handleNumberFieldChange}
                    disabled={!canChange}
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    value={importBook.totalImportValue}
                    fullWidth
                    label="Total Import Value (GBP)"
                    propertyName="totalImportValue"
                    onChange={handleNumberFieldChange}
                    disabled={!canChange}
                />
            </Grid>

            <InvoiceDetails
                invoiceDetails={importBook.importBookInvoiceDetails}
                canChange={canChange}
                handleFieldChange={handleFieldChange}
            />

            <OrderDetails
                orderDetails={importBook.importBookOrderDetails}
                countries={countries}
                canChange={canChange}
                handleFieldChange={handleFieldChange}
            />
        </Grid>
    );
}

export default MainTab;
