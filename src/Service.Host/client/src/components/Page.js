import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import Avatar from '@mui/material/Avatar';
import { green, red } from '@mui/material/colors';
import { useSnackbar } from 'notistack';
import { useAuth } from 'react-oidc-context';
import React, { useEffect } from 'react';
import { Breadcrumbs, Loading } from '@linn-it/linn-form-components-library';
import Tooltip from '@mui/material/Tooltip';
import Typography from '@mui/material/Typography';
import { useLocation, useNavigate } from 'react-router-dom';

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
    width = 'l',
    requestErrors = [],
    showRequestErrors = false,
    homeUrl = null,
    showBreadcrumbs = true,
    showAuthUi = true
}) {
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
        <Grid container spacing={3} sx={{ width: '100%' }}>
            <Grid size={1} />
            <Grid size={10} className="hide-when-printing">
                {showBreadcrumbs && (
                    <div style={{ marginTop: '80px' }}>
                        <Breadcrumbs navigate={navigate} location={location} homeUrl={homeUrl} />
                    </div>
                )}
            </Grid>
            <Grid size={1} />
            <Grid size={columnWidth[width] || false} />
            <Grid size={pageWidth[width]}>
                <Paper
                    square
                    sx={{
                        p: 4
                    }}
                >
                    <>
                        {showAuthUi && <div style={{ float: 'right' }}>{authUi()}</div>}
                        {children}
                    </>
                </Paper>
            </Grid>
            <Grid size={columnWidth[width] || false} />
        </Grid>
    );
}

export default Page;
