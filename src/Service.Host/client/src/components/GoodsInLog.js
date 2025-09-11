import React, { useState, useMemo } from 'react';
import {
    DatePicker,
    Dropdown,
    InputField,
    Loading,
    Search,
    ReportDataGrids
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import queryString from 'query-string';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import useInitialise from '../hooks/useInitialise';
import Page from './Page';

function GoodsInLog() {
    const [createdBy, setCreatedBy] = useState(null);
    const [articleNumber, setArticleNumber] = useState(null);
    const [quantity, setQuantity] = useState(null);
    const [orderNumber, setOrderNumber] = useState(null);
    const [reqNumber, setReqNumber] = useState(null);
    const [storagePlaceSelect, setStoragePlaceSelect] = useState(null);
    const [storagePlace, setStoragePlace] = useState(null);

    const defaultStartDate = moment().subtract(1, 'days');
    const [fromDate, setFromDate] = useState(defaultStartDate);
    const [toDate, setToDate] = useState(new Date());

    const {
        send: getStoragePlaces,
        storagePlacesLoading,
        result: storagePlacesResult,
        clearData: clearStoragePlaces
    } = useGet(itemTypes.storagePlaces.url);

    const { send: getReport, isLoading, result: reportResult } = useGet(itemTypes.goodsInLog.url);

    const [firstTime, setFirstTime] = useState(true);
    const getQueryString = () => {
        const opt = {};
        if (fromDate) opt.fromDate = fromDate.toISOString();
        if (toDate) opt.toDate = toDate.toISOString();
        if (createdBy) opt.createdBy = createdBy;
        if (articleNumber?.length) opt.articleNumber = articleNumber;
        if (quantity) opt.quantity = quantity;
        if (orderNumber) opt.orderNumber = orderNumber;
        if (reqNumber) opt.reqNumber = reqNumber;
        if (storagePlace?.length) opt.storagePlace = storagePlace;
        return `?${queryString.stringify(opt)}`;
    };

    if (firstTime) {
        getReport(null, getQueryString());
        setFirstTime(false);
    }

    const { result: currentEmployees, isLoading: isCurrentEmployeesLoading } = useInitialise(
        itemTypes.currentEmployees.url
    );

    const handleEmployeeDropDownChange = (propertyName, newValue) => {
        setCreatedBy(newValue);
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
            <Grid container spacing={2}>
                <Grid size={12}>
                    <Typography variant="h4">Goods In Log</Typography>
                </Grid>
                {isLoading ||
                    (isCurrentEmployeesLoading && (
                        <Grid size={12}>
                            <Loading />
                        </Grid>
                    ))}
                <Grid size={3}>
                    <DatePicker
                        label="From Date"
                        value={fromDate}
                        maxDate={toDate}
                        onChange={setFromDate}
                    />
                </Grid>
                <Grid size={3}>
                    <DatePicker
                        label="To Date"
                        value={toDate}
                        minDate={fromDate}
                        onChange={setToDate}
                    />
                </Grid>
                <Grid size={3}>
                    <Dropdown
                        propertyName="createdBy"
                        items={currentEmployees?.items.map(employee => ({
                            id: employee.id,
                            displayText: `${employee?.firstName} ${employee?.lastName}`
                        }))}
                        label="Created By"
                        onChange={handleEmployeeDropDownChange}
                        value={createdBy}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="orderNumber"
                        label="Order Number"
                        type="number"
                        value={orderNumber}
                        onChange={(_, newValue) => setOrderNumber(newValue)}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="reqNumber"
                        label="Req Number"
                        type="number"
                        value={reqNumber}
                        onChange={(_, newValue) => setReqNumber(newValue)}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        propertyName="quantity"
                        label="Quantity"
                        type="number"
                        value={quantity}
                        onChange={(_, newValue) => setQuantity(newValue)}
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
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
                <Grid size={4}>
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
                    <Grid size={12}>
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
