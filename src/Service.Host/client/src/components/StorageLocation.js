import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import Typography from '@mui/material/Typography';
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

function StorageLocation({ creating }) {
    const [hasFetched, setHasFetched] = useState(false);
    const { id } = useParams();
    const {
        send: getLocation,
        isLoading,
        result: locationGetResult
    } = useGet(itemTypes.storageLocations.url);

    const {
        send: updateLocation,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.storageLocations.url, true);

    const {
        send: createLocation,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.storageLocations.url, true, true);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getLocation(id);
    }

    const navigate = useNavigate();

    const [formValues, setFormValues] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (locationGetResult && !formValues) {
        setFormValues(locationGetResult);
    }

    if (creating && !formValues) {
        setFormValues({
            locationCode: null,
            description: null
        });
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
                <Grid size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create Storage Location' : 'Storage Location'}
                    </Typography>
                </Grid>
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

                {isLoading || updateLoading || createLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    formValues && (
                        <>
                            <Grid size={5}>
                                <InputField
                                    disabled={!creating}
                                    value={formValues.locationCode}
                                    fullWidth
                                    label="Code"
                                    propertyName="locationCode"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={7}>
                                <InputField
                                    value={formValues.description}
                                    fullWidth
                                    label="Description"
                                    propertyName="description"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/storage')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createLocation(null, formValues);
                                        } else {
                                            updateLocation(code, formValues);
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
StorageLocation.propTypes = { creating: PropTypes.bool };
StorageLocation.defaultProps = { creating: false };

export default StorageLocation;
