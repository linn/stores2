import config from './config';

const itemTypes = {
    health: { url: `${config.appRoot}/healthcheck` },
    countries: { url: `${config.appRoot}/stores2/countries` },
    carriers: { url: `${config.appRoot}/stores2/carriers` },
    storagePlaceAudit: { url: `${config.appRoot}/stores2/reports/storage-place-audit/report` },
    storagePlaces: { url: `${config.proxyRoot}/inventory/storage-places` },
    auditLocations: { url: `${config.proxyRoot}/inventory/audit-locations` },
    createAuditReqs: { url: `${config.proxyRoot}/inventory/storage-places/create-audit-reqs` },
    stockPools: { url: `${config.appRoot}/stores2/stock-pools` },
    accountingCompany: { url: `${config.proxyRoot}/inventory/accounting-companies` }
};

export default itemTypes;
