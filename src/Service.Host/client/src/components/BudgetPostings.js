import React from 'react';
import { DataGrid } from '@mui/x-data-grid';
import PropTypes from 'prop-types';

function BudgetPostings({ budgetPostings }) {
    const postingColumns = [
        { field: 'sequence', headerName: 'Seq', width: 80 },
        { field: 'debitOrCreditDisplay', headerName: 'Debit/Credit', width: 100 },
        { field: 'quantity', type: 'number', headerName: 'Quantity', width: 100 },
        { field: 'nominalCode', headerName: 'Nominal', width: 130 },
        { field: 'nominalDescription', headerName: 'Description', width: 250 },
        { field: 'departmentCode', headerName: 'Department', width: 130 },
        { field: 'departmentDescription', headerName: 'Description', width: 250 },
        { field: 'product', headerName: 'Product', width: 130 },
        { field: 'building', headerName: 'Building', width: 130 },
        { field: 'vehicle', headerName: 'Vehicle', width: 130 },
        { field: 'person', headerName: 'Person', width: 130 }
    ];

    const getBudgetPostings = () => {
        if (budgetPostings && budgetPostings.length) {
            return budgetPostings.map((p, i) => ({
                ...p,
                id: i,
                debitOrCreditDisplay: p.debitOrCredit === 'D' ? 'Debit' : 'Credit',
                nominalCode: p.nominalAccount?.nominalCode,
                nominalDescription: p.nominalAccount?.nominal?.description,
                departmentCode: p.nominalAccount?.departmentCode,
                departmentDescription: p.nominalAccount?.department?.description
            }));
        }

        return [];
    };

    return (
        <DataGrid
            rows={getBudgetPostings()}
            columns={postingColumns}
            density="compact"
            autoHeight
            hideFooterSelectedRowCount
            hideFooter={getBudgetPostings().length <= 100}
            initialState={{
                columns: {
                    columnVisibilityModel: {
                        product: false,
                        building: false,
                        person: false,
                        vehicle: false
                    }
                }
            }}
        />
    );
}

BudgetPostings.propTypes = {
    budgetPostings: PropTypes.arrayOf(PropTypes.shape({})).isRequired
};

export default BudgetPostings;
