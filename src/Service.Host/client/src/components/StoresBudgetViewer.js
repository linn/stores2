import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { Loading, InputField } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid2';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import StoresBudget from './StoresBudget';

function StoresBudgetViewer() {
    const [hasFetched, setHasFetched] = useState(false);
    const [selectedId, setSelectedId] = useState(null);

    const { id } = useParams();
    const {
        send: getStoresBudget,
        isLoading,
        result: storesBudgetGetResult
    } = useGet(itemTypes.storesBudget.url);

    if (id && !hasFetched) {
        setHasFetched(true);
        getStoresBudget(id);
    }

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={3}>
                    <InputField
                        value={selectedId}
                        fullWidth
                        disabled={isLoading}
                        label="Enter Budget Id"
                        propertyName="selectedId"
                        onChange={(_, value) => setSelectedId(value)}
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        color="secondary"
                        variant="outlined"
                        style={{ marginTop: '28px' }}
                        onClick={() => {
                            getStoresBudget(selectedId);
                        }}
                    >
                        Go
                    </Button>
                </Grid>
                <Grid item size={8} />
                {isLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {storesBudgetGetResult && (
                    <Grid size={12}>
                        <StoresBudget storesBudget={storesBudgetGetResult} />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default StoresBudgetViewer;
