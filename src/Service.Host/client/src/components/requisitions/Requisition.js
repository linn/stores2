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
import MovesTab from './MovesTab';

function Requisition() {
    const { reqNumber } = useParams();
    const { isLoading, result } = useInitialise(itemTypes.requisitions.url, reqNumber);
    const {
        send: cancelReq,
        isLoading: cancelLoading,
        errorMessage: cancelError
    } = usePost(`${itemTypes.requisitions.url}/cancel`, true);
    const [tab, setTab] = useState(0);
    const [selectedLine, setSelectedLine] = useState();

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
                {!isLoading && !cancelLoading && result && (
                    <>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.reqNumber}
                                type="number"
                                onChange={() => {}}
                                label="Req Number"
                                propertyName="reqNumber"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={result.dateBooked}
                                onChange={() => {}}
                                label="Date Booked"
                                propertyName="dateBooked"
                            />
                        </Grid>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.bookedByName}
                                onChange={() => {}}
                                label="Booked By"
                                propertyName="bookedByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={result.reversed}
                                onChange={() => {}}
                                label="Reversed"
                                propertyName="reversed"
                            />
                        </Grid>
                        <Grid size={4} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.functionCode}
                                onChange={() => {}}
                                label="Function Code"
                                propertyName="functionCode"
                            />
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={result.functionCodeDescription}
                                onChange={() => {}}
                                label="Function Code Description"
                                propertyName="functionCodeDescription"
                            />
                        </Grid>
                        <Grid size={6} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.createdByName}
                                onChange={() => {}}
                                label="Created By"
                                propertyName="createdByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={result.dateCreated}
                                onChange={() => {}}
                                label="Date Created"
                                propertyName="dateCreated"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={result.cancelled}
                                onChange={() => {}}
                                label="Cancelled"
                                propertyName="cancelled"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Button
                                disabled={result.cancelled === 'Y'}
                                variant="contained"
                                sx={{ marginTop: '30px' }}
                                onClick={() => setCancelDialogVisible(true)}
                                color="secondary"
                            >
                                Cancel Req
                            </Button>
                        </Grid>
                        <Grid size={4} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.department?.departmentCode}
                                onChange={() => {}}
                                label="Dept"
                                propertyName="departmentCode"
                            />
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={result.department?.description}
                                onChange={() => {}}
                                label="Desc"
                                propertyName="departmentDescription"
                            />
                        </Grid>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.nominal?.nominalCode}
                                onChange={() => {}}
                                label="Nominal"
                                propertyName="nominalCode"
                            />
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={result.nominal?.description}
                                onChange={() => {}}
                                label="Desc"
                                propertyName="nominalDescription"
                            />
                        </Grid>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={result.authorisedByName}
                                onChange={() => {}}
                                label="Auth By"
                                propertyName="authorisedByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={result.dateAuthorised}
                                onChange={() => {}}
                                label="Date Authd"
                                propertyName="dateAuthorised"
                            />
                        </Grid>
                        <Grid size={8} />
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                allowNoValue
                                value={result.manualPick}
                                onChange={() => {}}
                                label="Manual Pick"
                                propertyName="manualPick"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['F', 'O']}
                                allowNoValue
                                value={result.reqType}
                                onChange={() => {}}
                                label="Req Type"
                                propertyName="reqType"
                            />
                        </Grid>
                        <Grid size={8} />
                        <Grid size={6}>
                            <InputField
                                fullWidth
                                value={result.comments}
                                onChange={() => {}}
                                label="Comments"
                                propertyName="comments"
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                fullWidth
                                value={result.reference}
                                onChange={() => {}}
                                label="Reference"
                                propertyName="reference"
                            />
                        </Grid>
                        <Grid size={12}>
                            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                <Tabs value={tab} onChange={handleChange}>
                                    <Tab label="Lines" />
                                    <Tab label="Moves" disabled={!selectedLine} />
                                    <Tab label="Transactions" disabled />
                                </Tabs>
                            </Box>
                        </Grid>
                        <Grid size={12}>
                            {tab === 0 && (
                                <LinesTab
                                    lines={result.lines}
                                    selected={selectedLine}
                                    setSelected={setSelectedLine}
                                />
                            )}
                            {tab === 1 && (
                                <MovesTab
                                    moves={
                                        result.lines?.find(x => x.lineNumber === selectedLine)
                                            ?.moves
                                    }
                                />
                            )}
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default Requisition;
