import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { Search, InputField, Dropdown, DatePicker } from '@linn-it/linn-form-components-library';
import Box from '@mui/material/Box';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useSearch from '../hooks/useSearch';
import Page from './Page';

function Pallets() {
    const [searchPallet, setSearchPallet] = useState('');
    const [employee, setEmployee] = useState('');
    const [locationType, setLocationType] = useState();
    const [department, setDepartment] = useState('');
    const [stockPool, setStockPool] = useState();

    const { isEmployeeLoading, result: employeesResult } = useInitialise(
        `${itemTypes.historicEmployees.url}`
    );
    const [pallet, setPallet] = useState('');

    const stockTypes = ['Any Stock', 'Raw Materials', 'Finished Goods'];

    const stockStates = ['Any State', 'Inspected State', 'QC/ Failed State'];

    const {
        search: searchPallets,
        results: palletSearchResults,
        loading: palletSearchLoading,
        clear: clearPallets
    } = useSearch(itemTypes.pallets.url, 'palletNumber', 'palletNumber', 'description');

    const {
        search: searchDepartment,
        results: departmentSearchResults,
        loading: departmentSearchLoading,
        clear: clearDepartments
    } = useSearch(itemTypes.departments.url, 'departmentCode', 'departmentCode', 'description');

    const {
        search: searchStockPool,
        results: stockPoolSearchResults,
        loading: stockPoolSearchLoading,
        clear: clearStockPool
    } = useSearch(
        itemTypes.stockPools.url,
        'stockPoolCode',
        'stockPoolCode',
        'stockPoolDescription'
    );

    const handlePalletSearchResultSelect = selected => {
        if (selected && selected.palletNumber) {
            setSearchPallet(selected.palletNumber);
            setPallet(selected);
            setDepartment(selected.auditedByDepartmentName);
            setStockPool(selected.defaultStockPool.description);
            setLocationType({
                key: selected.locationType,
                description: selected.locationTypeDescription
            });

            const auditedByEmployee =
                employeesResult?.items.find(emp => emp.id === selected?.auditedBy) || '';
            setEmployee(auditedByEmployee);
        }
    };

    const handleDepartmentSearchResultSelect = selected => {
        setDepartment(selected?.description);
        setPallet(c => ({ ...c, auditedByDepartmentCode: selected.departmentCode }));
    };
    const handleStockPoolSearchResultSelect = selected => {
        setStockPool(selected.stockPoolDescription);
        setPallet(c => ({
            ...c,
            defaultStockPool: selected,
            defaultStockPoolId: selected.stockPoolCode
        }));
    };

    const mappedPalletSearchResults =
        palletSearchResults?.map(r => ({
            ...r,
            name: r.palletNumber ? r.palletNumber.toString() : ''
        })) ?? [];

    const handleFieldChange = (propertyName, newValue) => {
        setPallet(c => ({ ...c, [propertyName]: newValue }));
    };

    const handleLocationTypeFieldChange = (propertyName, newValue) => {
        newValue == 'LINN'
            ? setLocationType({ key: 'L', description: newValue })
            : setLocationType({ key: '', description: '' });
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">
                        {pallet ? `Pallet ${pallet.palletNumber}` : 'Pallets'}
                    </Typography>
                </Grid>
                <Grid size={2}>
                    <Search
                        autoFocus
                        propertyName="palletNumber"
                        label="Pallet Number"
                        resultsInModal
                        resultLimit={100}
                        value={searchPallet}
                        loading={palletSearchLoading}
                        handleValueChange={(_, newVal) => setSearchPallet(newVal)}
                        search={searchPallets}
                        searchResults={mappedPalletSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handlePalletSearchResultSelect}
                        clearSearch={clearPallets}
                    />
                </Grid>

                {pallet && (
                    <>
                        <Box mt={3} />
                        <Grid item size={3}>
                            <InputField
                                propertyName="description"
                                label="Description"
                                value={pallet?.description}
                                fullWidth
                            />
                        </Grid>
                        <Grid item size={3}>
                            <InputField
                                propertyName="storageLocationId"
                                label="Storage Location ID"
                                value={pallet.storageLocation?.locationCode}
                                fullWidth
                                disabled
                            />
                        </Grid>
                        <Grid item size={3}>
                            <InputField
                                propertyName="storageLocationName"
                                label="Storage Location Name"
                                value={pallet.storageLocation?.description}
                                fullWidth
                                disabled
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container spacing={2}>
                                <Grid item size={4}>
                                    <InputField
                                        propertyName="employee"
                                        label="Last Audited By"
                                        value={employee?.fullName}
                                        fullWidth
                                    />
                                </Grid>
                                <Grid item size={4}>
                                    <DatePicker
                                        label="Date Last Audited"
                                        propertyName="dateLastAudited"
                                        value={pallet?.dateLastAudited}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid item size={4}>
                                    <InputField
                                        propertyName="auditFrequencyWeeks"
                                        label="Audit Frequency Weeks"
                                        type="number"
                                        onChange={handleFieldChange}
                                        value={pallet.auditFrequencyWeeks}
                                        fullWidth
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container spacing={2}>
                                <Grid item size={6}>
                                    <InputField
                                        propertyName="auditedByDepartmentCode"
                                        label="Audited By Department Code"
                                        value={pallet.auditedByDepartmentCode}
                                        disabled
                                        fullWidth
                                    />
                                </Grid>
                                <Grid item size={6}>
                                    <Search
                                        autoFocus
                                        propertyName="auditedByDepartment"
                                        label="Audited By Department"
                                        resultsInModal
                                        resultLimit={100}
                                        value={department}
                                        loading={departmentSearchLoading}
                                        handleValueChange={(_, newVal) => setDepartment(newVal)}
                                        search={searchDepartment}
                                        searchResults={departmentSearchResults}
                                        priorityFunction="closestMatchesFirst"
                                        onResultSelect={handleDepartmentSearchResultSelect}
                                        clearSearch={clearDepartments}
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container spacing={2}>
                                <Grid size={4}>
                                    <DatePicker
                                        label="Alloc Queue Time"
                                        propertyName="allocQueueTime"
                                        value={pallet?.allocQueueTime}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid size={2}>
                                    <Dropdown
                                        value={pallet.accessible}
                                        fullWidth
                                        label="Acessible"
                                        propertyName="accessible"
                                        allowNoValue
                                        items={['Y', 'N']}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <DatePicker
                                        label="Date Invalid"
                                        propertyName="dateInvalid"
                                        value={pallet?.dateInvalid}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container spacing={2}>
                                <Grid size={2}>
                                    <Dropdown
                                        value={pallet.storesKitting}
                                        fullWidth
                                        label="Stores Kitting"
                                        propertyName="storesKitting"
                                        allowNoValue
                                        items={['Y', 'N']}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid item size={4}>
                                    <InputField
                                        propertyName="storesKittingPriority"
                                        label="Stores Kitting Priority"
                                        value={pallet.storesKittingPriority}
                                        fullWidth
                                        onChange={handleFieldChange}
                                        type="number"
                                    />
                                </Grid>
                                <Grid size={2}>
                                    <Dropdown
                                        value={pallet.salesKitting}
                                        fullWidth
                                        label="Sales Kitting"
                                        propertyName="salesKitting"
                                        allowNoValue
                                        items={['Y', 'N']}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid item size={4}>
                                    <InputField
                                        propertyName="salesKittingPriority"
                                        label="Sales Kitting Priority"
                                        value={pallet.salesKittingPriority}
                                        fullWidth
                                        onChange={handleFieldChange}
                                        type="number"
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container spacing={2}>
                                <Grid item xs={2}>
                                    <Dropdown
                                        value={locationType?.description}
                                        fullWidth
                                        propertyName="locationTypeId"
                                        label="Location Type ID"
                                        allowNoValue
                                        items={['LINN']}
                                        onChange={handleLocationTypeFieldChange}
                                    />
                                </Grid>
                                <Grid item size={3}>
                                    <InputField
                                        propertyName="defaultStockPoolId"
                                        label="Default Stock Pool"
                                        value={pallet.defaultStockPoolId}
                                        fullWidth
                                    />
                                </Grid>
                                <Grid item size={7}>
                                    <Search
                                        autoFocus
                                        propertyName="defaultStockPoolDescription"
                                        label="Default Stock Pool Description"
                                        resultsInModal
                                        resultLimit={100}
                                        value={stockPool}
                                        loading={stockPoolSearchLoading}
                                        handleValueChange={(_, newVal) => setStockPool(newVal)}
                                        search={searchStockPool}
                                        searchResults={stockPoolSearchResults}
                                        priorityFunction="closestMatchesFirst"
                                        onResultSelect={handleStockPoolSearchResultSelect}
                                        clearSearch={clearStockPool}
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container spacing={2}>
                                <Grid item xs={2}>
                                    <Dropdown
                                        value={pallet.stockType}
                                        fullWidth
                                        propertyName="stockType"
                                        label="Stock Type"
                                        allowNoValue
                                        items={stockTypes}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <Dropdown
                                        value={pallet.stockState}
                                        fullWidth
                                        propertyName="stockState"
                                        label="Stock State"
                                        allowNoValue
                                        items={stockStates}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                                <Grid size={2}>
                                    <Dropdown
                                        value={pallet.mixStates}
                                        fullWidth
                                        label="Mix States"
                                        propertyName="mixStates"
                                        allowNoValue
                                        items={['Y', 'N']}
                                        onChange={handleFieldChange}
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default Pallets;
