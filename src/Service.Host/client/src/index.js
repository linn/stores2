import React from 'react';
import { createRoot } from 'react-dom/client';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider, StyledEngineProvider, createTheme } from '@mui/material/styles';
import { AuthProvider } from 'react-oidc-context';
import CssBaseline from '@mui/material/CssBaseline';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { BrowserRouter } from 'react-router-dom';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import Root from './components/Root';
import 'typeface-roboto';
import { oidcConfig } from './helpers/authUtils';

const container = document.getElementById('root');
const root = createRoot(container);

const theme = createTheme({});

const render = Component => {
    root.render(
        <BrowserRouter>
            <AuthProvider {...oidcConfig}>
                <CssBaseline />
                <StyledEngineProvider injectFirst>
                    <ThemeProvider theme={theme}>
                        <SnackbarProvider dense maxSnack={5}>
                            <LocalizationProvider dateAdapter={AdapterMoment} locale="en-GB">
                                <Component />
                            </LocalizationProvider>
                        </SnackbarProvider>
                    </ThemeProvider>
                </StyledEngineProvider>
            </AuthProvider>
        </BrowserRouter>
    );
};

document.body.style.margin = '0';

render(Root);
