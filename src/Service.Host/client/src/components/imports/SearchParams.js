import React, { useEffect, useState, useRef } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import queryString from 'query-string';
import { DataGrid } from '@mui/x-data-grid';
import {
    DatePicker,
    Dropdown,
    ExportButton,
    InputField
} from '@linn-it/linn-form-components-library';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';

function SearchParams({ handleRowClick, table = true, exportUri, runReport }) {
    const inputRef = useRef(null);

    const [options, setOptions] = useState({
        fromDate: moment().subtract(1, 'months'),
        toDate: new Date()
    });

    const { send, isLoading, result } = useGet(itemTypes.importBooks.url);

    const handleOptionChange = (property, newValue) => {
        if (newValue) {
            setOptions(o => ({ ...o, [property]: newValue }));
        } else {
            const opt = { ...options };
            delete opt[property];
            setOptions(() => opt);
        }
    };

    const queryUri = () => {
        const searchParams = { ...options };

        if (searchParams.fromDate) {
            searchParams.fromDate = searchParams.fromDate.toISOString();
        }
        if (searchParams.toDate) {
            searchParams.toDate = searchParams.toDate.toISOString();
        }

        return queryString.stringify(searchParams);
    };

    const doSearch = () => {
        if (
            options.transportBillNumber ||
            options.customsEntryCode ||
            options.rsnNumber ||
            options.poNumber ||
            options.dateField ||
            options.status
        ) {
            send(null, `?${queryUri()}`);
        }
    };

    const doReport = () => {
        if (
            options.transportBillNumber ||
            options.customsEntryCode ||
            options.rsnNumber ||
            options.poNumber ||
            options.dateField ||
            options.status
        ) {
            runReport(null, `?${queryUri()}`);
        }
    };

    const columns = [
        {
            field: 'id',
            headerName: 'Import Id',
            width: 140
        },
        { field: 'created', headerName: 'Created', width: 120 },
        { field: 'supplierName', headerName: 'Supplier', width: 150 },
        { field: 'transportBillNumber', headerName: 'AWB', width: 200 },
        { field: 'customsEntryCode', headerName: 'Customs Entry', width: 300 }
    ];

    useEffect(() => {
        if (inputRef.current) {
            inputRef.current.focus();
        }
    }, []);

    return (
        <>
            <Grid size={3}>
                <InputField
                    fullWidth
                    value={options.transportBillNumber}
                    onChange={handleOptionChange}
                    label="Transport Bill Number/AWB"
                    propertyName="transportBillNumber"
                    disabled={options.customsEntryCode}
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    fullWidth
                    value={options.customsEntryCode}
                    onChange={handleOptionChange}
                    label="Customs Entry Code"
                    propertyName="customsEntryCode"
                    disabled={options.transportBillNumber}
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    fullWidth
                    type="number"
                    value={options.rsnNumber}
                    onChange={handleOptionChange}
                    label="RSN Number"
                    propertyName="rsnNumber"
                    disabled={options.customsEntryCode}
                />
            </Grid>
            <Grid size={3}>
                <InputField
                    fullWidth
                    type="number"
                    value={options.poNumber}
                    onChange={handleOptionChange}
                    label="PO Number"
                    propertyName="poNumber"
                    disabled={options.transportBillNumber}
                />
            </Grid>
            <Grid size={3}>
                <Dropdown
                    value={options.status}
                    fullWidth
                    label="Status"
                    propertyName="status"
                    allowNoValue
                    items={['Cleared', 'Instruction Sent', 'Raised', 'Cancelled', 'Received']}
                    onChange={handleOptionChange}
                />
            </Grid>
            <Grid size={3}>
                <Dropdown
                    value={options.dateField}
                    fullWidth
                    label="Date Filter"
                    propertyName="dateField"
                    allowNoValue
                    items={['Date Created', 'Date Received', 'Customs Date']}
                    onChange={handleOptionChange}
                />
            </Grid>
            <Grid size={3}>
                {options.dateField && (
                    <DatePicker
                        value={options.fromDate}
                        label="From Date"
                        propertyName="fromDate"
                        onChange={date => handleOptionChange('fromDate', date)}
                    />
                )}
            </Grid>
            <Grid size={3}>
                {options.dateField && (
                    <DatePicker
                        value={options.toDate}
                        label="To Date"
                        propertyName="toDate"
                        onChange={date => handleOptionChange('toDate', date)}
                    />
                )}
            </Grid>
            <Grid size={9}>
                {table && (
                    <Button onClick={doSearch} variant="outlined" style={{ marginTop: '29px' }}>
                        Search
                    </Button>
                )}
                {runReport && (
                    <Button onClick={doReport} variant="outlined" style={{ marginTop: '29px' }}>
                        Run Report
                    </Button>
                )}
            </Grid>
            <Grid size={3}>
                {exportUri && (
                    <div style={{ marginTop: '25px' }}>
                        <ExportButton
                            href={`${exportUri}?${queryUri()}`}
                            fileName="ImportReport.csv"
                            tooltipText="Download as CSV"
                        />
                    </div>
                )}
            </Grid>
            {result && table && (
                <Grid size={12}>
                    <DataGrid
                        rows={
                            result?.map(r => ({
                                ...r,
                                created: moment(r.dateCreated).format('DD-MMM-YYYY')
                            })) || []
                        }
                        columns={columns}
                        onRowClick={clicked => {
                            handleRowClick(clicked.row);
                        }}
                        loading={isLoading}
                        checkboxSelection={false}
                        density="compact"
                        hideFooter
                    />
                </Grid>
            )}
        </>
    );
}

export default SearchParams;
