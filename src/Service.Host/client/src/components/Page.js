import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import Avatar from '@mui/material/Avatar';
import { green, red } from '@mui/material/colors';
import { makeStyles } from '@mui/styles';
import { useSnackbar } from 'notistack';
import { useAuth } from 'react-oidc-context';
import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { Breadcrumbs, Loading } from '@linn-it/linn-form-components-library';
import Tooltip from '@mui/material/Tooltip';
import Typography from '@mui/material/Typography';
import { useLocation, useNavigate } from 'react-router-dom';

const useStyles = makeStyles(theme => ({
    root: {
        padding: theme.spacing(4)
    },
    breadcrumbs: {
        // marginTop: theme.spacing(4),
        marginLeft: theme.spacing(4),
        marginRight: theme.spacing(4),
        padding: theme.spacing(2)
    },
    grid: {
        // marginTop: theme.spacing(4),
        width: '100%'
    }
}));

const pageWidth = {
    xs: 4,
    s: 6,
    m: 8,
    l: 10,
    xl: 12
};

const columnWidth = {
    xs: 4,
    s: 3,
    m: 2,
    l: 1,
    xl: false
};

function Page({
    children,
    width,
    requestErrors,
    showRequestErrors,
    homeUrl,
    showBreadcrumbs,
    showAuthUi
}) {
    const classes = useStyles();
    const navigate = useNavigate();
    const location = useLocation();
    const { enqueueSnackbar } = useSnackbar();
    useEffect(() => {
        if (requestErrors && showRequestErrors) {
            requestErrors.forEach(t => {
                enqueueSnackbar(`${t.message} - ${t.type}`, {
                    variant: 'error',
                    preventDuplicate: true
                });
            });
        }
    }, [requestErrors, enqueueSnackbar, showRequestErrors]);

    const auth = useAuth();

    const authUi = () => {
        if (auth.activeNavigator === 'signinSilent') {
            return <Typography variant="subtitle1">Signing in...</Typography>;
        }
        if (auth.activeNavigator === 'signoutRedirect') {
            return <Typography variant="subtitle1">Signing out...</Typography>;
        }

        if (auth.isLoading) {
            return <Loading />;
        }

        if (!auth.isAuthenticated) {
            return (
                <Tooltip title="Unable to log in">
                    <Avatar sx={{ bgcolor: red[500] }}>!</Avatar>
                </Tooltip>
            );
        }

        const initials = auth?.user?.profile?.name
            ?.split(/\s/)
            .reduce((response, word) => response + word.slice(0, 1), '');

        return (
            <Tooltip title={`you are logged in as ${auth?.user?.profile?.preferred_username}`}>
                <Avatar sx={{ bgcolor: green[500] }}>{initials}</Avatar>
            </Tooltip>
        );
    };
    return (
        <Grid container spacing={3} className={classes.grid}>
            <Grid item xs={1} />
            <Grid item xs={10} className="hide-when-printing">
                {showBreadcrumbs && (
                    <div style={{ marginTop: '80px' }}>
                        <Breadcrumbs navigate={navigate} location={location} homeUrl={homeUrl} />
                    </div>
                )}
            </Grid>
            <Grid item xs={1} />

            <Grid item xs={columnWidth[width]} />
            <Grid item xs={pageWidth[width]}>
                <Paper className={classes.root} square>
                    <>
                        {showAuthUi && <div style={{ float: 'right' }}>{authUi()}</div>}
                        {children}
                    </>
                </Paper>
            </Grid>
            <Grid item xs={columnWidth[width]} />
        </Grid>
    );
}

Page.propTypes = {
    children: PropTypes.node.isRequired,
    width: PropTypes.oneOf(['xs', 's', 'm', 'l', 'xl']),
    showRequestErrors: PropTypes.bool,
    requestErrors: PropTypes.arrayOf(PropTypes.shape({})),
    homeUrl: PropTypes.string,
    showBreadcrumbs: PropTypes.bool,
    showAuthUi: PropTypes.bool
};

Page.defaultProps = {
    width: 'l',
    showRequestErrors: false,
    requestErrors: [],
    homeUrl: null,
    showBreadcrumbs: true,
    showAuthUi: true
};

export default Page;
