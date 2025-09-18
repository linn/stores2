import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import React from 'react';
import ListItem from '@mui/material/ListItem';
import config from '../config';
import Page from './Page';

function App() {
    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">stores2</Typography>
                </Grid>
                <Grid size={12}>
                    <List>
                        <ListItem component={Link} to="/requisitions">
                            <Typography color="primary">Requisitions</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/requisitions/pending">
                            <Typography color="primary">Pending Requisitions</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/reports/storage-place-audit">
                            <Typography color="primary">Storage Place Audit</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/budgets">
                            <Typography color="primary">Stores Budget Viewer</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/carriers">
                            <Typography color="primary">Carriers</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/requisitions/stores-functions/view">
                            <Typography color="primary">Stores Functions</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/storage">
                            <Typography color="primary">Storage Locations</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/requisitions/reports/requisition-cost">
                            <Typography color="primary">Cost Of Req Report</Typography>
                        </ListItem>
                        <ListItem component={Link} to="/stores2/reports/labour-hours-in-stock">
                            <Typography color="primary">Labour Hours In Stock Report</Typography>
                        </ListItem>
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default App;
