import React, { useState, useEffect } from 'react';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import queryString from 'query-string';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid2';
import { utilities, InputField, Dropdown } from '@linn-it/linn-form-components-library';
import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';

function SearchRequisitions() {
    const { send, isLoading, result } = useGet(itemTypes.requisitions.url);
    const navigate = useNavigate();

    const [options, setOptions] = useState({ includeCancelled: false });

    const handleOptionChange = (property, newValue) => {
        if (property === 'includeCancelled') {
            setOptions(o => ({ ...o, [property]: newValue === 'Y' }));
        } else if (newValue) {
            setOptions(o => ({ ...o, [property]: newValue }));
        } else {
            const opt = { ...options };
            delete opt[property];
            setOptions(() => opt);
        }
    };

    useEffect(() => {
        const handleKeyDown = event => {
            if (event.key === 'Enter') {
                const query = queryString.stringify(options);
                if (options.reqNumber || options.comments) {
                    send(null, `?${query}`);
                }
            }
        };

        window.addEventListener('keydown', handleKeyDown);

        return () => {
            window.removeEventListener('keydown', handleKeyDown);
        };
    }, [options, send]);

    const columns = [
        {
            field: 'reqNumber',
            headerName: 'Req Number',
            width: 200
        },
        { field: 'comments', headerName: 'Comments', width: 200 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">Search Requisitions</Typography>
                </Grid>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={options.reqNumber}
                        type="number"
                        onChange={handleOptionChange}
                        label="Req Number"
                        helperText="if you know the req number"
                        propertyName="reqNumber"
                    />
                </Grid>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={options.comments}
                        onChange={handleOptionChange}
                        label="Comments"
                        propertyName="comments"
                        helperText="you can search for a string included in the comments field of the header"
                    />
                </Grid>
                <Grid size={3}>
                    <Dropdown
                        fullWidth
                        value={options.includeCancelled ? 'Y' : 'N'}
                        onChange={handleOptionChange}
                        label="Include Cancelled"
                        allowNoValue={false}
                        propertyName="includeCancelled"
                        items={['N', 'Y']}
                    />
                </Grid>

                <Grid size={12}>
                    <DataGrid
                        rows={
                            result?.map(r => ({
                                ...r,
                                id: r.reqNumber
                            })) || []
                        }
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

export default SearchRequisitions;
