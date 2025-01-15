import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import App from './App';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';
import useSignIn from '../hooks/useSignIn';
import Navigation from '../containers/Navigation';
import Carriers from './Carriers';
import Carrier from './Carrier';
import StoragePlaceAudit from './StoragePlaceAudit';

function Root() {
    useSignIn();

    return (
        <div>
            <div>
                <Navigation />
                <Routes>
                    <Route path="/" element={<Navigate to="/stores2" replace />} />
                    <Route path="/stores2" element={<App />} />
                    <Route path="/stores2/carriers" element={<Carriers />} />
                    <Route path="/stores2/carriers/create" element={<Carrier creating />} />
                    <Route path="/stores2/carriers/:code" element={<Carrier />} />
                    <Route
                        path="/stores2/reports/storage-place-audit"
                        element={<StoragePlaceAudit />}
                    />
                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </div>
        </div>
    );
}

export default Root;
