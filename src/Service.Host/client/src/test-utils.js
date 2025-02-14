import { render } from '@testing-library/react';
import React from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { MemoryRouter } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { useAuth } from 'react-oidc-context';

jest.mock('react-oidc-context', () => ({
    ...jest.requireActual('react-router-dom'),
    useAuth: jest.fn(),
    hasAuthParams: jest.fn()
}));

useAuth.mockImplementation(() => ({ signinRedirect: jest.fn() }));

function Providers({ children }) {
    global.fetch = jest.fn(() =>
        Promise.resolve({
            json: () => Promise.resolve({})
        })
    );
    return (
        <ThemeProvider theme={createTheme()}>
            <SnackbarProvider dense maxSnack={5}>
                <MemoryRouter>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        {children}
                    </LocalizationProvider>
                </MemoryRouter>
            </SnackbarProvider>
        </ThemeProvider>
    );
}

const customRender = (ui, options) => render(ui, { wrapper: Providers, ...options });

export default customRender;
