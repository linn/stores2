import React from 'react';
import { utilities } from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import Grid from '@mui/material/Grid2';
import moment from 'moment';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import BudgetPostings from './BudgetPostings';

function StoresBudget({ storesBudget }) {
    const useStyles = makeStyles(() => ({
        marginBelow: {
            marginBottom: '10px'
        },
        displayLabel: {
            float: 'right',
            fontWeight: 'bold'
        }
    }));
    const classes = useStyles();

    const currencyFormat = new Intl.NumberFormat('en-GB', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    const partPriceFormat = new Intl.NumberFormat('en-GB', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 5
    });
    const reqHref = utilities.getSelfHref(storesBudget?.requisitionLine?.requisitionHeader);

    return (
        <Grid container spacing={1}>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Budget Id:
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">{storesBudget?.budgetId}</Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Date Booked:
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">
                    {moment(storesBudget?.dateBooked).format('DD-MMM-YYYY')}
                </Typography>
            </Grid>
            <Grid size={2} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Part Number:
                </Typography>
            </Grid>
            <Grid size={10}>
                <Typography variant="body1">
                    {storesBudget?.partNumber} - {storesBudget?.part?.description}
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Quantity
                </Typography>
            </Grid>
            <Grid size={4}>
                <Typography variant="body1">{storesBudget?.quantity}</Typography>
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Transaction:
                </Typography>
            </Grid>
            <Grid size={10}>
                <Typography variant="body1">
                    {storesBudget?.transactionCode} - {storesBudget?.transactionCodeDescription}
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
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
                <Typography className={classes.displayLabel} variant="body1">
                    Created By:
                </Typography>
            </Grid>
            <Grid size={6}>
                <Stack direction="row" spacing={2}>
                    <Typography variant="body1">
                        {storesBudget?.requisitionLine?.requisitionHeader?.createdBy}
                    </Typography>
                    <Typography variant="body1">
                        {storesBudget?.requisitionLine?.requisitionHeader?.createdByName}
                    </Typography>
                </Stack>
            </Grid>
            <Grid size={4} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Document Number/Line:
                </Typography>
            </Grid>
            <Grid size={4}>
                {storesBudget.requisitionLine?.document1Number && (
                    <Stack direction="row" spacing={2}>
                        <Typography variant="body1">
                            {storesBudget.requisitionLine.document1Number}
                        </Typography>
                        <Typography variant="body1">/</Typography>
                        <Typography variant="body1">
                            {storesBudget.requisitionLine.document1Line}
                        </Typography>
                        {'  '}
                        <Typography variant="body1">
                            {storesBudget.requisitionLine.document1Type}
                        </Typography>
                    </Stack>
                )}
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Document 2 Number/Line:
                </Typography>
            </Grid>
            <Grid size={4}>
                {storesBudget.requisitionLine?.document2Number && (
                    <Stack direction="row" spacing={2}>
                        <Typography variant="body1">
                            {storesBudget.requisitionLine.document2Number}
                        </Typography>
                        <Typography variant="body1">/</Typography>
                        <Typography variant="body1">
                            {storesBudget.requisitionLine.document2Line}
                        </Typography>
                        {'  '}
                        <Typography variant="body1">
                            {storesBudget.requisitionLine.document2Type}
                        </Typography>
                    </Stack>
                )}
            </Grid>
            <Grid size={6} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Currency:
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">{storesBudget?.currencyCode}</Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Exchange Rate:
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">
                    {currencyFormat.format(storesBudget?.spotRate)}
                </Typography>
            </Grid>
            <Grid size={4} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Reference:
                </Typography>
            </Grid>
            <Grid size={6}>
                <Typography variant="body1">{storesBudget?.reference}</Typography>
            </Grid>
            <Grid size={4} />
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Base Unit Price:
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">
                    {partPriceFormat.format(storesBudget?.partPrice)}
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Material Price
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">
                    {partPriceFormat.format(storesBudget?.materialPrice)}
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography className={classes.displayLabel} variant="body1">
                    Labour Price
                </Typography>
            </Grid>
            <Grid size={2}>
                <Typography variant="body1">
                    {partPriceFormat.format(storesBudget?.labourPrice)}
                </Typography>
            </Grid>
            <Grid size={10}>
                <BudgetPostings budgetPostings={storesBudget?.storesBudgetPostings} />
            </Grid>
            <Grid size={2} />
        </Grid>
    );
}

StoresBudget.propTypes = {
    storesBudget: PropTypes.shape({
        budgetId: PropTypes.number,
        dateBooked: PropTypes.string,
        transactionCode: PropTypes.string,
        transactionCodeDescription: PropTypes.string,
        partNumber: PropTypes.string,
        currencyCode: PropTypes.string,
        storesBudgetPostings: PropTypes.arrayOf(PropTypes.shape({})),
        quantity: PropTypes.number,
        spotRate: PropTypes.number,
        requisitionNumber: PropTypes.number,
        lineNumber: PropTypes.number,
        materialPrice: PropTypes.number,
        labourPrice: PropTypes.number,
        partPrice: PropTypes.number,
        reference: PropTypes.string,
        part: PropTypes.shape({ description: PropTypes.string }),
        requisitionLine: PropTypes.shape({
            document1Number: PropTypes.number,
            document1Line: PropTypes.number,
            document1Type: PropTypes.string,
            document2Number: PropTypes.number,
            document2Line: PropTypes.number,
            document2Type: PropTypes.string,
            requisitionHeader: PropTypes.shape({
                createdBy: PropTypes.string,
                createdByName: PropTypes.string
            })
        })
    }).isRequired
};

export default StoresBudget;
