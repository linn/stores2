import React from 'react';
import { createRoot } from 'react-dom/client';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider, StyledEngineProvider, createTheme } from '@mui/material/styles';
import { AuthProvider } from 'react-oidc-context';
import { WebStorageStateStore } from 'oidc-client-ts';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { BrowserRouter } from 'react-router-dom';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import Root from './components/Root';
import 'typeface-roboto';
import config from './config';

const container = document.getElementById('root');
const root = createRoot(container);

const host = window.location.origin;

const oidcConfig = {
    authority: config.authorityUri,
    client_id: 'app2',
    response_type: 'code',
    scope: 'openid profile email associations',
    redirect_uri: `${host}/stores2`,
    post_logout_redirect_uri: `${config.proxyRoot}/authentication/Account/Logout`,
    onSigninCallback: () => {
        const redirect = sessionStorage.getItem('auth:redirect');
        if (redirect) {
            window.location.href = redirect;
            sessionStorage.removeItem('auth:redirect');
        } else {
            window.location.href = `${host}/stores2`;
        }
    },
    userStore: new WebStorageStateStore({ store: window.localStorage })
};
const theme = createTheme({
    // cssVariables: {
    //     colorSchemeSelector: 'class'
    // },
    // colorSchemes: {
    //     light: true,
    //     dark: true
    // }
});
const render = Component => {
    root.render(
        <BrowserRouter>
            <AuthProvider {...oidcConfig}>
                {/* <StyledEngineProvider injectFirst> */}
                <ThemeProvider theme={theme}>
                    <SnackbarProvider dense maxSnack={5}>
                        <LocalizationProvider dateAdapter={AdapterMoment} locale="en-GB">
                            <Component />
                        </LocalizationProvider>
                    </SnackbarProvider>
                </ThemeProvider>
                {/* </StyledEngineProvider> */}
            </AuthProvider>
        </BrowserRouter>
    );
};

document.body.style.margin = '0';

render(Root);
