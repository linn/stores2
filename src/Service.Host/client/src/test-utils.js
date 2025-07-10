import { render } from '@testing-library/react';
import React from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { vi } from 'vitest';
import { SnackbarProvider } from 'notistack';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { useAuth } from 'react-oidc-context';

vi.mock('react-oidc-context', () => ({
    useAuth: vi.fn(),
    hasAuthParams: vi.fn()
}));

useAuth.mockImplementation(() => ({ signinRedirect: vi.fn() }));

vi.mock('react-router-dom', () => ({
    useNavigate: () => vi.fn(),
    useLocation: () => ({
        pathname: ''
    }),
    useParams: () => vi.fn(),
    Link: () => <div />
}));

function Providers({ children }) {
    return (
        <ThemeProvider theme={createTheme()}>
            <SnackbarProvider dense maxSnack={5}>
                <LocalizationProvider dateAdapter={AdapterMoment}>{children}</LocalizationProvider>
            </SnackbarProvider>
        </ThemeProvider>
    );
}

const customRender = (ui, options) => render(ui, { wrapper: Providers, ...options });

export default customRender;
