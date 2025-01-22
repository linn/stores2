import React from 'react';
import { utilities } from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid2';
import moment from 'moment';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';

function StoresBudget({ storesBudget }) {
    const useStyles = makeStyles(() => ({
        pullRight: {
            float: 'right'
        },
        marginBelow: {
            marginBottom: '10px'
        }
    }));
    const classes = useStyles();

    const numberFormat = new Intl.NumberFormat('en-GB', { minimumFractionDigits: 2 });
    const reqHref = utilities.getSelfHref(storesBudget?.requisition);

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
        if (storesBudget) {
            const { storesBudgetPostings } = storesBudget;
            if (storesBudgetPostings && storesBudgetPostings.length) {
                return storesBudgetPostings.map((p, i) => ({
                    ...p,
                    id: i,
                    debitOrCreditDisplay: p.debitOrCredit === 'D' ? 'Debit' : 'Credit',
                    nominalCode: p.nominalAccount?.nominalCode,
                    nominalDescription: p.nominalAccount?.description,
                    departmentCode: p.nominalAccount?.departmentCode,
                    departmentDescription: p.nominalAccount?.departmentDescription
                }));
            }
        }

        return [];
    };

    return (
        <Grid container spacing={1}>
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Budget Id:
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">{storesBudget?.budgetId}</Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Date Booked:
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">
                    {moment(storesBudget?.dateBooked).format('DD MMM YYYY')}
                </Typography>
            </Grid>
            <Grid size={2} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Part Number:
                </Typography>
            </Grid>
            <Grid size={10}>
                <Typography variant="body1">
                    {storesBudget?.partNumber} - {storesBudget?.part?.description}
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Quantity
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">{storesBudget?.quantity}</Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Transaction:
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">{storesBudget?.transactionCode}</Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Req Number / Line:
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">
                    <Link to={reqHref}>{storesBudget?.requisitionNumber}</Link> /{' '}
                    {storesBudget?.lineNumber}
                </Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    By:
                </Typography>
            </Grid>
            <Grid size={6}>
                <Stack direction="row" spacing={2}>
                    <Typography variant="body1">{storesBudget?.bookedById}</Typography>
                    <Typography variant="body1">{storesBudget?.bookedByName}</Typography>
                </Stack>
            </Grid>
            <Grid size={4} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Order Number/Line:
                </Typography>
            </Grid>
            <Grid size={3}>
                {storesBudget?.orderNumber && (
                    <Stack direction="row" spacing={2}>
                        <Typography variant="body1">{storesBudget?.orderNumber}</Typography>
                        <Typography variant="body1">/</Typography>
                        <Typography variant="body1">{storesBudget?.lineNumber}</Typography>
                    </Stack>
                )}
            </Grid>
            <Grid size={7} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Budget Price
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">
                    {numberFormat.format(storesBudget?.budgetPartPrice)}
                </Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Material Price
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">
                    {numberFormat.format(storesBudget?.materialPrice)}
                </Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Exchange Rate:
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">
                    {numberFormat.format(storesBudget?.spotRate)}
                </Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.pullRight} variant="body1">
                    Reference:
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">{storesBudget?.reference}</Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={12}>
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
            </Grid>
        </Grid>
    );
}

StoresBudget.propTypes = {
    storesBudget: PropTypes.shape({
        budgetId: PropTypes.number,
        dateBooked: PropTypes.string,
        transactionCode: PropTypes.string,
        bookedById: PropTypes.number,
        bookedByName: PropTypes.string,
        partNumber: PropTypes.string,
        storesBudgetPostings: PropTypes.arrayOf(PropTypes.shape({})),
        quantity: PropTypes.number,
        spotRate: PropTypes.number,
        requisitionNumber: PropTypes.number,
        lineNumber: PropTypes.number,
        materialPrice: PropTypes.number,
        budgetPartPrice: PropTypes.number,
        orderNumber: PropTypes.number,
        reference: PropTypes.string,
        part: PropTypes.shape({ description: PropTypes.string }),
        requisition: PropTypes.shape({})
    }).isRequired
};

export default StoresBudget;
