import React, { useState, useMemo } from 'react';
import { DatePicker, InputField, Loading, Search } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import ReportDataGrids from './ReportDataGrids';

function GoodsInLog() {
    const [createdBy, setCreatedBy] = useState(null);
    const [articleNumber, setArticleNumber] = useState(null);
    const [quantity, setQuantity] = useState(null);
    const [orderNumber, setOrderNumber] = useState(null);
    const [reqNumber, setReqNumber] = useState(null);
    const [storagePlaceSelect, setStoragePlaceSelect] = useState(null);
    const [storagePlace, setStoragePlace] = useState(null);

    const defaultStartDate = moment().subtract(2, 'weeks');
    const [fromDate, setFromDate] = useState(defaultStartDate);
    const [toDate, setToDate] = useState(new Date());

    const {
        send: getStoragePlaces,
        storagePlacesLoading,
        result: storagePlacesResult,
        clearData: clearStoragePlaces
    } = useGet(itemTypes.storagePlaces.url);

    const { send: getReport, isLoading, result: reportResult } = useGet(itemTypes.goodsInLog.url);

    const getQueryString = () => {
        let queryString = '?';

        if (fromDate) {
            queryString += `fromDate=${fromDate.toISOString()}&`;
        }

        if (toDate) {
            queryString += `toDate=${toDate.toISOString()}&`;
        }

        if (createdBy) {
            queryString += `createdBy=${createdBy}&`;
        }

        if (articleNumber?.length) {
            queryString += `articleNumber=${articleNumber}&`;
        }

        if (quantity) {
            queryString += `quantity=${quantity}&`;
        }

        if (orderNumber) {
            queryString += `orderNumber=${orderNumber}&`;
        }

        if (reqNumber) {
            queryString += `reqNumber=${reqNumber}&`;
        }

        if (storagePlace?.length) {
            queryString += `storagePlace=${storagePlace.toUpperCase()}`;
        }

        console.log(queryString);

        return queryString;
    };

    const reports = useMemo(
        () => (
            <ReportDataGrids
                perReportExport={false}
                reportData={reportResult?.reportResults ?? []}
                repeatHeaders
                fixedRowHeight
                showHeader={false}
                renderZeroes
                showTotals={false}
            />
        ),
        [reportResult]
    );

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Goods In Log</Typography>
                </Grid>
                {isLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={6}>
                    <DatePicker
                        label="From Date"
                        value={fromDate}
                        maxDate={toDate}
                        onChange={setFromDate}
                    />
                </Grid>
                <Grid item xs={6}>
                    <DatePicker
                        label="To Date"
                        value={toDate}
                        minDate={fromDate}
                        onChange={setToDate}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="createdBy"
                        label="Created By"
                        type="number"
                        fullWidth
                        value={createdBy}
                        onChange={(_, newValue) => setCreatedBy(newValue)}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="orderNumber"
                        label="Order Number"
                        type="number"
                        fullWidth
                        value={orderNumber}
                        onChange={(_, newValue) => setOrderNumber(newValue)}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="reqNumber"
                        label="Req Number"
                        type="number"
                        fullWidth
                        value={reqNumber}
                        onChange={(_, newValue) => setReqNumber(newValue)}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="quantity"
                        label="Quantity"
                        type="number"
                        fullWidth
                        value={quantity}
                        onChange={(_, newValue) => setQuantity(newValue)}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        fullWidth
                        value={articleNumber}
                        onChange={(_, newValue) => setArticleNumber(newValue)}
                        label="Article Number"
                        propertyName="articleNumber"
                    />
                </Grid>
                <Grid size={3}>
                    <Search
                        propertyName="storagePlaceSelect"
                        label="Storage Location"
                        resultsInModal
                        autoFocus={false}
                        resultLimit={100}
                        value={storagePlaceSelect}
                        handleValueChange={(_, newVal) => setStoragePlaceSelect(newVal)}
                        helperText="Press ENTER to search or TAB to proceed"
                        search={() => getStoragePlaces(null, `?searchTerm=${storagePlaceSelect}`)}
                        onKeyPressFunctions={[
                            { keyCode: 9, action: () => setStoragePlace(storagePlaceSelect) }
                        ]}
                        searchResults={storagePlacesResult?.map(s => ({
                            ...s,
                            id: s.name
                        }))}
                        loading={storagePlacesLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setStoragePlace(newValue.id);
                        }}
                        clearSearch={clearStoragePlaces}
                    />
                </Grid>
                <Grid size={1} />
                <Grid size={1}>
                    <Button
                        disabled={isLoading}
                        variant="contained"
                        onClick={() => {
                            getReport(null, getQueryString());
                        }}
                    >
                        Run
                    </Button>
                </Grid>
                {isLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    reports
                )}
            </Grid>
        </Page>
    );
}

export default GoodsInLog;
