module.exports = {
    presets: [
        ['@babel/preset-env', { modules: 'commonjs' }],
        [
            '@babel/preset-react'
            // React 17+ automatic JSX transform - removes necessity to import React in jsx files
            // {
            //     runtime: 'automatic' - this doesn't work properly for some reason, so omitting
            // }
        ]
    ],
    plugins: ['@babel/plugin-proposal-class-properties'],
    env: {
        test: {
            plugins: ['@babel/plugin-transform-runtime'] // For test environment
        }
    }
};
