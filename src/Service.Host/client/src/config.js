const config = window.APPLICATION_SETTINGS;
const defaultConfig = { appRoot: 'localhost:5053', proxyRoot: 'localhost:5053' };

export default { ...defaultConfig, ...config };
