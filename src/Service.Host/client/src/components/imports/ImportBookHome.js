import React, { useState } from 'react';
import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Grid from '@mui/material/Grid';
import config from '../../config';
import Page from '../Page';
import SearchTab from './SearchTab';
import CreateTab from './CreateTab';
import ExportTab from './ExportTab';

function ImportBookHome() {
    const [tab, setTab] = useState(0);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid size={12}>
                <Box sx={{ width: '100%' }}>
                    <Box sx={{ borderBottom: 0, borderColor: 'divider', marginBottom: '20px' }}>
                        <Tabs
                            value={tab}
                            onChange={(_, newValue) => {
                                setTab(newValue);
                            }}
                            sx={{
                                '& .MuiTabs-flexContainer': {
                                    flexWrap: 'wrap'
                                },
                                '& .MuiTab-root': {
                                    minWidth: 'auto',
                                    flex: '0 0 auto'
                                }
                            }}
                        >
                            <Tab label="Search" />
                            <Tab label="Create" />
                            <Tab label="Export" />
                        </Tabs>
                    </Box>

                    {tab === 0 && <SearchTab />}
                    {tab === 1 && <CreateTab />}
                    {tab === 2 && <ExportTab />}
                </Box>
            </Grid>
        </Page>
    );
}

export default ImportBookHome;
