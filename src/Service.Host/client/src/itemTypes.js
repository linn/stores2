﻿import config from './config';

const itemTypes = {
    health: { url: `${config.appRoot}/healthcheck` },
    countries: { url: `${config.appRoot}/stores2/countries` },
    carriers: { url: `${config.appRoot}/stores2/carriers` },
    storagePlaceAudit: { url: `${config.appRoot}/stores2/reports/storage-place-audit/report` },
    storagePlaces: { url: `${config.proxyRoot}/inventory/storage-places` },
    auditLocations: { url: `${config.proxyRoot}/inventory/audit-locations` },
    requisitions: { url: `${config.appRoot}/requisitions` },
    functionCodes: { url: `${config.appRoot}/requisitions/function-codes` },
    createAuditReqs: { url: `${config.appRoot}/stores2/storage-places/create-checked-audit-reqs` },
    storesBudget: { url: `${config.appRoot}/stores2/budgets` },
    storageSites: { url: `${config.appRoot}/stores2/storage/sites` },
    storageTypes: { url: `${config.appRoot}/stores2/storage-types` },
    storageLocations: { url: `${config.appRoot}/stores2/storage/locations` },
    nominals: { url: `${config.proxyRoot}/ledgers/nominals` },
    departments: { url: `${config.proxyRoot}/ledgers/departments` },
    parts: { url: `${config.proxyRoot}/parts` },
    stockLocators: { url: `${config.proxyRoot}/inventory/stock-locators-by-location` },
    stockPools: { url: `${config.appRoot}/stores2/stock-pools` },
    accountingCompany: { url: `${config.proxyRoot}/inventory/accounting-companies` }
};

export default itemTypes;
