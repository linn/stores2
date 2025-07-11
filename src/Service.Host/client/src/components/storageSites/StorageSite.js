import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons,
    useGet,
    usePost,
    usePut
} from '@linn-it/linn-form-components-library';
import itemTypes from '../../itemTypes';
import config from '../../config';
import Page from '../Page';

function StorageSite({ creating }) {
    const [hasFetched, setHasFetched] = useState(false);
    const { code } = useParams();
    const {
        send: getStorageSite,
        isLoading,
        result: storageSiteGetResult
    } = useGet(itemTypes.storageSites.url);

    const {
        send: updateStorageSite,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.storageSites.url, true);

    const {
        send: createStorageSite,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.storageSites.url, true, true);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getStorageSite(code);
    }

    const navigate = useNavigate();

    const [formValues, setFormValues] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (storageSiteGetResult && !formValues) {
        setFormValues(storageSiteGetResult);
    }

    if (creating && !formValues) {
        setFormValues({ description: 'DESC' });
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
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Storage Site">
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
                                    value={formValues.siteCode}
                                    fullWidth
                                    label="Code"
                                    propertyName="siteCode"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={5}>
                                <InputField
                                    value={formValues.description}
                                    fullWidth
                                    label="description"
                                    propertyName="description"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={2}>
                                <InputField
                                    value={formValues.sitePrefix}
                                    fullWidth
                                    label="Prefix"
                                    propertyName="sitePrefix"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/service/storage-sites')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createStorageSite(null, formValues);
                                        } else {
                                            updateStorageSite(code, formValues);
                                        }
                                    }}
                                    saveDisabled={!changesMade}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        if (creating) {
                                            setFormValues({ desc: 'DESC' });
                                        } else {
                                            setFormValues(storageSiteGetResult);
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

export default StorageSite;
