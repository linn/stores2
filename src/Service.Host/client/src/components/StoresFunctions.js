import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { Loading, utilities } from '@linn-it/linn-form-components-library';
import ListItem from '@mui/material/ListItem';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';
import Page from './Page';

function StoresFunction() {
    const { isLoading, result } = useInitialise(itemTypes.functionCodes.url);
    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Stores Functions</Typography>
                </Grid>
                {isLoading && (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                )}
                <Grid size={12}>
                    <List>
                        {result?.map(c => (
                            <ListItem key={c.code} component={Link} to={utilities.getSelfHref(c)}>
                                <Typography color="primary">
                                    {c.code} - {c.description}
                                </Typography>
                            </ListItem>
                        ))}
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default StoresFunction;
