import config from './config';

const itemTypes = {
    countries: { url: `${config.appRoot}/stores2/countries` },
    carriers: { url: `${config.appRoot}/stores2/carriers` },
    currentEmployees: { url: `${config.proxyRoot}/employees?currentEmployees=true` },
    historicEmployees: { url: `${config.proxyRoot}/employees` },
    storagePlaceAudit: { url: `${config.appRoot}/stores2/reports/storage-place-audit/report` },
    storagePlaces: { url: `${config.proxyRoot}/inventory/storage-places` },
    requisitions: { url: `${config.appRoot}/requisitions` },
    functionCodes: { url: `${config.appRoot}/requisitions/stores-functions` },
    goodsInLog: { url: `${config.appRoot}/stores2/goods-in-log/report` },
    createAuditReqs: { url: `${config.appRoot}/stores2/storage-places/create-checked-audit-reqs` },
    storesBudgets: { url: `${config.appRoot}/stores2/budgets` },
    sundryBookInDetails: { url: `${config.appRoot}/requisitions/sundry-book-ins` },
    auditLocations: { url: `${config.appRoot}/stores2/storage/audit-locations` },
    storageSites: { url: `${config.appRoot}/stores2/storage/sites` },
    storageTypes: { url: `${config.appRoot}/stores2/storage-types` },
    pcasStorageTypes: { url: `${config.appRoot}/stores2/pcas-storage-types` },
    pcasBoards: { url: `${config.appRoot}/stores2/pcas-boards` },
    storageLocations: { url: `${config.appRoot}/stores2/storage/locations` },
    pallets: { url: `${config.appRoot}/stores2/pallets` },
    nominals: { url: `${config.proxyRoot}/ledgers/nominals` },
    departments: { url: `${config.proxyRoot}/ledgers/departments` },
    nominalAccounts: { url: `${config.proxyRoot}/ledgers/nominal-accounts` },
    parts: { url: `${config.proxyRoot}/parts` },
    stockLocators: { url: `${config.proxyRoot}/inventory/stock-locators-by-location` },
    stockPools: { url: `${config.appRoot}/stores2/stock-pools` },
    stockStates: { url: `${config.appRoot}/stores2/stock/states` },
    accountingCompany: { url: `${config.proxyRoot}/inventory/accounting-companies` },
    partStorageTypes: { url: `${config.appRoot}/stores2/part-storage-types` },
    storesTransViewer: { url: `${config.appRoot}/stores2/stores-trans-viewer/report` },
    purchaseOrders: { url: `${config.proxyRoot}/purchasing/purchase-orders` },
    loans: { url: `${config.proxyRoot}/sales/loans` },
    creditNotes: { url: `${config.proxyRoot}/sales/credit-notes` },
    worksOrders: { url: `${config.proxyRoot}/production/works-orders` },
    debitNotes: { url: `${config.proxyRoot}/purchasing/pl-credit-debit-notes` },
    requisitionCostReport: {
        url: `${config.appRoot}/requisitions/reports/requisition-cost/report`
    },
    printQcLabels: { url: `${config.appRoot}/requisitions/print-qc-labels` },
    getDefaultBookInLocation: { url: `${config.appRoot}/requisitions/default-book-in-location` },
    workStations: { url: `${config.appRoot}/stores2/work-stations` },
    workStationsApplicationState: {
        url: `${config.appRoot}/stores2/work-stations/application-state`
    },
    dailyEuDispatchReport: { url: `${config.appRoot}/stores2/reports/daily-eu-dispatch` },
    dailyEuImportRsnReport: { url: `${config.appRoot}/stores2/reports/daily-eu-import-rsn` },
    citCodes: { url: `${config.proxyRoot}/production/maintenance/cits` },
    labourHoursInStockReport: {
        url: `${config.appRoot}/stores2/reports/labour-hours-in-stock/report`
    },
    tqmsJobrefs: { url: `${config.proxyRoot}/inventory/tqms-jobrefs` },
    labourHoursInStockTotal: {
        url: `${config.appRoot}/stores2/reports/labour-hours-in-stock/total`
    },
    labourHoursSummaryReport: {
        url: `${config.appRoot}/stores2/reports/labour-hours-summary/report`
    },
    labourHoursInLoansReport: {
        url: `${config.appRoot}/stores2/reports/labour-hours-in-loans/report`
    }
};

export default itemTypes;
