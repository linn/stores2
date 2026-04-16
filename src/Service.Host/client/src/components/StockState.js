import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    Dropdown,
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';
import Page from './Page';

function StockState({ creating }) {
    const [hasFetched, setHasFetched] = useState(false);
    const { state } = useParams();
    const {
        send: getStockState,
        isLoading,
        result: stockStateGetResult
    } = useGet(itemTypes.stockStates.url);

    const {
        send: updateStockState,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.stockStates.url, true);

    const {
        send: createStockState,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.stockStates.url, true, true);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getStockState(state);
    }

    const navigate = useNavigate();

    const [formValues, setFormValues] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (stockStateGetResult && !formValues) setFormValues(stockStateGetResult);
    if (creating && !formValues) setFormValues({ qcRequired: 'N' });

    const handleFieldChange = (propertyName, newValue) => {
        const value = propertyName === 'state' ? newValue?.toUpperCase() : newValue;
        setFormValues(current => ({ ...current, [propertyName]: value }));
        setChangesMade(true);
    };

    useEffect(() => {
        if (updateResult) setFormValues(updateResult);
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
                {isLoading || updateLoading || createLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    formValues && (
                        <>
                            <Grid size={4}>
                                <InputField
                                    disabled={!creating}
                                    value={formValues.state}
                                    fullWidth
                                    label="State"
                                    propertyName="state"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={8} />
                            <Grid size={6}>
                                <InputField
                                    value={formValues.description}
                                    fullWidth
                                    label="Description"
                                    propertyName="description"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={6} />
                            <Grid size={4}>
                                <Dropdown
                                    value={formValues.qcRequired}
                                    fullWidth
                                    label="QC Required"
                                    propertyName="qcRequired"
                                    allowNoValue={false}
                                    items={[
                                        { id: 'Y', displayText: 'Yes' },
                                        { id: 'N', displayText: 'No' }
                                    ]}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={8} />
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/stock/states')}
                                    saveClick={() => {
                                        setChangesMade(false);
                                        if (creating) createStockState(null, formValues);
                                        else updateStockState(state, formValues);
                                    }}
                                    saveDisabled={!changesMade}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        if (creating) setFormValues({ qcRequired: 'N' });
                                        else setFormValues(stockStateGetResult);
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

StockState.propTypes = { creating: PropTypes.bool };
StockState.defaultProps = { creating: false };

export default StockState;
