import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import App from './App';
import 'typeface-roboto';
// import NotFoundPage from './NotFoundPage';
import useSignIn from '../hooks/useSignIn';
import Navigation from '../containers/Navigation';
import Carriers from './Carriers';
import Carrier from './Carrier';
import GoodsInLog from './GoodsInLog';
import StoragePlaceAudit from './StoragePlaceAudit';
import SearchRequisitions from './requisitions/SearchRequisitions';
import Requisition from './requisitions/Requisition';
import StoresBudgetViewer from './StoresBudgetViewer';
import StorageLocations from './StorageLocations';
import StorageLocation from './StorageLocation';
import StockPools from './StockPools';
import StorageTypes from './StorageTypes';

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
                    <Route path="/stores2/goods-in-log" element={<GoodsInLog />} />
                    <Route
                        path="/stores2/reports/storage-place-audit"
                        element={<StoragePlaceAudit />}
                    />
                    <Route path="/requisitions" element={<SearchRequisitions />} />
                    <Route path="/requisitions/create" element={<Requisition creating />} />

                    <Route
                        path="/requisitions/:reqNumber"
                        element={<Requisition creating={false} />}
                    />

                    <Route path="/stores2/budgets" element={<StoresBudgetViewer />} />
                    <Route path="/stores2/budgets/:id" element={<StoresBudgetViewer />} />
                    <Route path="/stores2/storage" element={<StorageLocations />} />
                    <Route path="/stores2/storage-types" element={<StorageTypes />} />
                    <Route
                        path="/stores2/storage/locations/create"
                        element={<StorageLocation creating />}
                    />
                    <Route path="/stores2/storage/locations/:id" element={<StorageLocation />} />
                    <Route path="/stores2/stock-pools" element={<StockPools />} />
                    {/* <Route path="*" element={<NotFoundPage />} /> */}
                </Routes>
            </div>
        </div>
    );
}

export default Root;
