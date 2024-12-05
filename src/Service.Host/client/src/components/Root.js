import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import App from './App';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';
import useSignIn from '../hooks/useSignIn';
import Navigation from '../containers/Navigation';
import TestPage from './TestPage';

function Root() {
    useSignIn();

    return (
        <div>
            <div>
                <Navigation />
                <Routes>
                    <Route path="/" element={<Navigate to="/template" replace />} />
                    <Route path="/template" element={<App />} />
                    <Route path="/test" element={<TestPage />} />
                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </div>
        </div>
    );
}

export default Root;
