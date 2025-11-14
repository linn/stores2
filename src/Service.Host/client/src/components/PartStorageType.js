import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import { useParams } from 'react-router';
import { useNavigate } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';
import {
    Loading,
    Search,
    InputField,
    utilities,
    ErrorCard,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import useSearch from '../hooks/useSearch';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import useGet from '../hooks/useGet';
import Page from './Page';

function PartStorageType({ creating }) {
    const { id } = useParams();
    const navigate = useNavigate();

    const { isPartStorageTypesLoading, result: partStorageTypeResult } = useInitialise(
        `${itemTypes.partsStorageTypes.url}/${id}`
    );

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
    } = usePut(itemTypes.partsStorageTypes.url, true);

    const {
        send: createPartStorageType,
        createStorageTypeLoading,
        errorMessage: createError,
        postResult: createResult,
        clearPostResult: clearCreateResult
    } = usePost(itemTypes.partsStorageTypes.url);

    const {
        send,
        isLoading: isExistingPartsStorageTypesLoading,
        result: existingPartsStorageTypes
    } = useGet(itemTypes.partsStorageTypes.url);

    useEffect(() => {
        if (!creating) {
            setPartStorageType(partStorageTypeResult);
        }
    }, [partStorageTypeResult, creating]);

    useEffect(() => {
        if (updateResult || createResult) {
            setSnackbarVisible(true);
            clearCreateResult();
            clearUpdateResult();
        }
    }, [updateResult, createResult, clearCreateResult, clearUpdateResult]);

    const handlePartSearchResultSelect = selected => {
        setPartStorageType(c => ({ ...c, partNumber: selected.partNumber, part: selected }));
        setPartSearchTerm(selected.partNumber);
    };

    const handleStorageTypeSearchResultSelect = selected => {
        setPartStorageType(c => ({ ...c, storageTypeCode: selected.storageTypeCode }));
        setStorageTypeSearchTerm(selected.storageTypeCode);
    };

    const handleFieldChange = (propertyName, newValue) => {
        setPartStorageType(c => ({ ...c, [propertyName]: newValue }));
    };

    const storageTypeColumns = [
        { field: 'partNumber', headerName: 'Part Number', width: 150 },
        {
            field: 'storageTypeCode',
            headerName: 'Storage Type Code',
            width: 150
        },
        {
            field: 'storageType',
            headerName: 'Storage Type Description',
            width: 300,
            valueGetter: value => {
                return value?.description || '';
            }
        },
        { field: 'maximum', headerName: 'Maximum', width: 100 },
        { field: 'bridgeId', headerName: 'Bridge ID', width: 100 },
        { field: 'incr', headerName: 'Incr', width: 100 },
        { field: 'preference', headerName: 'Preference', width: 100 },
        { field: 'remarks', headerName: 'Remarks', width: 100 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={4}>
                <Grid size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create a Part Storage Type' : 'Edit a Part Storage Type'}
                    </Typography>
                </Grid>

                {(isPartStorageTypesLoading || updateLoading || createStorageTypeLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}

                {creating ? (
                    <>
                        <Grid size={6}>
                            <Search
                                autoFocus
                                propertyName="part"
                                label="Part"
                                resultsInModal
                                resultLimit={100}
                                value={partSearchTerm}
                                loading={partsSearchLoading}
                                handleValueChange={(_, newVal) => {
                                    setPartSearchTerm(newVal.toUpperCase());
                                    send(`?part=${partSearchTerm}`);
                                }}
                                search={searchParts}
                                searchResults={partsSearchResults}
                                priorityFunction="closestMatchesFirst"
                                onResultSelect={handlePartSearchResultSelect}
                                clearSearch={clearParts}
                            />
                        </Grid>
                        <Grid size={6}>
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
                    </>
                ) : (
                    <>
                        <Grid size={6}>
                            <InputField
                                propertyName="partNumber"
                                label="Part Number"
                                value={partStorageType?.partNumber}
                                fullWidth
                                disabled
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                propertyName="partDescription"
                                label="Part Description"
                                value={partStorageType?.part?.description}
                                fullWidth
                                disabled
                            />
                        </Grid>

                        <Grid size={6}>
                            <InputField
                                propertyName="storageTypeCode"
                                label="Storage Type Code"
                                value={partStorageType?.storageTypeCode}
                                fullWidth
                                disabled
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                propertyName="storageTypeDescription"
                                label="Storage Type Description"
                                value={partStorageType?.storageType?.description}
                                fullWidth
                                disabled
                            />
                        </Grid>
                    </>
                )}
            </Grid>
            <Grid container spacing={3}>
                <Grid size={3}>
                    <InputField
                        propertyName="remarks"
                        label="Remarks"
                        value={partStorageType?.remarks}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="maximum"
                        label="Maximum"
                        value={partStorageType?.maximum}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="incr"
                        label="Incr"
                        value={partStorageType?.incr}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={13}>
                    <InputField
                        propertyName="preference"
                        label="Preference"
                        value={partStorageType?.preference}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
            </Grid>
            <Grid container spacing={3}>
                <Grid size={1}>
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
                                updatePartStorageType(`${id}`, partStorageType);
                            }
                        }}
                    >
                        {creating ? 'Create ' : 'Save'}
                    </Button>
                </Grid>

                {partStorageType?.part?.partNumber && (
                    <>
                        <Grid size={12}>
                            <Typography variant="h4">
                                {`Existing Parts storage Types for ${partStorageType?.partNumber}`}
                            </Typography>
                        </Grid>
                        <Grid size={12}>
                            <DataGrid
                                getRowId={row => row.bridgeId}
                                rows={existingPartsStorageTypes}
                                editMode="cell"
                                columns={storageTypeColumns}
                                onRowClick={clicked => {
                                    navigate(utilities.getSelfHref(clicked.row));
                                }}
                                rowHeight={34}
                                loading={isExistingPartsStorageTypesLoading}
                            />
                        </Grid>
                        <Grid size={12}>
                            <SnackbarMessage
                                visible={snackbarVisible}
                                onClose={() => setSnackbarVisible(false)}
                                message="Save Successful"
                            />
                        </Grid>
                    </>
                )}
                {(updateError || createError) && (
                    <Grid size={12}>
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
