const config = window.APPLICATION_SETTINGS;
const defaultConfig = { appRoot: 'localhost:5050' };

export default { ...defaultConfig, ...config };
