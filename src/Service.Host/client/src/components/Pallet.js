import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import { useParams } from 'react-router';
import Grid from '@mui/material/Grid';
import PropTypes from 'prop-types';
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

function Pallet({ creating }) {
    const { id } = useParams();
    const [snackbarVisible, setSnackbarVisible] = useState(false);

    const { isHistoricalEmployeeLoading, result: historicalEmployeesResult } = useInitialise(
        `${itemTypes.historicEmployees.url}`
    );

    const { isPalletLoading, result: palletResult } = useInitialise(`${itemTypes.pallets.url}`, id);

    useEffect(() => {
        if (palletResult) {
            setPallet(palletResult);
        } else if (createResult) {
            setPallet(createResult);
        } else if (updateResult) {
            setPallet(updateResult);
        }
    }, [createResult, palletResult, updateResult]);

    const { isCurrentEmployeeLoading, result: currentEmployeesResult } = useInitialise(
        `${itemTypes.currentEmployees.url}`
    );

    const [pallet, setPallet] = useState({});

    const stockTypes = ['Any Stock', 'Raw Materials', 'Finished Goods'];

    const stockStates = ['Any State', 'Inspected State', 'QC/ Failed State'];

    const {
        search: searchDepartments,
        results: departmentsSearchResults,
        loading: departmentsSearchLoading,
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

    const handleDepartmentSearchResultSelect = selected => {
        setPallet(p => ({
            ...p,
            auditedByDepartmentCode: selected.departmentCode,
            auditedByDepartment: selected
        }));
    };
    const handleStockPoolSearchResultSelect = selected => {
        setPallet(p => ({
            ...p,
            defaultStockPool: selected,
            defaultStockPoolId: selected.stockPoolCode
        }));
    };

    const handleStorageLocationSearchResultSelect = selected => {
        setPallet(p => ({
            ...p,
            storageLocation: selected,
            storageLocationId: selected.locationId
        }));
    };

    const handleEmployeeFieldChange = (propertyName, newValue) => {
        const employeeInfo = currentEmployeesResult?.items.find(emp => emp.fullName === newValue);

        setPallet(c => ({
            ...c,
            auditedBy: employeeInfo?.id,
            auditedByEmployee: employeeInfo?.fullName
        }));
    };

    const handleFieldChange = (propertyName, newValue) => {
        if (typeof newValue === 'string') {
            setPallet({ ...pallet, [propertyName]: newValue });
        } else if (typeof newValue === 'number' && !lessThan(newValue, 0)) {
            setPallet({ ...pallet, [propertyName]: newValue });
        }
    };

    const handleDateChange = (propertyName, momentObj) => {
        const dateValue = momentObj ? momentObj.toISOString() : null;
        setPallet(c => ({ ...c, [propertyName]: dateValue }));
    };

    const auditedByEmployee = historicalEmployeesResult?.items.find(
        emp => emp.id === pallet?.auditedBy
    );

    let employeeNames = currentEmployeesResult?.items.map(emp => emp.fullName);

    if (auditedByEmployee && !employeeNames?.includes(auditedByEmployee?.fullName)) {
        employeeNames = [...employeeNames, auditedByEmployee?.fullName];
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
                isPalletLoading) && (
                <Grid size={12}>
                    <Loading />
                </Grid>
            )}
            <Grid container spacing={3}>
                <Grid size={10}>
                    <InputField
                        propertyName="palletNumber"
                        label="Pallet Number"
                        type="number"
                        onChange={handleFieldChange}
                        disabled={!creating}
                        value={pallet?.palletNumber}
                        fullWidth
                    />
                </Grid>
            </Grid>

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
                            value={pallet?.storageLocation?.description}
                            loading={storageLocationSearchLoading}
                            handleValueChange={(_, newVal) =>
                                setPallet(p => ({
                                    ...p,
                                    storageLocation: {
                                        ...p.storageLocation,
                                        description: newVal
                                    }
                                }))
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
                            value={auditedByEmployee?.fullName}
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
                            clearable
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
                            value={pallet?.auditedByDepartment?.description}
                            loading={departmentsSearchLoading}
                            handleValueChange={(_, newVal) =>
                                setPallet(p => ({
                                    ...p,
                                    auditedByDepartmentDescription: newVal
                                }))
                            }
                            search={searchDepartments}
                            searchResults={departmentsSearchResults}
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
                            onChange={date => handleDateChange('dateInvalid', date)}
                            clearable
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
                            propertyName="locationType"
                            label="Location Type"
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
                            value={pallet?.defaultStockPool?.stockPoolDescription}
                            loading={stockPoolSearchLoading}
                            handleValueChange={(_, newVal) =>
                                setPallet(p => ({
                                    ...p,
                                    defaultStockPool: {
                                        ...p.defaultStockPool,
                                        stockPoolDescription: newVal
                                    }
                                }))
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
        </Page>
    );
}

Pallet.propTypes = {
    creating: PropTypes.bool
};

Pallet.defaultProps = {
    creating: false
};

export default Pallet;
