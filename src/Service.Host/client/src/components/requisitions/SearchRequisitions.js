import React, { useState, useEffect } from 'react';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import queryString from 'query-string';
import moment from 'moment';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
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

    const tryToGoToReq = selectedNumber => {
        navigate(`/requisitions/${selectedNumber}`);
    };

    const doQuery = () => {
        const query = queryString.stringify(options);
        if (options.reqNumber || options.comments || options.documentNumber) {
            send(null, `?${query}`);
        }
    };

    useEffect(() => {
        const handleKeyDown = event => {
            if (event.key === 'Enter') {
                const query = queryString.stringify(options);
                if (options.reqNumber || options.comments || options.documentNumber) {
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
            width: 140
        },
        { field: 'created', headerName: 'Created', width: 100 },
        { field: 'createdByName', headerName: 'By', width: 200 },
        { field: 'doc1', headerName: 'Doc1', width: 100 },
        { field: 'comments', headerName: 'Comments', width: 200 }
    ];

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Search Reqs">
            <Grid container spacing={3}>
                <Grid size={11}>
                    <Typography variant="h4">View, Search, Create Requisitions</Typography>
                </Grid>
                <Grid size={1}>
                    <CreateButton createUrl="/requisitions/create" />
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
                <Grid size={1}>
                    <Button
                        onClick={() => tryToGoToReq(options.reqNumber)}
                        variant="outlined"
                        style={{ marginTop: '29px' }}
                    >
                        Go
                    </Button>
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
                    <Button onClick={doQuery} variant="outlined" style={{ marginTop: '29px' }}>
                        Search
                    </Button>
                </Grid>
                <Grid size={2} />
                <Grid size={2}>
                    <Dropdown
                        fullWidth
                        value={options.documentName}
                        onChange={handleOptionChange}
                        label="Doc Type"
                        allowNoValue
                        propertyName="documentName"
                        items={[
                            { id: 'CONS', displayText: 'Consignment' },
                            { id: 'CO', displayText: 'Credit Order' },
                            { id: 'L', displayText: 'Loan' },
                            { id: 'PO', displayText: 'Purchase Order' },
                            { id: 'RO', displayText: 'Returns Order' },
                            { id: 'R', displayText: 'RSN' },
                            { id: 'WO', displayText: 'Works Order' }
                        ]}
                    />
                </Grid>
                <Grid size={3}>
                    {options.documentName && (
                        <InputField
                            fullWidth
                            value={options.documentNumber}
                            onChange={handleOptionChange}
                            label="Doc Number"
                            helperText="if you know the doc number"
                            propertyName="documentNumber"
                        />
                    )}
                </Grid>
                <Grid size={2}>
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
                <Grid size={5} />

                <Grid size={12}>
                    <DataGrid
                        rows={
                            result?.map(r => ({
                                ...r,
                                id: r.reqNumber,
                                created: moment(r.dateCreated).format('DD-MMM-YYYY'),
                                doc1: `${r.document1Name} ${r.document1}`
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
