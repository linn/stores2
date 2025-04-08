import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import moment from 'moment';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import { utilities, Dropdown, Search } from '@linn-it/linn-form-components-library';
import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import useInitialise from '../../hooks/useInitialise';
import useSearch from '../../hooks/useSearch';

function PendingRequisitions() {
    const { send, isLoading, result } = useGet(itemTypes.requisitions.url);
    const navigate = useNavigate();

    const { result: accountingCompaniesResult } = useInitialise(itemTypes.accountingCompany.url);

    const { result: functionCodesResult } = useInitialise(itemTypes.functionCodes.url);

    const {
        search: searchDepartments,
        results: departmentsSearchResults,
        loading: departmentsSearchLoading,
        clear: clearDepartmentsSearch
    } = useSearch(itemTypes.departments.url, 'departmentCode', 'departmentCode', 'description');

    const {
        search: searchEmployees,
        results: employeesSearchResults,
        loading: employeesSearchLoading,
        clear: clearEmployeesSearch
    } = useSearch(itemTypes.historicEmployees.url, 'id', 'fullName', 'fullName', false, true);

    const [filters, setFilters] = useState({
        accountingCompany: 'LINN',
        functionCode: null,
        daysAgo: 0,
        docType: null,
        needsAuth: null,
        department: null,
        departmentCode: null,
        departmentName: null,
        employee: null,
        employeeId: null,
        employeeName: null
    });

    const handleFiltersChange = (propertyName, newValue) => {
        setFilters(current => ({ ...current, [propertyName]: newValue }));
    };

    const getReqs = () => {
        send(null, '?pending=true');
    };

    const clearFilters = () => {
        setFilters({
            accountingCompany: 'LINN',
            functionCode: null,
            docType: null,
            needsAuth: null,
            daysAgo: 0,
            department: null,
            departmentCode: null,
            departmentName: null,
            employee: null,
            employeeId: null,
            employeeName: null
        });
    };

    const columns = [
        {
            field: 'reqNumber',
            headerName: 'Req Num',
            width: 100
        },
        { field: 'created', headerName: 'Created', width: 120 },
        { field: 'createdByName', headerName: 'By', width: 160 },
        { field: 'department', headerName: 'Department', width: 160 },
        { field: 'requiresAuth', headerName: 'Need Auth', width: 100 },
        { field: 'authorisedByName', headerName: 'Auth By', width: 160 },
        { field: 'comments', headerName: 'Comments', width: 160 },
        { field: 'functionCode', headerName: 'Function Code', width: 160 },
        { field: 'document1', headerName: 'Doc1', width: 120 }
    ];

    const overDaysAgo = dateString => {
        if (filters.daysAgo) {
            return moment(dateString).isBefore(moment().subtract(filters.daysAgo, 'days'));
        }
        return true;
    };

    const matchesDocType = documentName => {
        if (filters.docType) {
            if (filters.docType.includes(',')) {
                return filters.docType.includes(documentName);
            }
            return filters.docType === documentName;
        }
        return true;
    };

    const matchesNeedsAuth = requiresAuthorisation => {
        if (filters.needsAuth !== null) {
            return filters.needsAuth ? requiresAuthorisation : !requiresAuthorisation;
        }
        return true;
    };

    const filteredReqs = reqs => {
        return reqs
            ? reqs.filter(
                  r =>
                      (!filteredReqs.accountingCompany ||
                          r.accountingCompanyCode === filters.accountingCompany) &&
                      (!filters.departmentCode ||
                          !filters.department ||
                          r.department?.departmentCode === filters.departmentCode) &&
                      (!filters.employeeId ||
                          !filters.employee ||
                          r.createdBy === filters.employeeId) &&
                      (!filters.functionCode || r.storesFunction?.code === filters.functionCode) &&
                      overDaysAgo(r.dateCreated) &&
                      matchesDocType(r.document1Name) &&
                      matchesNeedsAuth(r.requiresAuthorisation)
              )
            : [];
    };

    const reqRows = () => {
        return (
            filteredReqs(result).map(r => ({
                ...r,
                id: r.reqNumber,
                created: moment(r.dateCreated).format('DD-MMM-YYYY'),
                department: r.department?.description,
                functionCode: r.storesFunction?.code,
                requiresAuth: r.requiresAuthorisation === true ? 'Yes' : 'No',
                document1: `${r.document1Name}${r.document1 ? r.document1 : ''}`
            })) || []
        );
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={11}>
                    <Typography variant="h4">Pending Requisitions</Typography>
                </Grid>
                <Grid size={1} />
                <Grid size={3}>
                    {accountingCompaniesResult && (
                        <Dropdown
                            value={filters.accountingCompany}
                            fullWidth
                            label="Accounting Company"
                            propertyName="accountingCompany"
                            allowNoValue
                            items={accountingCompaniesResult.map(c => ({
                                id: c.name,
                                displayText: c.description
                            }))}
                            onChange={handleFiltersChange}
                        />
                    )}
                </Grid>
                <Grid size={3}>
                    {functionCodesResult && (
                        <Dropdown
                            value={filters.functionCode}
                            fullWidth
                            label="Function Code"
                            propertyName="functionCode"
                            allowNoValue
                            items={functionCodesResult.map(c => ({
                                id: c.code,
                                displayText: `${c.code} - ${c.description}`
                            }))}
                            onChange={handleFiltersChange}
                        />
                    )}
                </Grid>
                <Grid size={3}>
                    <Dropdown
                        label="Age"
                        propertyName="daysAgo"
                        value={filters.daysAgo}
                        onChange={handleFiltersChange}
                        items={[
                            { id: 0, displayText: 'Any' },
                            { id: 5, displayText: 'Over 5 Days Old' }
                        ]}
                        allowNoValue={false}
                    />
                </Grid>
                <Grid size={3}>
                    <Dropdown
                        label="Document Type"
                        propertyName="docType"
                        value={filters.docType}
                        onChange={handleFiltersChange}
                        items={[
                            { id: 'REQ,L,WO', displayText: 'Req/Loan/Works Order' },
                            { id: 'REQ', displayText: 'Requisition' },
                            { id: 'L', displayText: 'Loan' },
                            { id: 'WO', displayText: 'Works Order' },
                            { id: 'PO', displayText: 'Purchase Order' }
                        ]}
                        allowNoValue
                    />
                </Grid>
                <Grid size={3}>
                    <Search
                        propertyName="departmentCode"
                        label="Department"
                        resultsInModal
                        resultLimit={100}
                        helperText={
                            filters.departmentName
                                ? filters.departmentName
                                : 'Enter a search term and press enter to look up departments'
                        }
                        value={filters.department}
                        handleValueChange={(_, newVal) => {
                            handleFiltersChange('department', newVal);
                        }}
                        search={searchDepartments}
                        loading={departmentsSearchLoading}
                        searchResults={departmentsSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={r => {
                            handleFiltersChange('departmentCode', r?.departmentCode);
                            handleFiltersChange('departmentName', r?.description);
                        }}
                        clearSearch={clearDepartmentsSearch}
                        autoFocus={false}
                    />
                </Grid>
                <Grid size={3}>
                    <Search
                        propertyName="employeeId"
                        label="Employee"
                        resultsInModal
                        resultLimit={100}
                        helperText={
                            filters.employeeName
                                ? filters.employeeName
                                : 'Enter a search term and press enter to look up employees'
                        }
                        value={filters.employee}
                        handleValueChange={(_, newVal) => {
                            handleFiltersChange('employee', newVal);
                        }}
                        search={searchEmployees}
                        loading={employeesSearchLoading}
                        searchResults={employeesSearchResults}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={r => {
                            handleFiltersChange('employeeId', r?.id);
                            handleFiltersChange('employeeName', r?.description);
                        }}
                        clearSearch={clearEmployeesSearch}
                        autoFocus={false}
                    />
                </Grid>
                <Grid size={3}>
                    <Dropdown
                        label="Needs Auth"
                        propertyName="needsAuth"
                        value={filters.needsAuth}
                        onChange={handleFiltersChange}
                        items={[
                            { id: true, displayText: 'Yes' },
                            { id: false, displayText: 'No' }
                        ]}
                        allowNoValue
                    />
                </Grid>
                <Grid size={3}>
                    <Button variant="contained" sx={{ marginTop: '30px' }} onClick={getReqs}>
                        View Reqs
                    </Button>
                    <Button variant="outlined" sx={{ marginTop: '30px' }} onClick={clearFilters}>
                        Clear
                    </Button>
                </Grid>

                <Grid size={12}>
                    <DataGrid
                        rows={reqRows()}
                        columns={columns}
                        onRowClick={clicked => {
                            navigate(utilities.getSelfHref(clicked.row));
                        }}
                        loading={isLoading}
                        checkboxSelection={false}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default PendingRequisitions;
