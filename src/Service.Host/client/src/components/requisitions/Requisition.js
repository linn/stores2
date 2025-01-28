import React, { useEffect, useReducer, useState } from 'react';
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
    ErrorCard,
    Search
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import PropTypes from 'prop-types';
import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import LinesTab from './LinesTab';
import CancelWithReasonDialog from '../CancelWithReasonDialog';
import usePost from '../../hooks/usePost';
import MovesTab from './MovesTab';
import useSearch from '../../hooks/useSearch';
import requisitionReducer from './reducers/requisitonReducer';

function Requisition({ creating }) {
    const { reqNumber } = useParams();
    const { send: fetchReq, isLoading: fetchLoading, result } = useGet(itemTypes.requisitions.url);
    const [hasFetched, setHasFetched] = useState(false);

    const {
        send: fetchFunctionCodes,
        isLoading: codesLoading,
        result: functionCodes
    } = useGet(itemTypes.functionCodes.url);
    if (!hasFetched) {
        if (!creating && reqNumber) {
            fetchReq(reqNumber);
        }
        fetchFunctionCodes();
        setHasFetched(true);
    }

    const {
        send: cancel,
        isLoading: cancelLoading,
        errorMessage: cancelError,
        postResult: cancelResult
    } = usePost(`${itemTypes.requisitions.url}/cancel`, true);

    const [tab, setTab] = useState(0);
    const [selectedLine, setSelectedLine] = useState(1);

    const [cancelDialogVisible, setCancelDialogVisible] = useState(false);

    const handleChange = (_, newValue) => {
        setTab(newValue);
    };

    const [formState, dispatch] = useReducer(requisitionReducer, null);

    useEffect(() => {
        const defaultState = { dateCreated: new Date(), dateAuthorised: null, lines: [] };

        if (cancelResult) {
            dispatch({ type: 'reload', payload: cancelResult });
        } else if (result) {
            dispatch({ type: 'reload', payload: result });
        } else if (creating) {
            dispatch({ type: 'reload', payload: defaultState });
        }
    }, [result, cancelResult, creating]);

    const {
        search: searchDepartments,
        results: departmentsSearchResults,
        loading: departmentsSearchLoading,
        clear: clearDepartmentsSearch
    } = useSearch(itemTypes.departments.url, 'departmentCode', 'departmentCode', 'description');

    const {
        search: searchNominals,
        results: nominalsSearchResults,
        loading: nominalsSearchLoading,
        clear: clearNominalsSearch
    } = useSearch(itemTypes.nominals.url, 'nominalCode', 'nominalCode', 'description');

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {cancelDialogVisible && (
                    <CancelWithReasonDialog
                        visible={cancelDialogVisible}
                        closeDialog={() => setCancelDialogVisible(false)}
                        onConfirm={reason => {
                            cancel(null, { reason, reqNumber });
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
                {(fetchLoading || cancelLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {!fetchLoading && !cancelLoading && formState && (
                    <>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={formState.reqNumber}
                                type="number"
                                onChange={() => {}}
                                label="Req Number"
                                propertyName="reqNumber"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={formState.dateBooked}
                                onChange={() => {}}
                                label="Date Booked"
                                propertyName="dateBooked"
                            />
                        </Grid>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={formState.bookedByName}
                                onChange={() => {}}
                                label="Booked By"
                                propertyName="bookedByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={formState.reversed}
                                onChange={() => {}}
                                label="Reversed"
                                propertyName="reversed"
                            />
                        </Grid>
                        <Grid size={4} />
                        <Grid size={2}>
                            {!codesLoading && functionCodes && (
                                <Dropdown
                                    fullWidth
                                    value={formState.functionCode}
                                    items={functionCodes.map(f => f.id)}
                                    onChange={() => {}}
                                    label="Function Code"
                                    propertyName="functionCode"
                                />
                            )}
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={formState.functionCodeDescription}
                                onChange={() => {}}
                                label="Function Code Description"
                                propertyName="functionCodeDescription"
                            />
                        </Grid>
                        <Grid size={6} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={formState.createdByName}
                                onChange={() => {}}
                                label="Created By"
                                propertyName="createdByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={formState.dateCreated}
                                onChange={() => {}}
                                label="Date Created"
                                propertyName="dateCreated"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={formState.cancelled}
                                onChange={() => {}}
                                label="Cancelled"
                                propertyName="cancelled"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Button
                                disabled={
                                    formState.cancelled === 'Y' || formState.dateBooked || creating
                                }
                                variant="contained"
                                sx={{ marginTop: '30px', backgroundColor: 'error.light' }}
                                onClick={() => setCancelDialogVisible(true)}
                            >
                                Cancel Req
                            </Button>
                        </Grid>
                        <Grid size={4} />
                        <Grid size={2}>
                            <Search
                                propertyName="departmentCode"
                                label="Department"
                                resultsInModal
                                resultLimit={100}
                                helperText="Enter a search term and press enter to look up departments"
                                value={formState.department?.departmentCode}
                                handleValueChange={(_, newVal) => {
                                    dispatch({
                                        type: 'update_header_department',
                                        payload: { departmentCode: newVal }
                                    });
                                }}
                                search={searchDepartments}
                                loading={departmentsSearchLoading}
                                searchResults={departmentsSearchResults}
                                priorityFunction="closestMatchesFirst"
                                onResultSelect={r => {
                                    dispatch({
                                        type: 'update_header_department',
                                        payload: r
                                    });
                                }}
                                clearSearch={clearDepartmentsSearch}
                                autoFocus={false}
                            />
                        </Grid>

                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={formState.department?.description}
                                onChange={() => {}}
                                label="Desc"
                                propertyName="departmentDescription"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Search
                                propertyName="nominalCode"
                                label="Nominal"
                                resultsInModal
                                resultLimit={100}
                                helperText="Enter a search term and press enter to look up nominals"
                                value={formState.nominal?.nominalCode}
                                handleValueChange={(_, newVal) => {
                                    dispatch({
                                        type: 'update_header_nominal',
                                        payload: { nominalCode: newVal }
                                    });
                                }}
                                search={searchNominals}
                                loading={nominalsSearchLoading}
                                searchResults={nominalsSearchResults}
                                priorityFunction="closestMatchesFirst"
                                onResultSelect={r => {
                                    dispatch({
                                        type: 'update_header_nominal',
                                        payload: r
                                    });
                                }}
                                clearSearch={clearNominalsSearch}
                                autoFocus={false}
                            />
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={formState.nominal?.description}
                                onChange={() => {}}
                                label="Desc"
                                propertyName="nominalDescription"
                            />
                        </Grid>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={formState.authorisedByName}
                                onChange={() => {}}
                                label="Auth By"
                                propertyName="authorisedByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={formState.dateAuthorised}
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
                                value={formState.manualPick}
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
                                value={formState.reqType}
                                onChange={() => {}}
                                label="Req Type"
                                propertyName="reqType"
                            />
                        </Grid>
                        <Grid size={8} />
                        <Grid size={6}>
                            <InputField
                                fullWidth
                                value={formState.comments}
                                onChange={() => {}}
                                label="Comments"
                                propertyName="comments"
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                fullWidth
                                value={formState.reference}
                                onChange={() => {}}
                                label="Reference"
                                propertyName="reference"
                            />
                        </Grid>
                        <Grid size={12}>
                            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                <Tabs value={tab} onChange={handleChange}>
                                    <Tab label="Lines" />
                                    <Tab
                                        label="Moves"
                                        disabled={!formState.lines || !selectedLine}
                                    />
                                    <Tab label="Transactions" disabled />
                                </Tabs>
                            </Box>
                        </Grid>
                        <Grid size={12}>
                            {tab === 0 && (
                                <LinesTab
                                    lines={formState.lines}
                                    selected={selectedLine}
                                    setSelected={setSelectedLine}
                                    cancelLine={cancel}
                                />
                            )}
                            {tab === 1 && (
                                <MovesTab
                                    moves={
                                        formState.lines?.find(x => x.lineNumber === selectedLine)
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

Requisition.propTypes = { creating: PropTypes.bool.isRequired };

export default Requisition;
