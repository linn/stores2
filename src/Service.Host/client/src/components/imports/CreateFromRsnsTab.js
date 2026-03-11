import React, { useState } from 'react';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Grid from '@mui/material/Grid';
import CheckIcon from '@mui/icons-material/Check';
import DeleteIcon from '@mui/icons-material/Delete';
import Tooltip from '@mui/material/Tooltip';
import { DataGrid } from '@mui/x-data-grid';
import { ErrorCard, InputField, Loading, utilities } from '@linn-it/linn-form-components-library';
import { useNavigate } from 'react-router-dom';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';

function CreateFromRsnsTab() {
    const navigate = useNavigate();
    const [rsnNumber, setRsnNumber] = useState();
    const [error, setError] = useState();
    const [rsns, setRsns] = useState([]);

    const {
        send: getRsn,
        isLoading,
        result: rsnResult,
        clearData
    } = useGet(itemTypes.rsns.url, true);

    const handleRsnChange = (property, newValue) => {
        setRsnNumber(newValue);
        if (rsnResult) {
            clearData();
        }
        if (error) {
            setError(null);
        }
    };

    const addRsn = () => {
        if (rsnNumber && !rsns.some(r => r.rsnNumber === rsnNumber) && !isLoading) {
            getRsn(rsnNumber);
        }
    };

    const handleDeleteRow = row => {
        setRsns(prev => prev.filter(r => r.rsnNumber !== row.rsnNumber));
    };

    if (rsnResult && !rsns.some(r => r.rsnNumber === rsnNumber)) {
        if (rsnResult.salesOutlet) {
            if (rsnResult.salesOutlet.countryCode == 'GB') {
                setError('Not an import');
                clearData();
            } else if (rsns.length > 0 && rsnResult.salesOutlet != rsns[0].salesOutlet) {
                setError('Cannot mix sales outlets');
                clearData();
            } else if (utilities.getHref(rsnResult, 'import-book')) {
                setError('Already has import book');
                clearData();
            } else {
                setRsns([...rsns, rsnResult]);
                setRsnNumber(null);
                clearData();
            }
        }
    }

    const columns = [
        { field: 'rsnNumber', headerName: 'RSN Number', width: 110 },
        { field: 'articleNumber', headerName: 'Article Number', width: 140 },
        {
            field: 'ipr',
            headerName: 'IPR',
            width: 80,
            renderCell: params => (params.row.ipr ? <CheckIcon /> : null)
        },
        {
            field: 'delete',
            headerName: '',
            width: 120,
            renderCell: params => (
                <Tooltip title="Delete">
                    <div>
                        <IconButton
                            aria-label="delete"
                            size="small"
                            onClick={() => handleDeleteRow(params.row)}
                        >
                            <DeleteIcon fontSize="inherit" />
                        </IconButton>
                    </div>
                </Tooltip>
            )
        }
    ];

    const salesOutlet = () => {
        if (rsns.length > 0 && rsns[0].salesOutlet) {
            return rsns[0].salesOutlet;
        }
        if (rsnResult && rsnResult.salesOutlet) {
            return rsnResult.salesOutlet;
        }
        return null;
    };

    const createUrl = () => {
        return `create?rsnNumbers=${rsns.map(r => r.rsnNumber).join(',')}`;
    };

    return (
        <>
            <Grid container spacing={3}>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={rsnNumber}
                        type="number"
                        onChange={handleRsnChange}
                        label="RSN Number"
                        propertyName="rsnNumber"
                    />
                </Grid>
                <Grid size={2}>
                    <Button
                        onClick={addRsn}
                        variant="outlined"
                        style={{ marginTop: '29px' }}
                        disabled={!rsnNumber || isLoading}
                    >
                        Add RSN
                    </Button>
                </Grid>
                <Grid size={7}></Grid>
                {isLoading && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {salesOutlet() && (
                    <>
                        <Grid size={3}>
                            <InputField
                                disabled
                                value={` ${salesOutlet().accountId}/${salesOutlet().outletNumber}`}
                                fullWidth
                                label="Outlet"
                                propertyName="outlet"
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                disabled
                                value={salesOutlet().name}
                                fullWidth
                                label="Name"
                                propertyName="outletName"
                            />
                        </Grid>
                        <Grid size={3}>
                            <InputField
                                disabled
                                value={salesOutlet().countryName}
                                fullWidth
                                label="Country"
                                propertyName="outletCountry"
                            />
                        </Grid>
                    </>
                )}
                {error && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={error} />
                    </Grid>
                )}
                {rsns.length > 0 && (
                    <>
                        <Grid size={12}>
                            <DataGrid
                                getRowId={r => r.rsnNumber}
                                rows={rsns}
                                columns={columns}
                                density="compact"
                                hideFooter
                            />
                        </Grid>
                        <Grid size={6}>
                            <Button
                                onClick={() => navigate(createUrl())}
                                variant="contained"
                                color="primary"
                            >
                                Create
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </>
    );
}

export default CreateFromRsnsTab;
