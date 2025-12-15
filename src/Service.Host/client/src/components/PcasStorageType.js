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

    const {
        search: searchBoard,
        results: boardSearchResults,
        loading: boardSearchLoading,
        clear: clearBoard
    } = useSearch(itemTypes.pcasBoards.url, 'boardCode', 'boardCode', 'description');

    const {
        search: searchStorageTypes,
        results: storageTypesSearchResults,
        loading: storageTypesSearchLoading,
        clear: clearStorageTypes
    } = useSearch(itemTypes.storageTypes.url, 'storageTypeCode', 'storageTypeCode', 'description');

    const {
        send: updatePcasStorageType,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult,
        clearPutResult: clearUpdateResult
    } = usePut(itemTypes.pcasStorageTypes.url, true);

    const {
        send: createPcasStorageType,
        createStorageTypeLoading,
        errorMessage: createError,
        postResult: createResult,
        clearPostResult: clearCreateResult
    } = usePost(itemTypes.pcasStorageTypes.url);

    useEffect(() => {
        if (pcasStorageTypeResult) {
            setPcasStorageType(pcasStorageTypeResult);
        }
    }, [pcasStorageTypeResult]);

    useEffect(() => {
        if (updateResult) {
            setPcasStorageType(updateResult);
            setSnackbarVisible(true);
            clearUpdateResult();
        } else if (createResult) {
            setPcasStorageType(createResult);
            setSnackbarVisible(true);
            clearCreateResult();
        }
    }, [updateResult, createResult, clearCreateResult, clearUpdateResult]);

    const handleBoardTypeSearchResultSelect = selected => {
        setPcasStorageType(c => ({ ...c, boardCode: selected.boardCode }));
        setBoardSearchTerm(selected.boardCode);
    };

    const handleStorageTypeSearchResultSelect = selected => {
        setPcasStorageType(c => ({ ...c, storageTypeCode: selected.storageTypeCode }));
        setStorageTypeSearchTerm(selected.storageTypeCode);
    };

    const handleFieldChange = (propertyName, newValue) => {
        setPcasStorageType(c => ({ ...c, [propertyName]: newValue }));
    };

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
                {creating ? (
                    <Grid item size={4}>
                        <Search
                            autoFocus
                            propertyName="board"
                            label="Board"
                            resultsInModal
                            resultLimit={100}
                            value={boardSearchTerm}
                            loading={boardSearchLoading}
                            handleValueChange={(_, newVal) =>
                                setBoardSearchTerm(newVal.toUpperCase())
                            }
                            search={searchBoard}
                            searchResults={boardSearchResults}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleBoardTypeSearchResultSelect}
                            clearSearch={clearBoard}
                        />
                    </Grid>
                ) : (
                    <Grid item size={4}>
                        <InputField
                            propertyName="boardDescription"
                            label="Pcas Board"
                            value={pcasStorageType?.pcasBoard?.boardCode}
                            fullWidth
                            disabled
                        />
                    </Grid>
                )}
                {creating ? (
                    <Grid item size={4}>
                        <Search
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
                ) : (
                    <Grid item size={4}>
                        <InputField
                            propertyName="storageTypeDescription"
                            label="Storage Type Code"
                            value={pcasStorageType?.storageType?.storageTypeCode}
                            fullWidth
                            disabled
                        />
                    </Grid>
                )}
            </Grid>

            <Grid container spacing={3}>
                <Grid item size={3}>
                    <InputField
                        propertyName="maximum"
                        label="Maximum"
                        type="number"
                        value={pcasStorageType?.maximum}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item size={3}>
                    <InputField
                        propertyName="increment"
                        label="Incr"
                        type="number"
                        value={pcasStorageType?.increment}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item size={3}>
                    <InputField
                        propertyName="preference"
                        label="Preference"
                        value={pcasStorageType?.preference}
                        fullWidth
                        onChange={handleFieldChange}
                    />
                </Grid>
            </Grid>

            <Grid>
                <Grid size={1}>
                    <Button
                        variant="contained"
                        fullWidth
                        disabled={
                            pcasStorageType === pcasStorageTypeResult ||
                            !pcasStorageType?.boardCode ||
                            !pcasStorageType?.storageTypeCode
                        }
                        onClick={() => {
                            if (creating) {
                                createPcasStorageType(null, pcasStorageType);
                            } else {
                                updatePcasStorageType(
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
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={updateError ? updateError : createError} />
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
