import React, { useEffect, useState, useRef } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { useNavigate } from 'react-router-dom';
import moment from 'moment';
import queryString from 'query-string';
import { DataGrid } from '@mui/x-data-grid';
import { InputField, utilities } from '@linn-it/linn-form-components-library';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';

function SearchTab() {
    const navigate = useNavigate();
    const inputRef = useRef(null);

    const [options, setOptions] = useState({});

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

    const goToImportBook = () => {
        navigate(`/stores2/import-books/${options.importBookId}`);
    };

    const doSearch = () => {
        const query = queryString.stringify(options);
        if (options.transportBillNumber || options.customsEntryCode) {
            send(null, `?${query}`);
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
            <Grid container spacing={3}>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={options.importBookId}
                        type="number"
                        onChange={handleOptionChange}
                        label="Import Book Id"
                        helperText="if you know the Import Book Id"
                        propertyName="importBookId"
                        autoFocus
                        ref={inputRef}
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        onClick={goToImportBook}
                        variant="outlined"
                        style={{ marginTop: '29px' }}
                    >
                        Go
                    </Button>
                </Grid>
                <Grid size={8} />
                <Grid size={4}>
                    <InputField
                        fullWidth
                        value={options.transportBillNumber}
                        onChange={handleOptionChange}
                        label="Transport Bill Number/AWB"
                        propertyName="transportBillNumber"
                        disabled={options.customsEntryCode}
                    />
                </Grid>
                <Grid size={4}>
                    <InputField
                        fullWidth
                        value={options.customsEntryCode}
                        onChange={handleOptionChange}
                        label="Customs Entry Code"
                        propertyName="customsEntryCode"
                        disabled={options.transportBillNumber}
                    />
                </Grid>
                <Grid size={4}>
                    <Button onClick={doSearch} variant="outlined" style={{ marginTop: '29px' }}>
                        Search
                    </Button>
                </Grid>
                {result && (
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
                                navigate(utilities.getSelfHref(clicked.row));
                            }}
                            loading={isLoading}
                            checkboxSelection={false}
                            density="compact"
                            hideFooter
                        />
                    </Grid>
                )}
            </Grid>
        </>
    );
}

export default SearchTab;
