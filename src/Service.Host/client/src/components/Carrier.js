import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import {
    Dropdown,
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import useInitialise from '../hooks/useInitialise';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';

function Carrier({ creating }) {
    const { result: countries, loading: countriesLoading } = useInitialise(itemTypes.countries.url);

    const [hasFetched, setHasFetched] = useState(false);
    const { code } = useParams();
    const {
        send: getCarrier,
        isLoading,
        result: carrierGetResult
    } = useGet(itemTypes.carriers.url);

    const {
        send: updateCarrier,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.carriers.url, true);

    const {
        send: createCarrier,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.carriers.url, true);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getCarrier(code);
    }

    const navigate = useNavigate();

    const [formValues, setFormValues] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (carrierGetResult && !formValues) {
        setFormValues(carrierGetResult);
    }

    if (creating && !formValues) {
        setFormValues({ countryCode: 'GB' });
    }

    const handleFieldChange = (propertyName, newValue) => {
        setFormValues(current => ({ ...current, [propertyName]: newValue }));
        setChangesMade(true);
    };

    useEffect(() => {
        if (updateResult) {
            setFormValues(updateResult);
        }
    }, [updateResult]);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {updateError && (
                    <Grid size={12}>
                        <List>
                            <ErrorCard errorMessage={updateError} />
                        </List>
                    </Grid>
                )}
                {createError && (
                    <Grid size={12}>
                        <List>
                            <ErrorCard errorMessage={createError} />
                        </List>
                    </Grid>
                )}

                {isLoading || countriesLoading || updateLoading || createLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    formValues &&
                    countries && (
                        <>
                            <Grid size={6}>
                                <InputField
                                    value={formValues.code}
                                    fullWidth
                                    label="Code"
                                    propertyName="code"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={formValues.name}
                                    fullWidth
                                    label="Name"
                                    propertyName="name"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    value={formValues.addressee}
                                    fullWidth
                                    label="Addressee Name"
                                    propertyName="addressee"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.line1}
                                    fullWidth
                                    label="Address Line 1"
                                    propertyName="line1"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.line2}
                                    fullWidth
                                    label="Address Line 2"
                                    propertyName="line2"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.line3}
                                    fullWidth
                                    label="Address Line 3"
                                    propertyName="line3"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.line4}
                                    fullWidth
                                    label="Address Line 4"
                                    propertyName="line4"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.postCode}
                                    fullWidth
                                    label="Post Code"
                                    propertyName="postCode"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <Dropdown
                                    value={formValues.countryCode}
                                    fullWidth
                                    label="Country"
                                    propertyName="countryCode"
                                    allowNoValue={false}
                                    items={countries.map(c => ({
                                        id: c.countryCode,
                                        displayText: c.name
                                    }))}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.phoneNumber}
                                    fullWidth
                                    label="Phone"
                                    propertyName="phoneNumber"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.vatRegistrationNumber}
                                    fullWidth
                                    label="Vat Reg No"
                                    propertyName="vatRegistrationNumber"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item size={6} />
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/carriers')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createCarrier(null, formValues);
                                        } else {
                                            updateCarrier(code, formValues);
                                        }
                                    }}
                                    saveDisabled={!changesMade}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        if (creating) {
                                            setFormValues({ countryCode: 'GB' });
                                        } else {
                                            setFormValues(carrierGetResult);
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
Carrier.propTypes = { creating: PropTypes.bool };
Carrier.defaultProps = { creating: false };

export default Carrier;
