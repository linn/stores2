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
    SnackbarMessage,
    useGet
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import config from '../config';
import useSearch from '../hooks/useSearch';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import itemTypes from '../itemTypes';
import useDelete from '../hooks/useDelete';
import Page from './Page';

function PartStorageType({ creating }) {
    const { id } = useParams();
    const navigate = useNavigate();

    const {
        send: sendPartStorageType,
        loading: isPartStorageTypesLoading,
        result: partStorageTypeResult,
        clearData: clearPartStorageType
    } = useGet(itemTypes.partStorageTypes.url);

    const [partStorageType, setPartStorageType] = useState(partStorageTypeResult);

    const [partSearchTerm, setPartSearchTerm] = useState('');
    const [selectedPart, setSelectedPart] = useState('');
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
        send,
        isLoading: ispartStorageTypesLoading,
        result: partStorageTypes
    } = useGet(itemTypes.partStorageTypes.url);

    const {
        send: updatePartStorageType,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult,
        clearPutResult: clearUpdateResult
    } = usePut(itemTypes.partStorageTypes.url, true);

    const {
        send: createPartStorageType,
        createStorageTypeLoading,
        errorMessage: createError,
        postResult: createResult,
        clearPostResult: clearCreateResult
    } = usePost(itemTypes.partStorageTypes.url);

    const {
        send: deletePartStorageType,
        isLoading: isDeleteLoading,
        deleteResult,
        clearData: clearDeletePartStorageType
    } = useDelete(itemTypes.partStorageTypes.url);

    useEffect(() => {
        if (id) {
            clearPartStorageType();
            sendPartStorageType(id);
        }
    }, [id]);

    useEffect(() => {
        if (partStorageTypeResult) {
            setPartStorageType(partStorageTypeResult);
            send(`?part=${partStorageTypeResult?.partNumber}`);
        }
    }, [partStorageTypeResult]);

    useEffect(() => {
        if (updateResult || createResult || deleteResult) {
            setSnackbarVisible(true);
            clearCreateResult();
            clearUpdateResult();
            clearDeletePartStorageType();
        }
    }, [
        updateResult,
        createResult,
        deleteResult,
        clearCreateResult,
        clearUpdateResult,
        clearDeletePartStorageType
    ]);

    const handlePartSearchResultSelect = selected => {
        setPartStorageType(c => ({ ...c, partNumber: selected.partNumber, part: selected }));
        setPartSearchTerm(selected.partNumber);
        send(`?part=${selected.partNumber}`);
        setSelectedPart(selected.partNumber);
    };

    const handleStorageTypeSearchResultSelect = selected => {
        setPartStorageType(c => ({ ...c, storageTypeCode: selected.storageTypeCode }));
        setStorageTypeSearchTerm(selected.storageTypeCode);
    };

    const handleFieldChange = (propertyName, newValue) => {
        setPartStorageType(c => ({ ...c, [propertyName]: newValue }));
    };

    const partStorageTypeColumns = [
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
        { field: 'incr', headerName: 'Incr', width: 100 },
        { field: 'preference', headerName: 'Preference', width: 100 }
    ];

    useEffect(() => {
        const delayedSearch = setTimeout(() => {
            if (partSearchTerm && partSearchTerm.length > 0) {
                send(`?part=${partSearchTerm}`);
                setSelectedPart(partSearchTerm);
            }
        }, 500);

        return () => clearTimeout(delayedSearch);
    }, [partSearchTerm, send]);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={4}>
                <Grid size={5}>
                    <Button
                        variant="outlined"
                        onClick={() => navigate('/stores2/parts-storage-types')}
                    >
                        Back to Part Storage Types
                    </Button>
                </Grid>
                <Grid size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create a Part Storage Type' : 'Edit a Part Storage Type'}
                    </Typography>
                </Grid>

                {(isPartStorageTypesLoading ||
                    isDeleteLoading ||
                    updateLoading ||
                    createStorageTypeLoading) && (
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
                                handleValueChange={(_, newVal) =>
                                    setStorageTypeSearchTerm(newVal.toUpperCase())
                                }
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
                <Grid size={3}>
                    <InputField
                        propertyName="preference"
                        label="Preference"
                        value={partStorageType?.preference}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
            </Grid>
            <Grid container spacing={10}>
                <Grid size={1}>
                    <Button
                        variant="contained"
                        fullWidth
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
                {(updateError || createError) && (
                    <Grid size={12}>
                        <ErrorCard
                            errorMessage={updateError ? updateError?.details : createError}
                        />
                    </Grid>
                )}
                {!creating && (
                    <Grid size={1}>
                        <Button
                            variant="outlined"
                            fullWidth
                            onClick={() => {
                                deletePartStorageType(partStorageType.bridgeId);
                            }}
                        >
                            Delete
                        </Button>
                    </Grid>
                )}
                {partStorageTypes && (
                    <>
                        <Grid size={12}>
                            <Typography variant="h4">
                                {`Part Storage Types for ${creating ? selectedPart : partStorageTypeResult?.partNumber}`}
                            </Typography>
                        </Grid>
                        <Grid size={12}>
                            <DataGrid
                                getRowId={row => row.bridgeId}
                                rows={partStorageTypes}
                                editMode="cell"
                                columns={partStorageTypeColumns}
                                onRowClick={clicked => {
                                    navigate(utilities.getSelfHref(clicked.row));
                                }}
                                rowHeight={34}
                                loading={ispartStorageTypesLoading}
                            />
                        </Grid>
                    </>
                )}
                <Grid size={12}>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() => setSnackbarVisible(false)}
                        message="Save Successful"
                    />
                </Grid>
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
