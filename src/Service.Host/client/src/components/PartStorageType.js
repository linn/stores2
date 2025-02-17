import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import { useParams } from 'react-router';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';
import {
    Loading,
    Search,
    InputField,
    ErrorCard,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useSearch from '../hooks/useSearch';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import useGet from '../hooks/useGet';

function PartStorageType({ creating }) {
    const { partNumber, storageTypeCode } = useParams();

    const { isPartStorageTypesLoading, result: partStorageTypeResult } = useInitialise(
        `${itemTypes.partsStorageTypes.url}/${partNumber}/${storageTypeCode}`
    );

    const {
        send: getNewPartStorageType,
        isNewPartStorageTypeLoading,
        result: newPartStorageTypeGetResult
    } = useGet(itemTypes.stockPools.url);

    const [partStorageType, setPartStorageType] = useState(partStorageTypeResult);

    const [partSearchTerm, setPartSearchTerm] = useState('');
    const [storageTypeSearchTerm, setStorageTypeSearchTerm] = useState('');
    const [snackbarVisible, setSnackbarVisible] = useState();

    const {
        search: searchParts,
        results: partsSearchResults,
        loading: partsSearchLoading,
        clear: clearParts
    } = useSearch(itemTypes.parts.url, 'partNumber', 'partNumber', 'description');

    const {
        search: searchStorageTypes,
        results: storageTypesSearchResults,
        loading: storageTypesSearchLoading,
        clear: clearStorageTypes
    } = useSearch(itemTypes.storageTypes.url, 'storageTypeCode', 'storageTypeCode', 'description');

    const {
        send: updatePartStorageType,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult,
        clearPutResult: clearUpdateResult
    } = usePut(itemTypes.storageTypes.url, true);

    const {
        send: createPartStorageType,
        createStorageTypeLoading,
        errorMessage: createError,
        postResult: createResult,
        clearPostResult: clearCreateResult
    } = usePost(itemTypes.storageTypes.url);

    useEffect(() => {
        if (updateResult || createResult) {
            getNewPartStorageType();
            setSnackbarVisible(true);
            clearCreateResult();
            clearUpdateResult();
        }
    }, [updateResult, createResult, getNewPartStorageType, clearCreateResult, clearUpdateResult]);

    useEffect(() => {
        if (newPartStorageTypeGetResult) {
            setPartStorageType(newPartStorageTypeGetResult);
        } else {
            setPartStorageType(partStorageTypeResult);
        }
    }, [newPartStorageTypeGetResult, partStorageTypeResult]);

    const handlePartSearchResultSelect = selected => {
        setPartStorageType(c => ({ ...c, partNumber: selected.partNumber }));
        setPartSearchTerm(selected.description);
    };

    const handleStorageTypeSearchResultSelect = selected => {
        setPartStorageType(c => ({ ...c, storageTypeCode: selected.storageTypeCode }));
        setStorageTypeSearchTerm(selected.description);
    };

    const handleFieldChange = (propertyName, newValue) => {
        setPartStorageType(c => ({ ...c, [propertyName]: newValue }));
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={4}>
                <Grid size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create a Part Storage Type' : 'Part Storage Type'}
                    </Typography>
                </Grid>

                {(isPartStorageTypesLoading ||
                    updateLoading ||
                    createStorageTypeLoading ||
                    isNewPartStorageTypeLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={4}>
                    <InputField
                        propertyName="partNumber"
                        label="Part Number"
                        value={partStorageType}
                        fullWidth
                        onChange={handleFieldChange}
                        disabled
                    />
                </Grid>
                {creating && (
                    <Grid item xs={3}>
                        <Search
                            autoFocus
                            propertyName="part"
                            label="Part"
                            resultsInModal
                            resultLimit={100}
                            value={partSearchTerm}
                            loading={partsSearchLoading}
                            handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                            search={searchParts}
                            searchResults={partsSearchResults}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handlePartSearchResultSelect}
                            clearSearch={clearParts}
                        />
                    </Grid>
                )}

                <Grid item xs={4}>
                    <InputField
                        propertyName="storageTypeCode"
                        label="Storage Type Code"
                        value={partStorageType?.storageTypeCode}
                        fullWidth
                        onChange={handleFieldChange}
                        disabled
                    />
                </Grid>
                {creating && (
                    <Grid item xs={3}>
                        <Search
                            autoFocus
                            propertyName="storageType"
                            label="Storage Type"
                            resultsInModal
                            resultLimit={100}
                            value={storageTypeSearchTerm}
                            loading={storageTypesSearchLoading}
                            handleValueChange={(_, newVal) => setStorageTypeSearchTerm(newVal)}
                            search={searchStorageTypes}
                            searchResults={storageTypesSearchResults}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleStorageTypeSearchResultSelect}
                            clearSearch={clearStorageTypes}
                        />
                    </Grid>
                )}
            </Grid>
            <Grid container spacing={3}>
                <Grid item xs={3}>
                    <InputField
                        propertyName="remarks"
                        label="Remarks"
                        value={partStorageType?.remarks}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="maximum"
                        label="Maximum"
                        value={partStorageType?.maximum}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="incr"
                        label="Incr"
                        value={partStorageType?.incr}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={13}>
                    <InputField
                        propertyName="preference"
                        label="Preference"
                        value={partStorageType?.preference}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button
                        variant="contained"
                        fullWidth
                        disabled={
                            partStorageType === partStorageTypeResult ||
                            !partStorageType?.partNumber ||
                            !partStorageType?.storageTypeCode
                        }
                        onClick={() => {
                            if (creating) {
                                createPartStorageType(null, partStorageType);
                            } else {
                                updatePartStorageType(
                                    `?partNumber=${partNumber}&storageTypeCode=${storageTypeCode}`,
                                    partStorageType
                                );
                            }
                        }}
                    >
                        Create Alloc Code
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() => setSnackbarVisible(false)}
                        message="Save Successful"
                    />
                </Grid>
                {(updateError || createError) && (
                    <Grid item xs={12}>
                        <ErrorCard
                            errorMessage={updateError ? updateError?.details : createError}
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

PartStorageType.propTypes = {
    creating: PropTypes.bool
};

PartStorageType.defaultProps = {
    creating: false
};

export default PartStorageType;
