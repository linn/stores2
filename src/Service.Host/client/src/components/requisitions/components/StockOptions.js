import React from 'react';
import { Dropdown, DatePicker, utilities } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid2';
import PropTypes from 'prop-types';

function StockOptions({
    stockStates,
    stockPools,
    fromState = null,
    setFromState,
    fromStockPool = null,
    setFromStockPool,
    batchDate = null,
    setBatchDate,
    toState = null,
    setToState,
    toStockPool = null,
    setToStockPool,
    disabled = false,
    shouldRender = true
}) {
    if (!shouldRender) {
        return '';
    }

    const stockPoolItems = stockPools?.length
        ? utilities.sortEntityList(stockPools, 'stockPoolCode')
        : [];

    return (
        <>
            <Grid size={2}>
                <Dropdown
                    value={fromState}
                    disabled={disabled}
                    fullWidth
                    label="From State"
                    propertyName="fromState"
                    allowNoValue
                    items={stockStates?.map(s => ({
                        id: s.state,
                        displayText: s.state
                    }))}
                    onChange={(_, newValue) => setFromState(newValue)}
                />
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
                    onChange={(_, newValue) => setFromStockPool(newValue)}
                />
            </Grid>
            <Grid size={2}>
                <DatePicker
                    value={batchDate}
                    disabled={disabled}
                    onChange={setBatchDate}
                    label="Batch Date"
                    propertyName="batchDate"
                />
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Dropdown
                    value={toState}
                    disabled={disabled}
                    fullWidth
                    label="To State"
                    propertyName="toState"
                    allowNoValue
                    items={stockStates?.map(s => ({
                        id: s.state,
                        displayText: s.state
                    }))}
                    onChange={(_, newValue) => setToState(newValue)}
                />
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
                    onChange={(_, newValue) => setToStockPool(newValue)}
                />
            </Grid>
            <Grid size={8} />
        </>
    );
}

StockOptions.propTypes = {
    stockStates: PropTypes.arrayOf(PropTypes.shape({ state: PropTypes.string })).isRequired,
    stockPools: PropTypes.arrayOf(PropTypes.shape({ stockPoolCode: PropTypes.string })).isRequired,
    fromState: PropTypes.string,
    setFromState: PropTypes.func.isRequired,
    fromStockPool: PropTypes.string,
    setFromStockPool: PropTypes.func.isRequired,
    toState: PropTypes.string,
    setToState: PropTypes.func.isRequired,
    toStockPool: PropTypes.string,
    setToStockPool: PropTypes.func.isRequired,
    batchDate: PropTypes.string,
    setBatchDate: PropTypes.func.isRequired,
    disabled: PropTypes.bool,
    shouldRender: PropTypes.bool
};

StockOptions.defaultProps = {
    fromState: null,
    fromStockPool: null,
    toState: null,
    toStockPool: null,
    batchDate: null,
    disabled: false,
    shouldRender: true
};

export default StockOptions;
