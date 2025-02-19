import React, { useState, useEffect } from 'react';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import queryString from 'query-string';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import {
    utilities,
    InputField,
    Dropdown,
    CreateButton
} from '@linn-it/linn-form-components-library';
import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';

function PendingRequisitions() {
    const { send, isLoading, result } = useGet(itemTypes.requisitions.url);
    const navigate = useNavigate();

    const [options, setOptions] = useState({ pending: true });

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

    const getReqs = () => {
        const query = queryString.stringify(options);
        send(null, `?${query}`);
    }

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
                <Grid size={11}>
                    <Typography variant="h4">Pending Requisitions</Typography>
                </Grid>
                <Grid size={1}>
                    Outstanding Reqs
                </Grid>
                <Grid size={3}>
                    field1
                </Grid>
                <Grid size={3}>
                    field2
                </Grid>
                <Grid size={3}>
                    field3
                </Grid>
                <Grid size={3}>
                    <Button variant="contained" sx={{ marginTop: '30px' }} onClick={getReqs}>
                        View Reqs
                    </Button>
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

export default PendingRequisitions;
