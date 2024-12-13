import config from './config';

const itemTypes = {
    health: { url: `${config.appRoot}/healthcheck` },
    countries: { url: `${config.appRoot}/stores2/countries` },
    carriers: { url: `${config.appRoot}/stores2/carriers` }
};

export default itemTypes;
