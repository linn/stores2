import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import { useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid2';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import {
    InputField,
    Loading,
    DatePicker,
    Dropdown,
    ErrorCard
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';

import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';

import useInitialise from '../../hooks/useInitialise';
import LinesTab from './LinesTab';
import CancelWithReasonDialog from '../CancelWithReasonDialog';
import usePost from '../../hooks/usePost';

function Requisition() {
    const { reqNumber } = useParams();
    const { isLoading, result } = useInitialise(itemTypes.requisitions.url, reqNumber);
    const {
        send: cancelReq,
        isLoading: cancelLoading,
        errorMessage: cancelError
    } = usePost(`${itemTypes.requisitions.url}/cancel`, true);
    const [tab, setTab] = useState(0);

    const [cancelDialogVisible, setCancelDialogVisible] = useState(false);

    const handleChange = (_, newValue) => {
        setTab(newValue);
    };
    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {cancelDialogVisible && (
                    <CancelWithReasonDialog
                        visible={cancelDialogVisible}
                        closeDialog={() => setCancelDialogVisible(false)}
                        onConfirm={reason => {
                            cancelReq(null, { reason, reqNumber });
                        }}
                    />
                )}
                <Grid size={12}>
                    <Typography variant="h6">Requisition Viewer</Typography>
                </Grid>
                {cancelError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={cancelError} />{' '}
                    </Grid>
                )}
                {(isLoading || cancelLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {(!isLoading || !cancelLoading) && result && (
                    <>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result?.reqNumber}
                                type="number"
                                onChange={() => {}}
                                label="Req Number"
                                propertyName="reqNumber"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={result?.dateBooked}
                                onChange={() => {}}
                                label="Date Booked"
                                propertyName="dateBooked"
                            />
                        </Grid>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result?.bookedByName}
                                onChange={() => {}}
                                label="Booked By"
                                propertyName="bookedByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={result?.reversed}
                                onChange={() => {}}
                                label="Reversed"
                                propertyName="reversed"
                            />
                        </Grid>
                        <Grid size={4} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result?.functionCode}
                                onChange={() => {}}
                                label="Function Code"
                                propertyName="functionCode"
                            />
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={result?.functionCodeDescription}
                                onChange={() => {}}
                                label="Function Code Description"
                                propertyName="functionCodeDescription"
                            />
                        </Grid>
                        <Grid size={6} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result?.createdByName}
                                onChange={() => {}}
                                label="Created By"
                                propertyName="createdByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={result?.dateCreated}
                                onChange={() => {}}
                                label="Date Created"
                                propertyName="dateCreated"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={result?.cancelled}
                                onChange={() => {}}
                                label="Cancelled"
                                propertyName="cancelled"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Button
                                disabled={result?.cancelled === 'Y'}
                                variant="contained"
                                onClick={() => setCancelDialogVisible(true)}
                                color="secondary"
                            >
                                Cancel Req
                            </Button>
                        </Grid>
                        <Grid size={12}>
                            <InputField
                                fullWidth
                                value={result?.comments}
                                onChange={() => {}}
                                label="Comments"
                                propertyName="comments"
                            />
                        </Grid>
                        <Grid size={12}>
                            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                <Tabs value={tab} onChange={handleChange}>
                                    <Tab label="Lines" />
                                    <Tab label="Moves" disabled />
                                    <Tab label="Transactions" disabled />
                                </Tabs>
                            </Box>
                        </Grid>
                        <Grid size={12}>{tab === 0 && <LinesTab lines={result.lines} />}</Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default Requisition;
