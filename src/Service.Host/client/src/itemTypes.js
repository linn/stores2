import config from './config';

const itemTypes = {
    health: { url: `${config.appRoot}/healthcheck` },
    countries: { url: `${config.appRoot}/stores2/countries` },
    carriers: { url: `${config.appRoot}/stores2/carriers` },
    currentEmployees: { url: `${config.proxyRoot}/employees?currentEmployees=true` },
    historicEmployees: { url: `${config.proxyRoot}/employees` },
    storagePlaceAudit: { url: `${config.appRoot}/stores2/reports/storage-place-audit/report` },
    storagePlaces: { url: `${config.proxyRoot}/inventory/storage-places` },
    auditLocations: { url: `${config.proxyRoot}/inventory/audit-locations` },
    requisitions: { url: `${config.appRoot}/requisitions` },
    functionCodes: { url: `${config.appRoot}/requisitions/stores-functions` },
    goodsInLog: { url: `${config.appRoot}/stores2/goods-in-log/report` },
    createAuditReqs: { url: `${config.appRoot}/stores2/storage-places/create-checked-audit-reqs` },
    storesBudget: { url: `${config.appRoot}/stores2/budgets` },
    storageSites: { url: `${config.appRoot}/stores2/storage/sites` },
    storageTypes: { url: `${config.appRoot}/stores2/storage-types` },
    pcasStorageTypes: { url: `${config.appRoot}/stores2/pcas-storage-types` },
    storageLocations: { url: `${config.appRoot}/stores2/storage/locations` },
    nominals: { url: `${config.proxyRoot}/ledgers/nominals` },
    departments: { url: `${config.proxyRoot}/ledgers/departments` },
    nominalAccounts: { url: `${config.proxyRoot}/ledgers/nominal-accounts` },
    parts: { url: `${config.proxyRoot}/parts` },
    stockLocators: { url: `${config.proxyRoot}/inventory/stock-locators-by-location` },
    stockPools: { url: `${config.appRoot}/stores2/stock-pools` },
    stockStates: { url: `${config.appRoot}/stores2/stock/states` },
    accountingCompany: { url: `${config.proxyRoot}/inventory/accounting-companies` },
    partsStorageTypes: { url: `${config.appRoot}/stores2/parts-storage-types` },
    storesTransViewer: { url: `${config.appRoot}/stores2/stores-trans-viewer/report` },
    purchaseOrder: { url: `${config.proxyRoot}/purchasing/purchase-orders` },
    creditNotes: { url: `${config.proxyRoot}/sales/credit-notes` },
    worksOrder: { url: `${config.proxyRoot}/production/works-orders` },
    debitNote: { url: `${config.proxyRoot}/purchasing/pl-credit-debit-notes` }
};

export default itemTypes;
