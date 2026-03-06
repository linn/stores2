import React, { useState } from 'react';
import { useAuth } from 'react-oidc-context';
import { useNavigate, useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    DatePicker,
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons,
    utilities
} from '@linn-it/linn-form-components-library';
import moment from 'moment';
import PropTypes from 'prop-types';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import usePost from '../../hooks/usePost';
import Page from '../Page';

function ImportBookUtility({ creating }) {
    const [hasFetched, setHasFetched] = useState(false);
    const { id } = useParams();
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

    const auth = useAuth();
    const token = auth.user?.access_token;

    if (!creating && !hasFetched && token) {
        setHasFetched(true);
        getImportBook(id);
    }

    const navigate = useNavigate();

    const [importBook, setImportBook] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (importBookGetResult && !importBook) {
        setImportBook(importBookGetResult);
    }

    if (creating && !importBook) {
        setImportBook({});
    }

    const canChange = () => {
        if (creating) {
            return utilities.getHref(importBookGetResult, 'create');
        }
        return importBookGetResult ? utilities.getHref(importBookGetResult, 'update') : false;
    };

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
                        <List>
                            <ErrorCard errorMessage={createError} />
                        </List>
                    </Grid>
                )}

                {isLoading || createLoading ? (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                ) : (
                    importBook && (
                        <>
                            {!creating && (
                                <>
                                    <Grid size={6}>
                                        <InputField
                                            disabled
                                            value={importBook.id}
                                            fullWidth
                                            label="Import Book Id"
                                            propertyName="id"
                                        />
                                    </Grid>
                                    <Grid size={6} />
                                    <Grid size={3}>
                                        <InputField
                                            disabled
                                            value={
                                                importBook.dateCreated
                                                    ? moment(importBook.dateCreated).format(
                                                          'DD-MMM-YY HH:mm'
                                                      )
                                                    : ''
                                            }
                                            fullWidth
                                            label="Date Created"
                                            propertyName="dateCreated"
                                            onChange={handleFieldChange}
                                        />
                                    </Grid>
                                    <Grid size={6}>
                                        <InputField
                                            disabled
                                            value={importBook.createdByName}
                                            fullWidth
                                            label="Created By"
                                            propertyName="createdByName"
                                        />
                                    </Grid>
                                    <Grid size={3} />
                                </>
                            )}
                            <Grid size={3}>
                                <InputField
                                    disabled
                                    value={importBook.supplierId}
                                    fullWidth
                                    label="Supplier"
                                    propertyName="supplierId"
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    disabled
                                    value={importBook.supplierName}
                                    fullWidth
                                    label="Supplier Name"
                                    propertyName="supplierName"
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    disabled
                                    value={importBook.supplierCountry?.name}
                                    fullWidth
                                    label="Country"
                                    propertyName="supplierCountry"
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={importBook.customsEntryCodePrefix}
                                    fullWidth
                                    label="Prefix"
                                    propertyName="customsEntryCodePrefix"
                                    onChange={handleFieldChange}
                                    disabled={!canChange()}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={importBook.customsEntryCode}
                                    fullWidth
                                    label="Customs Entry Code"
                                    propertyName="customsEntryCode"
                                    onChange={handleFieldChange}
                                    disabled={!canChange()}
                                />
                            </Grid>
                            <Grid size={3}>
                                <DatePicker
                                    value={importBook.customsEntryCodeDate}
                                    label="Customs Entry Date"
                                    propertyName="customsEntryCodeDate"
                                    onChange={handleFieldChange}
                                    disabled={!canChange()}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={importBook.currency}
                                    fullWidth
                                    label="Currency"
                                    propertyName="currency"
                                    onChange={handleFieldChange}
                                    disabled={!canChange()}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={importBook.linnDuty}
                                    fullWidth
                                    label="Linn Duty (A00 value GBP)"
                                    propertyName="linnDuty"
                                    onChange={handleNumberFieldChange}
                                    disabled={!canChange()}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={importBook.linnVat}
                                    fullWidth
                                    label="Linn VAT (B00 value GBP)"
                                    propertyName="linnVat"
                                    onChange={handleNumberFieldChange}
                                    disabled={!canChange()}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={importBook.totalDuty}
                                    disabled
                                    label="Total Duty (GBP)"
                                    propertyName="totalDuty"
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    disabled
                                    value={importBook.carrierId}
                                    fullWidth
                                    label="Carrier"
                                    propertyName="carrierId"
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    disabled
                                    value={importBook.carrierName}
                                    fullWidth
                                    label="Carrier Name"
                                    propertyName="carrierName"
                                />
                            </Grid>
                            <Grid size={3} />
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/import-books')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createImportBook(null, importBook);
                                        }
                                    }}
                                    saveDisabled={!changesMade}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        if (creating) {
                                            setImportBook({});
                                        } else {
                                            setImportBook(importBookGetResult);
                                        }
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
