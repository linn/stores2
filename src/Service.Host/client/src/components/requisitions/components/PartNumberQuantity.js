import React from 'react';
import Grid from '@mui/material/Grid2';
import { InputField, Search } from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import itemTypes from '../../../itemTypes';
import useSearch from '../../../hooks/useSearch';

function PartNumberQuantity({
    partNumber = null,
    partDescription = null,
    setPart,
    quantity = null,
    disabled = false,
    setQuantity,
    showQuantity = true,
    shouldRender = true
}) {
    const {
        search: searchParts,
        results: partsSearchResults,
        loading: partsSearchLoading,
        clear: clearPartsSearch
    } = useSearch(itemTypes.parts.url, 'id', 'partNumber', 'description');

    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={2}>
                <Search
                    propertyName="partNumber"
                    label="Part"
                    resultsInModal
                    resultLimit={100}
                    helperText="Enter a search term and press enter or TAB"
                    value={partNumber}
                    disabled={disabled}
                    handleValueChange={(_, newVal) => {
                        setPart({ partNumber: newVal });
                    }}
                    search={searchParts}
                    loading={partsSearchLoading}
                    searchResults={partsSearchResults}
                    priorityFunction="closestMatchesFirst"
                    onKeyPressFunctions={[
                        {
                            keyCode: 9,
                            action: () => setPart({ partNumber: partNumber?.toUpperCase() })
                        }
                    ]}
                    onResultSelect={r => {
                        setPart(r);
                    }}
                    clearSearch={clearPartsSearch}
                    autoFocus={false}
                />
            </Grid>
            <Grid size={4}>
                <InputField
                    fullWidth
                    value={partDescription}
                    disabled={disabled}
                    onChange={() => {}}
                    label="Desc"
                    propertyName="partDescription"
                />
            </Grid>
            {showQuantity ? (
                <Grid size={4}>
                    <InputField
                        value={quantity}
                        onChange={(_, val) => setQuantity(val)}
                        disabled={disabled}
                        label="Qty"
                        propertyName="quantity"
                    />
                </Grid>
            ) : (
                <Grid size={4} />
            )}
            <Grid size={2} />
        </>
    );
}

PartNumberQuantity.propTypes = {
    partNumber: PropTypes.string,
    partDescription: PropTypes.string,
    setPart: PropTypes.func.isRequired,
    disabled: PropTypes.bool,
    quantity: PropTypes.number,
    setQuantity: PropTypes.func.isRequired,
    showQuantity: PropTypes.bool,
    shouldRender: PropTypes.bool
};

PartNumberQuantity.defaultProps = {
    partNumber: null,
    partDescription: null,
    quantity: null,
    showQuantity: true,
    shouldRender: true,
    disabled: false
};

export default PartNumberQuantity;
