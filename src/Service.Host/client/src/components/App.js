import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';

import ListItem from '@mui/material/ListItem';
import Page from './Page';
import config from '../config';

function App() {
    return (
        <Page homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">stores2</Typography>
                </Grid>
                <Grid size={12}>
                    <List>
                        <ListItem component={Link} to="/stores2/carriers">
                            <Typography color="primary">Carriers</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/reports/storage-place-audit">
                            <Typography color="primary">Storage Place Audit</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/budgets">
                            <Typography color="primary">Stores Budget Viewer</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/storage">
                            <Typography color="primary">Storage Locations</Typography>
                        </ListItem>
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default App;
