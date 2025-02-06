import React from 'react';
import Grid from '@mui/material/Grid2';
import { InputField, Search } from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import itemTypes from '../../../itemTypes';
import useSearch from '../../../hooks/useSearch';

function DepartmentNominal({
    departmentCode,
    departmentDescription,
    setDepartment,
    nominalCode,
    nominalDescription,
    setNominal,
    shouldRender
}) {
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

    if (!shouldRender) {
        return '';
    }

    return (
        <>
            <Grid size={2}>
                <Search
                    propertyName="departmentCode"
                    label="Department"
                    resultsInModal
                    resultLimit={100}
                    helperText="Enter a search term and press enter to look up departments"
                    value={departmentCode}
                    handleValueChange={(_, newVal) => {
                        setDepartment({ departmentCode: newVal });
                    }}
                    search={searchDepartments}
                    loading={departmentsSearchLoading}
                    searchResults={departmentsSearchResults}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={r => {
                        setDepartment(r);
                    }}
                    clearSearch={clearDepartmentsSearch}
                    autoFocus={false}
                />
            </Grid>
            <Grid size={4}>
                <InputField
                    fullWidth
                    value={departmentDescription}
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
                    value={nominalCode}
                    handleValueChange={(_, newVal) => {
                        setNominal({ nominalCode: newVal });
                    }}
                    search={searchNominals}
                    loading={nominalsSearchLoading}
                    searchResults={nominalsSearchResults}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={r => {
                        setNominal(r);
                    }}
                    clearSearch={clearNominalsSearch}
                    autoFocus={false}
                />
            </Grid>
            <Grid size={4}>
                <InputField
                    fullWidth
                    value={nominalDescription}
                    onChange={() => {}}
                    label="Desc"
                    propertyName="nominalDescription"
                />
            </Grid>
        </>
    );
}

DepartmentNominal.propTypes = {
    departmentCode: PropTypes.string,
    departmentDescription: PropTypes.string,
    setDepartment: PropTypes.func.isRequired,
    nominalCode: PropTypes.string,
    nominalDescription: PropTypes.string,
    setNominal: PropTypes.func.isRequired,
    shouldRender: PropTypes.bool
};

DepartmentNominal.defaultProps = {
    departmentCode: null,
    departmentDescription: null,
    nominalCode: null,
    nominalDescription: null,
    shouldRender: true
};

export default DepartmentNominal;
