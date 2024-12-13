import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import { Loading, utilities } from '@linn-it/linn-form-components-library';
import ListItem from '@mui/material/ListItem';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useInitialise from '../hooks/useInitialise';

function Carriers() {
    const { isLoading, result } = useInitialise(itemTypes.carriers.url);
    return (
        <Page homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Carriers</Typography>
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
                                    {c.code} - {c.name}
                                </Typography>
                            </ListItem>
                        ))}
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default Carriers;
