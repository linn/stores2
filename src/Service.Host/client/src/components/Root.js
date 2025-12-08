import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import 'typeface-roboto';
import useSignIn from '../hooks/useSignIn';
import Navigation from '../containers/Navigation';
import NotFoundPage from './NotFoundPage';
import App from './App';
import Carriers from './Carriers';
import Carrier from './Carrier';
import GoodsInLog from './GoodsInLog';
import StoragePlaceAudit from './StoragePlaceAudit';
import SearchRequisitions from './requisitions/SearchRequisitions';
import PendingRequisitions from './requisitions/PendingRequisitions';
import Requisition from './requisitions/Requisition';
import StoresBudgetViewer from './StoresBudgetViewer';
import StorageLocations from './StorageLocations';
import StorageLocation from './StorageLocation';
import StockPools from './StockPools';
import StorageTypes from './StorageTypes';
import PartStorageTypes from './PartStorageTypes';
import PartStorageType from './PartStorageType';
import StoresTransViewer from './StoresTransViewer';
import StoresFunctions from './StoresFunctions';
import StoresFunction from './StoresFunction';
import QcLabelPrintScreen from './requisitions/containers/QcLabelPrinter';
import PcasStorageTypes from './PcasStorageTypes';
import PcasStorageType from './PcasStorageType';
import RequisitionCostReport from './RequisitionCostReport';
import Workstations from './Workstations';
import Workstation from './Workstation';
import Pallets from './Pallets';
import Pallet from './Pallet';
import StorageSites from './storageSites/StorageSites';
import StorageSite from './storageSites/StorageSite';
import DailyEuDispatchReport from './DailyEuDespatchReport';
import DailyEuImportRsnReport from './DailyEuImportRsnReport';
import LabourHoursInStockReport from './LabourHoursInStockReport';
import LabourHoursSummaryReport from './LabourHoursSummaryReport';
import LabourHoursInLoansReport from './LabourHoursInLoansReport';

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
                    <Route
                        path="/requisitions/stores-functions/view"
                        element={<StoresFunctions />}
                    />
                    <Route
                        path="/requisitions/stores-functions/:code"
                        element={<StoresFunction />}
                    />
                    <Route path="/requisitions" element={<SearchRequisitions />} />
                    <Route path="/requisitions/print-qc-labels" element={<QcLabelPrintScreen />} />

                    <Route path="/requisitions/pending" element={<PendingRequisitions />} />
                    <Route path="/requisitions/create" element={<Requisition creating />} />

                    <Route
                        path="/requisitions/:reqNumber"
                        element={<Requisition creating={false} />}
                    />

                    <Route path="/stores2/budgets" element={<StoresBudgetViewer />} />
                    <Route path="/stores2/budgets/:id" element={<StoresBudgetViewer />} />
                    <Route path="/stores2/storage" element={<StorageLocations />} />
                    <Route
                        path="/stores2/storage/locations"
                        element={<Navigate to="/stores2/storage" replace />}
                    />

                    <Route path="/stores2/storage-types" element={<StorageTypes />} />
                    <Route path="/stores2/parts-storage-types" element={<PartStorageTypes />} />
                    <Route path="/stores2/parts-storage-types/:id" element={<PartStorageType />} />
                    <Route
                        path="/stores2/parts-storage-types/create"
                        element={<PartStorageType creating />}
                    />
                    <Route path="/stores2/pcas-storage-types" element={<PcasStorageTypes />} />
                    <Route
                        path="/stores2/pcas-storage-types/create"
                        element={<PcasStorageType creating />}
                    />
                    <Route
                        path="/stores2/pcas-storage-types/:boardCode/:storageTypeCode"
                        element={<PcasStorageType />}
                    />
                    <Route path="/stores2/storage/sites" element={<StorageSites />} />
                    <Route
                        path="/stores2/storage/sites/create"
                        element={<StorageSite creating />}
                    />
                    <Route path="/service/storage-sites/:code" element={<StorageSite />} />
                    <Route
                        path="/stores2/storage/locations/create"
                        element={<StorageLocation creating />}
                    />
                    <Route
                        path="/stores2/despatch/exbook/despatch/report"
                        element={<DailyEuDispatchReport />}
                    />
                    <Route
                        path="/stores2/customs/daily/eu/import/rsn/report"
                        element={<DailyEuImportRsnReport />}
                    />
                    <Route path="/stores2/pallets" element={<Pallets />} />
                    <Route path="/stores2/pallets/:id" element={<Pallet />} />
                    <Route path="/stores2/pallets/create" element={<Pallet creating />} />
                    <Route path="/stores2/storage/locations/:id" element={<StorageLocation />} />
                    <Route path="/stores2/stock-pools" element={<StockPools />} />
                    <Route path="/stores2/stores-trans-viewer" element={<StoresTransViewer />} />
                    <Route
                        path="/requisitions/reports/requisition-cost"
                        element={<RequisitionCostReport />}
                    />
                    <Route
                        path="/requisitions/reports/requisition-cost/:reqNumber"
                        element={<RequisitionCostReport />}
                    />
                    <Route path="/stores2/work-stations" element={<Workstations />} />
                    <Route path="/stores2/work-stations/detail" element={<Workstation />} />
                    <Route
                        path="/stores2/work-stations/create"
                        element={<Workstation creating />}
                    />
                    <Route
                        path="/stores2/reports/labour-hours-in-stock"
                        element={<LabourHoursInStockReport />}
                    />
                    <Route
                        path="/stores2/reports/labour-hours-in-loans"
                        element={<LabourHoursInLoansReport />}
                    />
                    <Route
                        path="/stores2/reports/labour-hours-summary"
                        element={<LabourHoursSummaryReport />}
                    />
                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </div>
        </div>
    );
}

export default Root;
