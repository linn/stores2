import React from 'react';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';

function OrderDetails({ orderDetails }) {
    const columns = [
        { field: 'lineNumber', headerName: 'Line Num', width: 80 },
        { field: 'lineType', headerName: 'Line Type', width: 140 },
        { field: 'lineDocument', headerName: 'Document', width: 140 },
        { field: 'orderDescription', headerName: 'Description', width: 240 },
        { field: 'tariffCode', headerName: 'Tariff no', width: 100 },
        { field: 'orderValue', headerName: 'Customs Value', width: 140 },
        { field: 'dutyValue', headerName: 'Duty Value', width: 120 },
        { field: 'qty', headerName: 'Qty', width: 100 }
    ];

    if (!orderDetails || orderDetails.length === 0) {
        return null;
    }

    return (
        <Grid size={12}>
            <DataGrid
                getRowId={r => r.lineNumber}
                rows={orderDetails}
                columns={columns}
                density="compact"
                hideFooter
            />
        </Grid>
    );
}

export default OrderDetails;
