import React, { useEffect } from 'react';
import Grid from '@mui/material/Grid';
import { InputField, Search } from '@linn-it/linn-form-components-library';
import itemTypes from '../../../itemTypes';
import useSearch from '../../../hooks/useSearch';

function PartNumberQuantity({
    partNumber = null,
    partDescription = null,
    setPart = null,
    quantity = null,
    disabled = false,
    setQuantity = null,
    showQuantity = true,
    shouldRender = true,
    partNumberProperty = 'partNumber',
    partDescriptionProperty = 'partDescription',
    partLabel = 'Part'
}) {
    const {
        search: searchParts,
        results: partsSearchResults,
        loading: partsSearchLoading,
        clear: clearPartsSearch
    } = useSearch(itemTypes.parts.url, 'id', 'partNumber', 'description');

    useEffect(() => {
        if (partsSearchResults?.length) {
            const exactMatch = partsSearchResults.find(
                part => part.partNumber.toUpperCase() === partNumber?.toUpperCase()
            );
            if (exactMatch) {
                setPart(exactMatch);
                clearPartsSearch();
            }
        }
    }, [partsSearchResults, partNumber, clearPartsSearch, setPart]);

    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={2}>
                <Search
                    propertyName={partNumberProperty}
                    label={partLabel}
                    resultsInModal
                    resultLimit={100}
                    helperText={
                        disabled || !setPart ? '' : '<Enter> to search part. <Tab> to select'
                    }
                    value={partNumber}
                    disabled={disabled || !setPart}
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
                            action: () => {
                                if (partNumber) {
                                    searchParts(partNumber?.toUpperCase());
                                }
                            }
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
                    disabled
                    onChange={() => {}}
                    label="Desc"
                    propertyName={partDescriptionProperty}
                />
            </Grid>
            {showQuantity ? (
                <Grid size={4}>
                    <InputField
                        value={quantity}
                        onChange={(_, val) => setQuantity(val)}
                        disabled={disabled || !setQuantity}
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

export default PartNumberQuantity;
