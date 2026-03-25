import React, { useEffect, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import {
    ErrorCard,
    Loading,
    SaveBackCancelButtons,
    utilities
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useInitialise from '../../hooks/useInitialise';
import useGet from '../../hooks/useGet';
import usePost from '../../hooks/usePost';
import Page from '../Page';
import MainTab from './MainTab';
import HistoryTab from './HistoryTab';

function ImportBookUtility({ creating }) {
    const [tab, setTab] = useState(0);
    const [hasFetched, setHasFetched] = useState(false);
    const { id } = useParams();
    const [searchParams] = useSearchParams();

    const { result: countries, loading: countriesLoading } = useInitialise(itemTypes.countries.url);
    const { result: currencies, loading: currenciesLoading } = useInitialise(
        itemTypes.currencies.url
    );
    const { result: cpcNumbers, loading: cpcNumbersLoading } = useInitialise(
        itemTypes.cpcNumbers.url
    );

    const {
        send: getImportBook,
        isLoading,
        result: importBookGetResult
    } = useGet(itemTypes.importBooks.url, true);

    const {
        send: createImportBook,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.importBooks.url, true, true);

    const {
        send: updateImportBook,
        isLoading: updateLoading,
        errorMessage: updateError,
        postResult: updateResult,
        clearPostResult: clearUpdateResult
    } = usePost(itemTypes.importBooks.url, true, false);

    const auth = useAuth();
    const token = auth.user?.access_token;

    if (!creating && !hasFetched && token) {
        setHasFetched(true);
        getImportBook(id);
    }

    if (creating && !hasFetched && token) {
        setHasFetched(true);
        const queryString = searchParams.toString();
        const path = queryString ? `initialise?${queryString}` : 'initialise';
        getImportBook(path);
    }

    const navigate = useNavigate();

    const [importBook, setImportBook] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (importBookGetResult && !importBook) {
        setImportBook(importBookGetResult);
    }

    useEffect(() => {
        if (updateResult) {
            setImportBook(updateResult);
            clearUpdateResult();
        }
    }, [updateResult, clearUpdateResult]);

    const canChange = () => {
        if (importBookGetResult) {
            if (creating && utilities.getHref(importBookGetResult, 'create')) {
                return true;
            } else if (!creating && utilities.getHref(importBookGetResult, 'update')) {
                return true;
            }
        }

        return false;
    };

    const validImportBook = () =>
        importBook && importBook.carrierId && importBook.supplierId && importBook.currency;

    const handleFieldChange = (propertyName, newValue) => {
        setImportBook(current => ({ ...current, [propertyName]: newValue }));
        setChangesMade(true);
    };

    const handleNumberFieldChange = (propertyName, newValue) => {
        if (!newValue || newValue.toString().trim() === '') {
            handleFieldChange(propertyName, null);
        } else {
            // Remove any non-numeric characters, keeping only digits and decimal point
            const cleanedValue = newValue.toString().replace(/[^0-9.]/g, '');

            // Count decimal points - should be 0 or 1
            const decimalCount = (cleanedValue.match(/\./g) || []).length;

            if (cleanedValue === '' || decimalCount > 1) {
                handleFieldChange(propertyName, null);
            } else {
                // Handle cases like just "." or empty after cleaning
                if (cleanedValue === '.') {
                    handleFieldChange(propertyName, null);
                } else {
                    // Store the string value to preserve decimal point during input
                    // This allows typing "2." and continuing with "2.5" without losing the decimal
                    handleFieldChange(propertyName, cleanedValue);
                }
            }
        }
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {createError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={createError} />
                    </Grid>
                )}
                {updateError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={updateError} />
                    </Grid>
                )}

                {isLoading ||
                createLoading ||
                updateLoading ||
                countriesLoading ||
                cpcNumbersLoading ||
                currenciesLoading ? (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                ) : (
                    importBook && (
                        <>
                            <Box sx={{ width: '100%' }}>
                                <Box
                                    sx={{
                                        borderBottom: 0,
                                        borderColor: 'divider',
                                        marginBottom: '20px'
                                    }}
                                >
                                    <Tabs
                                        value={tab}
                                        onChange={(_, newValue) => {
                                            setTab(newValue);
                                        }}
                                        sx={{
                                            '& .MuiTabs-flexContainer': {
                                                flexWrap: 'wrap'
                                            },
                                            '& .MuiTab-root': {
                                                minWidth: 'auto',
                                                flex: '0 0 auto'
                                            }
                                        }}
                                    >
                                        <Tab
                                            label={
                                                creating ? 'New Import' : `Import ${importBook.id}`
                                            }
                                        />
                                        <Tab label="History" />
                                    </Tabs>
                                </Box>

                                {tab === 0 && (
                                    <MainTab
                                        importBook={importBook}
                                        countries={countries}
                                        currencies={currencies}
                                        cpcNumbers={cpcNumbers}
                                        canChange={canChange()}
                                        handleFieldChange={handleFieldChange}
                                        handleNumberFieldChange={handleNumberFieldChange}
                                    />
                                )}
                                {tab === 1 && <HistoryTab importBook={importBook} />}
                            </Box>

                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/import-books')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createImportBook(null, importBook);
                                        } else {
                                            updateImportBook(id, importBook);
                                        }
                                    }}
                                    saveDisabled={!changesMade || !validImportBook()}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        setImportBook(importBookGetResult);
                                    }}
                                />
                            </Grid>
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}
ImportBookUtility.propTypes = { creating: PropTypes.bool };
ImportBookUtility.defaultProps = { creating: false };

export default ImportBookUtility;
