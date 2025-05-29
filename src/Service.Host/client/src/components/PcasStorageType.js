import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import { useParams } from 'react-router';
import Grid from '@mui/material/Grid';
import PropTypes from 'prop-types';
import {
    Loading,
    Search,
    InputField,
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
import Page from './Page';

function PcasStorageType({ creating }) {
    const { boardCode, storageTypeCode } = useParams();

    const { isPcasStorageTypesLoading, result: pcasStorageTypeResult } = useInitialise(
        `${itemTypes.pcasStorageTypes.url}/${boardCode}/${storageTypeCode}`
    );

    const [pcasStorageType, setPcasStorageType] = useState(pcasStorageTypeResult);

    const [boardSearchTerm, setBoardSearchTerm] = useState('');
    const [storageTypeSearchTerm, setStorageTypeSearchTerm] = useState('');
    const [snackbarVisible, setSnackbarVisible] = useState();

    useEffect(() => {
        setPcasStorageType(pcasStorageTypeResult);
    }, [pcasStorageTypeResult, storageTypesSearchResults]);

    const {
        search: searchBoard,
        results: boardSearchResults,
        loading: boardSearchLoading,
        clear: clearBoard
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

    useEffect(() => {
        if (updateResult || createResult) {
            setSnackbarVisible(true);
            clearCreateResult();
            clearUpdateResult();
        }
    }, [updateResult, createResult, clearCreateResult, clearUpdateResult]);

    const handleBoardTypeSearchResultSelect = selected => {
        setPcasStorageType(c => ({ ...c, boardCode: selected.boardCode }));
        setBoardSearchTerm(selected.description);
    };

    const handleStorageTypeSearchResultSelect = selected => {
        setPcasStorageType(c => ({ ...c, storageTypeCode: selected.storageTypeCode }));
        setStorageTypeSearchTerm(selected.description);
    };

    const handleFieldChange = (propertyName, newValue) => {
        setPcasStorageType(c => ({ ...c, [propertyName]: newValue }));
    };

    console.log(pcasStorageType);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={4}>
                <Grid size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create a PCAS Storage Type' : 'PCAS Storage Type'}
                    </Typography>
                </Grid>

                {(isPcasStorageTypesLoading || updateLoading || createStorageTypeLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {creating && (
                    <Grid size={3}>
                        <Search
                            autoFocus
                            propertyName="board"
                            label="Board"
                            resultsInModal
                            resultLimit={100}
                            value={boardSearchTerm}
                            loading={boardSearchLoading}
                            handleValueChange={(_, newVal) => setBoardSearchTerm(newVal)}
                            search={searchBoard}
                            searchResults={boardSearchResults}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleBoardTypeSearchResultSelect}
                            clearSearch={clearBoard}
                        />
                    </Grid>
                )}
                <Grid size={2}>
                    <InputField
                        propertyName="boardCode"
                        label="Board Code"
                        value={pcasStorageType?.boardCode}
                        fullWidth
                        disabled
                    />
                </Grid>
                <Grid size={5}>
                    <InputField
                        propertyName="boardDescription"
                        label="Board Description"
                        value={pcasStorageType?.pcasBoard?.description}
                        fullWidth
                        disabled
                    />
                </Grid>
                {/* Storage Type Info on a New Line, Full Width */}
                <Grid item xs={12}>
                    <Grid container spacing={2}>
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
                                    handleValueChange={(_, newVal) =>
                                        setStorageTypeSearchTerm(newVal)
                                    }
                                    search={searchStorageTypes}
                                    searchResults={storageTypesSearchResults}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={handleStorageTypeSearchResultSelect}
                                    clearSearch={clearStorageTypes}
                                />
                            </Grid>
                        )}
                        <Grid item xs={3}>
                            <InputField
                                propertyName="storageTypeCode"
                                label="Storage Type Code"
                                value={pcasStorageType?.storageTypeCode}
                                fullWidth
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                propertyName="storageTypeDescription"
                                label="Storage Type Description"
                                value={pcasStorageType?.storageType?.description}
                                fullWidth
                                disabled
                            />
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
            <Grid container spacing={4}>
                <Grid size={3}>
                    <InputField
                        propertyName="maximum"
                        label="Maximum"
                        value={pcasStorageType?.maximum}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="incr"
                        label="Incr"
                        value={pcasStorageType?.increment}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="preference"
                        label="Preference"
                        value={pcasStorageType?.preference}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
            </Grid>
            <Grid size={12}>
                <InputField
                    propertyName="remarks"
                    label="Remarks"
                    value={pcasStorageType?.remarks}
                    fullWidth
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Button
                        variant="contained"
                        fullWidth
                        // disabled={
                        //     partStorageType === partStorageTypeResult ||
                        //     !partStorageType?.partNumber ||
                        //     !partStorageType?.storageTypeCode
                        // }
                        onClick={() => {
                            if (creating) {
                                createPartStorageType(null, pcasStorageType);
                            } else {
                                updatePartStorageType(
                                    `${boardCode}/${storageTypeCode}`,
                                    pcasStorageType
                                );
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
                        <ErrorCard
                            errorMessage={updateError ? updateError?.details : createError}
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

PcasStorageType.propTypes = {
    creating: PropTypes.bool
};

PcasStorageType.defaultProps = {
    creating: false
};

export default PcasStorageType;
