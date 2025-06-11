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
import useGet from '../hooks/useGet';
import usePost from '../hooks/usePost';
import { lessThan } from '../helpers/numberUtilities';
import Page from './Page';

function Pallets() {
    const [searchPallet, setSearchPallet] = useState('');
    const [creating, setCreating] = useState(false);
    const [snackbarVisible, setSnackbarVisible] = useState(false);

    const { isHistoricalEmployeeLoading, result: historicalEmployeesResult } = useInitialise(
        `${itemTypes.historicEmployees.url}`
    );

    const { isCurrentEmployeeLoading, result: currentEmployeesResult } = useInitialise(
        `${itemTypes.currentEmployees.url}`
    );

    const {
        send: getInitialDepartment,
        isoading: isDepartmentLoading,
        result: departmentGetResult
    } = useGet(itemTypes.departments.url);

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

    const [pendingSelected, setPendingSelected] = useState(null);

    const handlePalletSearchResultSelect = selected => {
        if (selected && selected.palletNumber) {
            getInitialDepartment(selected.auditedByDepartmentCode);
            setPendingSelected(selected);
        }
    };

    useEffect(() => {
        if (pendingSelected && departmentGetResult) {
            const auditedByEmployee =
                historicalEmployeesResult?.items.find(
                    emp => emp.id === pendingSelected?.auditedBy
                ) || {};

            setPallet({
                ...pendingSelected,
                auditedByEmployee: auditedByEmployee.fullName,
                storageLocationDescription: pendingSelected.storageLocation?.description,
                defaultStockPoolDescription: pendingSelected.defaultStockPool?.stockPoolDescription,
                departmentCode: departmentGetResult?.departmentCode,
                auditedByDepartmentDescription: departmentGetResult?.description
            });
            setPendingSelected(null); // This prevents a loop!
        }
    }, [pendingSelected, departmentGetResult, historicalEmployeesResult]);

    const handleDepartmentSearchResultSelect = selected => {
        setPallet({
            ...pallet,
            auditedByDepartmentCode: selected.departmentCode,
            auditedByDepartmentDescription: selected.description
        });
    };
    const handleStockPoolSearchResultSelect = selected => {
        setPallet({
            ...pallet,
            defaultStockPool: selected,
            defaultStockPoolId: selected.stockPoolCode,
            defaultStockPoolDescription: selected.stockPoolDescription
        });
    };

    const handleStorageLocationSearchResultSelect = selected => {
        setPallet({
            ...pallet,
            storageLocation: selected,
            storageLocationId: selected.locationId,
            storageLocationDescription: selected.locationCode
        });
    };

    const handleEmployeeFieldChange = (propertyName, newValue) => {
        const employeeInfo = currentEmployeesResult?.items.find(emp => emp.fullName === newValue);

        setPallet(c => ({
            ...c,
            auditedBy: employeeInfo?.id,
            auditedByEmployee: employeeInfo?.fullName
        }));
    };

    const handleCreatingFieldChange = () => {
        setPallet();
        setCreating(!creating);
    };

    const mappedPalletSearchResults =
        palletSearchResults?.map(r => ({
            ...r,
            name: r.palletNumber ? r.palletNumber.toString() : ''
        })) ?? [];

    const handleFieldChange = (propertyName, newValue) => {
        if (typeof newValue === 'string') {
            setPallet({ ...pallet, [propertyName]: newValue });
        } else if (typeof newValue === 'number' && !lessThan(newValue, 0)) {
            setPallet({ ...pallet, [propertyName]: newValue });
        } else if (newValue instanceof Date || newValue === null) {
            setPallet({ ...pallet, [propertyName]: newValue });
        }
    };

    const auditedByEmployee = historicalEmployeesResult?.items.find(
        emp => emp.id === pallet?.auditedBy
    );

    let employeeNames = currentEmployeesResult?.items.map(emp => emp.fullName);

    if (auditedByEmployee && !employeeNames.includes(auditedByEmployee.fullName)) {
        employeeNames = [...employeeNames, auditedByEmployee.fullName];
    }

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
                isCurrentEmployeeLoading ||
                isDepartmentLoading) && (
                <Grid size={12}>
                    <Loading />
                </Grid>
            )}
            <Grid container spacing={3}>
                <Grid size={10}>
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
                            onChange={handleFieldChange}
                            value={pallet?.palletNumber}
                            fullWidth
                        />
                    )}
                </Grid>

                <Grid size={2}>
                    <Button variant="outlined" fullWidth onClick={handleCreatingFieldChange}>
                        {creating ? `View Pallet` : 'Create Pallet'}
                    </Button>
                </Grid>
            </Grid>

            {(pallet || creating) && (
                <>
                    <Grid size={12}>
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
                                    value={pallet?.storageLocationDescription}
                                    loading={storageLocationSearchLoading}
                                    handleValueChange={(_, newVal) =>
                                        setPallet({ ...pallet, storageLocationDescription: newVal })
                                    }
                                    search={searchStorageLoction}
                                    searchResults={storageLocationSearchResults}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={handleStorageLocationSearchResultSelect}
                                    clearSearch={clearStorageLocation}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid size={12}>
                        <Grid container spacing={2}>
                            <Grid item size={2}>
                                <Dropdown
                                    value={pallet.auditedByEmployee}
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
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={2}>
                                <InputField
                                    propertyName="auditFrequencyWeeks"
                                    label="Audit Frequency Weeks"
                                    type="number"
                                    onChange={handleFieldChange}
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
                                    value={pallet?.auditedByDepartmentDescription}
                                    loading={departmentSearchLoading}
                                    handleValueChange={(_, newVal) =>
                                        setPallet({
                                            ...pallet,
                                            auditedByDepartmentDescription: newVal
                                        })
                                    }
                                    search={searchDepartment}
                                    searchResults={departmentSearchResults}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={handleDepartmentSearchResultSelect}
                                    clearSearch={clearDepartments}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid size={12}>
                        <Grid container spacing={2}>
                            <Grid size={2}>
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
                            <Grid size={2}>
                                <DatePicker
                                    label="Date Invalid"
                                    propertyName="dateInvalid"
                                    value={pallet?.dateInvalid}
                                    onChange={handleFieldChange}
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
                    <Grid size={12}>
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
                                    onChange={handleFieldChange}
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
                                    onChange={handleFieldChange}
                                    type="number"
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid size={12}>
                        <Grid container spacing={2}>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.locationType}
                                    fullWidth
                                    propertyName="locationTypeId"
                                    label="Location Type ID"
                                    allowNoValue
                                    items={['LINN']}
                                    onChange={handleFieldChange}
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
                                    value={pallet?.defaultStockPoolDescription}
                                    loading={stockPoolSearchLoading}
                                    handleValueChange={(_, newVal) =>
                                        setPallet({
                                            ...pallet,
                                            defaultStockPoolDescription: newVal
                                        })
                                    }
                                    search={searchStockPool}
                                    searchResults={stockPoolSearchResults}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={handleStockPoolSearchResultSelect}
                                    clearSearch={clearStockPool}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid size={12}>
                        <Grid container spacing={2}>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.stockType}
                                    fullWidth
                                    propertyName="stockType"
                                    label="Stock Type"
                                    allowNoValue
                                    items={stockTypes}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    value={pallet?.stockState}
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
                                disabled={!pallet?.storageLocation}
                                onClick={() => {
                                    if (creating) {
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
                            <Grid size={12}>
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
