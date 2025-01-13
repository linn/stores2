import config from './config';

const itemTypes = {
    health: { url: `${config.appRoot}/healthcheck` },
    countries: { url: `${config.appRoot}/stores2/countries` },
    carriers: { url: `${config.appRoot}/stores2/carriers` },
    storagePlaceAudit: { url: `${config.appRoot}/stores2/reports/storage-place-audit/report` }
};

export default itemTypes;
