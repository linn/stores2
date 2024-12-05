import React from 'react';
import Typography from '@mui/material/Typography';
import { Link, useNavigate } from 'react-router-dom';
import List from '@mui/material/List';
import { Grid } from '@mui/material';

import ListItem from '@mui/material/ListItem';
import Page from './Page';
import config from '../config';

function TestPage() {
    const navigate = useNavigate();
    return (
        <Page homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h4">Test Page!!</Typography>
                </Grid>
                <Grid item xs={12}>
                    <List>
                        <ListItem component={Link} to="/">
                            <Typography color="primary">Home</Typography>
                        </ListItem>
                        <ListItem
                            onClick={() => {
                                navigate('/template');
                            }}
                        >
                            <Typography color="primary">Navigate Home</Typography>
                        </ListItem>
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default TestPage;
