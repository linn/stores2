import React, { useState } from 'react';
import {
    Dropdown,
    DatePicker,
    utilities,
    InputField,
    Search
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import itemTypes from '../../../itemTypes';
import useSearch from '../../../hooks/useSearch';
import PickStockDialog from '../PickStockDialog';

function StockOptions({
    stockPools = [],
    fromState = null,
    fromStockPool = null,
    batchDate = null,
    fromLocationCode = null,
    fromPalletNumber = null,
    toState = null,
    toStockPool = null,
    toLocationCode = null,
    toPalletNumber = null,
    disabled = false,
    shouldRender = true,
    partNumber = null,
    quantity = null,
    doPickStock = null,
    functionCode = null,
    setItemValue
}) {
    const [pickStockDialogVisible, setPickStockDialogVisible] = useState(false);
    const {
        search: searchLocations,
        results: locationsSearchResults,
        loading: locationsSearchLoading,
        clear: clearLocationsSearch
    } = useSearch(itemTypes.storageLocations.url, 'locationId', 'locationCode', 'description');

    if (!shouldRender || !functionCode) {
        return '';
    }

    const stockPoolItems = stockPools?.length
        ? utilities.sortEntityList(stockPools, 'stockPoolCode')
        : [];

    return (
        <>
            {(functionCode?.fromStateRequired !== 'N' ||
                functionCode?.fromStockPoolRequired !== 'N' ||
                functionCode?.batchDateRequired !== 'N' ||
                functionCode?.code === 'MOVE') && (
                <>
                    <Grid size={2}>
                        {functionCode?.fromStates && (
                            <Dropdown
                                value={fromState}
                                disabled={disabled}
                                fullWidth
                                label="From State"
                                propertyName="fromState"
                                allowNoValue
                                items={functionCode?.fromStates?.map(s => ({
                                    id: s,
                                    displayText: s
                                }))}
                                onChange={setItemValue}
                            />
                        )}
                    </Grid>
                    <Grid size={2}>
                        <Dropdown
                            value={fromStockPool}
                            disabled={disabled}
                            fullWidth
                            label="From Stock Pool"
                            propertyName="fromStockPool"
                            allowNoValue
                            items={stockPoolItems?.map(s => ({
                                id: s.stockPoolCode,
                                displayText: s.stockPoolCode
                            }))}
                            onChange={setItemValue}
                        />
                    </Grid>
                    <Grid size={2}>
                        <DatePicker
                            value={batchDate}
                            disabled={disabled}
                            onChange={newVal => setItemValue('batchDate', newVal)}
                            label="Batch Date"
                            propertyName="batchDate"
                        />
                    </Grid>
                    <Grid size={2}>
                        <Button
                            onClick={() => setPickStockDialogVisible(true)}
                            variant="outlined"
                            style={{ marginTop: '32px' }}
                            disabled={disabled || !doPickStock}
                        >
                            Pick Stock
                        </Button>
                    </Grid>
                    <Grid size={4} />
                </>
            )}
            {functionCode?.fromLocationRequired !== 'N' && (
                <>
                    <Grid size={2}>
                        <Search
                            propertyName="fromLocationCode"
                            label="From Loc"
                            resultsInModal
                            resultLimit={100}
                            helperText="Enter a search term and press enter to search"
                            value={fromLocationCode}
                            handleValueChange={setItemValue}
                            search={searchLocations}
                            loading={locationsSearchLoading}
                            searchResults={locationsSearchResults}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={r => {
                                setItemValue('fromLocationId', r.locationId);
                                setItemValue('fromLocationCode', r.locationCode);
                            }}
                            clearSearch={clearLocationsSearch}
                            autoFocus={false}
                        />
                    </Grid>
                    <Grid size={2}>
                        <InputField
                            value={fromPalletNumber}
                            onChange={setItemValue}
                            disabled={disabled}
                            label="From Pallet"
                            propertyName="fromPalletNumber"
                        />
                    </Grid>
                    <Grid size={8} />
                </>
            )}
            <Grid size={2}>
                {functionCode?.fromStates && (
                    <Dropdown
                        value={toState}
                        disabled={disabled}
                        fullWidth
                        label="To State"
                        propertyName="toState"
                        allowNoValue
                        items={functionCode?.toStates?.map(s => ({
                            id: s,
                            displayText: s
                        }))}
                        onChange={setItemValue}
                    />
                )}
            </Grid>
            <Grid size={2}>
                <Dropdown
                    value={toStockPool}
                    disabled={disabled}
                    fullWidth
                    label="To Stock Pool"
                    propertyName="toStockPool"
                    allowNoValue
                    items={stockPoolItems?.map(s => ({
                        id: s.stockPoolCode,
                        displayText: s.stockPoolCode
                    }))}
                    onChange={setItemValue}
                />
            </Grid>
            <Grid size={8} />
            <Grid size={2}>
                <Search
                    propertyName="toLocationCode"
                    label="To Loc"
                    resultsInModal
                    resultLimit={100}
                    helperText="Enter a search term and press enter to search"
                    value={toLocationCode}
                    handleValueChange={setItemValue}
                    search={searchLocations}
                    loading={locationsSearchLoading}
                    searchResults={locationsSearchResults}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={r => {
                        setItemValue('toLocationId', r.locationId);
                        setItemValue('toLocationCode', r.locationCode);
                    }}
                    clearSearch={clearLocationsSearch}
                    disabled={disabled}
                    autoFocus={false}
                />
            </Grid>
            <Grid size={2}>
                <InputField
                    value={toPalletNumber}
                    onChange={setItemValue}
                    disabled={disabled}
                    label="To Pallet"
                    propertyName="toPalletNumber"
                />
            </Grid>
            <Grid size={8} />
            {pickStockDialogVisible && (
                <PickStockDialog
                    open={pickStockDialogVisible}
                    setOpen={setPickStockDialogVisible}
                    partNumber={partNumber}
                    quantity={quantity}
                    handleConfirm={moves => {
                        doPickStock(moves);
                    }}
                />
            )}
        </>
    );
}

export default StockOptions;
