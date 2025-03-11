import React, { useEffect } from 'react';
import Grid from '@mui/material/Grid2';
import { InputField, Search } from '@linn-it/linn-form-components-library';
import itemTypes from '../../../itemTypes';
import useSearch from '../../../hooks/useSearch';

function DepartmentNominal({
    departmentCode = null,
    departmentDescription = null,
    setDepartment,
    nominalCode = null,
    nominalDescription = null,
    setNominal,
    shouldRender = true,
    enterNominal = true
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

    const setCode = code => {
        if (code?.length && code.length < 10) {
            return code.padStart(10, '0');
        }

        return code;
    };

    const handleDepartmentUpdate = () => {
        searchDepartments(setCode(departmentCode));
    };

    useEffect(() => {
        if (departmentsSearchResults?.length === 1) {
            setDepartment(departmentsSearchResults[0]);
        }
    }, [departmentsSearchResults, setDepartment]);

    const handleNominalUpdate = () => {
        searchNominals(setCode(nominalCode));
    };

    useEffect(() => {
        if (nominalsSearchResults?.length === 1) {
            setNominal(nominalsSearchResults[0]);
        }
    }, [setNominal, nominalsSearchResults]);

    if (!shouldRender) {
        return null;
    }

    return (
        <>
            <Grid size={2}>
                <Search
                    propertyName="departmentCode"
                    label="Department"
                    resultsInModal
                    resultLimit={100}
                    helperText={
                        departmentDescription
                            ? ''
                            : '<Enter> to search or <Tab> to select if you have entered a known dept code'
                    }
                    value={departmentCode}
                    handleValueChange={(_, newVal) => {
                        setDepartment({ departmentCode: newVal });
                    }}
                    search={searchDepartments}
                    loading={departmentsSearchLoading}
                    searchResults={departmentsSearchResults}
                    priorityFunction="closestMatchesFirst"
                    onKeyPressFunctions={[{ keyCode: 9, action: handleDepartmentUpdate }]}
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
                    helperText={
                        nominalDescription
                            ? ''
                            : '<Enter> to search or <Tab> to select if you have entered a known nominal code'
                    }
                    value={nominalCode}
                    handleValueChange={(_, newVal) => {
                        setNominal({ nominalCode: newVal });
                    }}
                    search={searchNominals}
                    loading={nominalsSearchLoading}
                    searchResults={nominalsSearchResults}
                    onKeyPressFunctions={[{ keyCode: 9, action: handleNominalUpdate }]}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={r => {
                        setNominal(r);
                    }}
                    clearSearch={clearNominalsSearch}
                    disabled={!enterNominal}
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

export default DepartmentNominal;
