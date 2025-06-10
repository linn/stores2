import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import {
    Loading,
    Search,
    InputField,
    Dropdown,
    DatePicker,
    SnackbarMessage,
    ErrorCard
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useSearch from '../hooks/useSearch';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import { lessThan } from '../helpers/numberUtilities';
import Page from './Page';

function Pallets() {
    const [searchPallet, setSearchPallet] = useState('');
    const [employee, setEmployee] = useState('');
    const [locationType, setLocationType] = useState();
    const [department, setDepartment] = useState('');
    const [stockPool, setStockPool] = useState();
    const [creating, setCreating] = useState(false);
    const [storageLocation, setStorageLocation] = useState();
    const [snackbarVisible, setSnackbarVisible] = useState(false);

    const { isHistoricalEmployeeLoading, result: historicalEmployeesResult } = useInitialise(
        `${itemTypes.historicEmployees.url}`
    );

    const { isCurrentEmployeeLoading, result: currentEmployeesResult } = useInitialise(
        `${itemTypes.currentEmployees.url}`
    );

    const [pallet, setPallet] = useState('');

    const stockTypes = [
        { value: 'A', label: 'Any Stock' },
        { value: 'R', label: 'Raw Materials' },
        { value: 'F', label: 'Finished Goods' }
    ];

    const stockStates = [
        { value: 'A', label: 'Any State' },
        { value: 'I', label: 'Inspected State' },
        { value: 'Q', label: 'QC/ Failed State' }
    ];

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

    const {
        search: searchStorageLoction,
        results: storageLocationSearchResults,
        loading: storageLocationSearchLoading,
        clear: clearStorageLocation
    } = useSearch(itemTypes.storageLocations.url, 'locationCode', 'locationCode', 'description');

    const {
        send: updatePallet,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult,
        clearPutResult: clearUpdateResult
    } = usePut(itemTypes.pallets.url, true);

    const {
        send: createPallet,
        isLoading: createPalletLoading,
        errorMessage: createError,
        postResult: createResult,
        clearPostResult: clearCreateResult
    } = usePost(itemTypes.pallets.url);

    useEffect(() => {
        if (updateResult) {
            setPallet(updateResult);
            setSnackbarVisible(true);
            clearUpdateResult();
        } else if (createResult) {
            setPallet(createResult);
            setSnackbarVisible(true);
            clearCreateResult();
        }
    }, [updateResult, createResult, clearCreateResult, clearUpdateResult]);

    const handlePalletSearchResultSelect = selected => {
        if (selected && selected.palletNumber) {
            setSearchPallet(selected.palletNumber);
            setPallet(selected);
            setDepartment(selected.auditedByDepartmentName);
            setStockPool(selected.defaultStockPool.description);
            setStorageLocation(selected.storageLocation?.locationCode);
            setLocationType({
                key: selected.locationType,
                description: selected.locationTypeDescription
            });

            const auditedByEmployee =
                historicalEmployeesResult?.items.find(emp => emp.id === selected?.auditedBy) || '';

            setEmployee(auditedByEmployee.fullName);
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

    const handleStorageLocationSearchResultSelect = selected => {
        setStorageLocation(selected.locationCode);
        setPallet(c => ({
            ...c,
            storageLocation: selected,
            storageLocationId: selected.locationId
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

    const handleNumberFieldChange = (propertyName, newValue) => {
        if (!lessThan(newValue, 0)) {
            setPallet(c => ({ ...c, [propertyName]: newValue }));
        }
    };

    const handleLocationTypeFieldChange = (propertyName, newValue) => {
        if (newValue === 'LINN') {
            setLocationType({ key: 'L', description: newValue });
            setPallet(c => ({
                ...c,
                locationTypeId: 'L'
            }));
        } else {
            setLocationType({ key: '', description: '' });
            setPallet(c => ({
                ...c,
                locationTypeId: null
            }));
        }
    };

    const handleEmployeeFieldChange = (propertyName, newValue) => {
        const employeeInfo = currentEmployeesResult?.items.find(emp => emp.fullName === newValue);

        setEmployee(newValue);
        setPallet(c => ({
            ...c,
            auditedBy: employeeInfo?.id
        }));
    };

    const handleCreatingFieldChange = () => {
        setPallet();
        setDepartment();
        setStockPool();
        setLocationType();
        setStorageLocation('');
        setEmployee();
        setCreating(!creating);
    };

    const auditedByEmployee = historicalEmployeesResult?.items.find(
        emp => emp.id === pallet?.auditedBy
    );

    let employeeNames = currentEmployeesResult?.items.map(emp => emp.fullName);

    if (auditedByEmployee && !employeeNames.includes(auditedByEmployee.fullName)) {
        employeeNames = [...employeeNames, auditedByEmployee.fullName];
    }

    const handleDateChange = (propertyName, momentObj) => {
        // If you want to store as ISO string:
        const dateValue = momentObj ? momentObj.toISOString() : null;
        setPallet(c => ({ ...c, [propertyName]: dateValue }));
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid size={10}>
                <Typography variant="h4">
                    {pallet?.palletNumber ? `Pallet ${pallet?.palletNumber}` : 'Pallets'}
                </Typography>
            </Grid>
            {(createPalletLoading ||
                updateLoading ||
                isHistoricalEmployeeLoading ||
                isCurrentEmployeeLoading) && (
                <Grid size={12}>
                    <Loading />
                </Grid>
            )}
            <Grid container spacing={70}>
                <Grid item xs={10}>
                    {!creating ? (
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
                    ) : (
                        <InputField
                            propertyName="palletNumber"
                            label="Pallet Number"
                            type="number"
                            onChange={handleNumberFieldChange}
                            value={pallet?.palletNumber}
                            fullWidth
                        />
                    )}
                </Grid>

                <Grid item xs={2}>
                    <Button variant="outlined" fullWidth onClick={handleCreatingFieldChange}>
                        {creating ? `View Pallet` : 'Create Pallet'}
                    </Button>
                </Grid>
            </Grid>

            {(pallet || creating) && (
                <>
                    <Grid item xs={12}>
                        <Grid container spacing={2}>
                            <Grid item size={3}>
                                <InputField
                                    propertyName="description"
                                    label="Description"
                                    value={pallet?.description}
                                    onChange={handleFieldChange}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item size={3}>
                                <InputField
                                    propertyName="storageLocationId"
                                    label="Storage Location ID"
                                    value={pallet?.storageLocationId}
                                    fullWidth
                                    disabled
                                />
                            </Grid>
                            <Grid item size={3}>
                                <Search
                                    autoFocus
                                    propertyName="storageLocationName"
                                    label="Storage Location Name"
                                    resultsInModal
                                    resultLimit={100}
                                    value={storageLocation}
                                    loading={storageLocationSearchLoading}
                                    handleValueChange={(_, newVal) => setStorageLocation(newVal)}
                                    search={searchStorageLoction}
                                    searchResults={storageLocationSearchResults}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={handleStorageLocationSearchResultSelect}
                                    clearSearch={clearStorageLocation}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container spacing={2}>
                            <Grid item size={2}>
                                <Dropdown
                                    value={employee}
                                    fullWidth
                                    propertyName="lastAuditedBy"
                                    label="Last Audited By"
                                    allowNoValue
                                    items={employeeNames}
                                    onChange={handleEmployeeFieldChange}
                                />
                            </Grid>

                            <Grid item size={2}>
                                <DatePicker
                                    label="Date Last Audited"
                                    propertyName="dateLastAudited"
                                    value={pallet?.dateLastAudited}
                                    onChange={date => handleDateChange('dateLastAudited', date)}
                                />
                            </Grid>
                            <Grid item size={2}>
                                <InputField
                                    propertyName="auditFrequencyWeeks"
                                    label="Audit Frequency Weeks"
                                    type="number"
                                    onChange={handleNumberFieldChange}
                                    value={pallet?.auditFrequencyWeeks}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item size={2}>
                                <InputField
                                    propertyName="auditedByDepartmentCode"
                                    label="Audited By Department Code"
                                    value={pallet?.auditedByDepartmentCode}
                                    disabled
                                    fullWidth
                                />
                            </Grid>
                            <Grid item size={4}>
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
                            <Grid item xs={2}>
                                <Dropdown
                                    value={pallet?.accessible}
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
                                    onChange={date => handleDateChange('dateInvalid', date)}
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.cage}
                                    fullWidth
                                    label="Cage"
                                    propertyName="cage"
                                    allowNoValue
                                    items={['Y', 'N']}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container spacing={2}>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.storesKittable}
                                    fullWidth
                                    label="Stores Kittable"
                                    propertyName="storesKittable"
                                    allowNoValue
                                    items={['Y', 'N']}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={4}>
                                <InputField
                                    propertyName="storesKittingPriority"
                                    label="Stores Kitting Priority"
                                    value={pallet?.storesKittingPriority}
                                    fullWidth
                                    onChange={handleNumberFieldChange}
                                    type="number"
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.salesKittable}
                                    fullWidth
                                    label="Sales Kittable"
                                    propertyName="salesKittable"
                                    allowNoValue
                                    items={['Y', 'N']}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={4}>
                                <InputField
                                    propertyName="salesKittingPriority"
                                    label="Sales Kitting Priority"
                                    value={pallet?.salesKittingPriority}
                                    fullWidth
                                    onChange={handleNumberFieldChange}
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
                            <Grid item size={4}>
                                <InputField
                                    propertyName="defaultStockPoolId"
                                    label="Default Stock Pool"
                                    disabled
                                    value={pallet?.defaultStockPoolId}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item size={6}>
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
                                    value={
                                        stockTypes.find(t => t.value === pallet?.stockType)
                                            ?.label || ''
                                    }
                                    fullWidth
                                    propertyName="stockType"
                                    label="Stock Type"
                                    allowNoValue
                                    items={stockTypes.map(t => t.label)}
                                    onChange={(_, newLabel) => {
                                        const selected = stockTypes.find(t => t.label === newLabel);
                                        setPallet(c => ({
                                            ...c,
                                            stockType: selected ? selected.value : null
                                        }));
                                    }}
                                />
                            </Grid>
                            <Grid item xs={2}>
                                <Dropdown
                                    value={
                                        stockStates.find(s => s.value === pallet?.stockState)
                                            ?.label || ''
                                    }
                                    fullWidth
                                    propertyName="stockState"
                                    label="Stock State"
                                    allowNoValue
                                    items={stockStates.map(s => s.label)}
                                    onChange={(_, newLabel) => {
                                        const selected = stockStates.find(
                                            s => s.label === newLabel
                                        );
                                        setPallet(c => ({
                                            ...c,
                                            stockState: selected ? selected.value : null
                                        }));
                                    }}
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.mixStates}
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
                    <Grid>
                        <Grid size={12}>
                            <Button
                                variant="contained"
                                fullWidth
                                disabled={
                                    pallet === palletSearchResults || !pallet?.storageLocation
                                }
                                onClick={() => {
                                    if (creating) {
                                        console.log(pallet);
                                        createPallet(null, pallet);
                                    } else {
                                        updatePallet(pallet?.palletNumber, pallet);
                                    }
                                }}
                            >
                                {creating ? 'Create ' : 'Save'}
                            </Button>
                        </Grid>
                        <Grid size={12}>
                            <SnackbarMessage
                                visible={snackbarVisible}
                                onClose={() => setSnackbarVisible(false)}
                                message="Save Successful"
                            />
                        </Grid>
                        {(updateError || createError) && (
                            <Grid item xs={12}>
                                <ErrorCard errorMessage={updateError ? updateError : createError} />
                            </Grid>
                        )}
                    </Grid>
                </>
            )}
        </Page>
    );
}

export default Pallets;
