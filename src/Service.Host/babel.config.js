module.exports = {
    presets: [
        ['@babel/preset-env', { modules: false, useBuiltIns: 'usage', corejs: 3 }],
        ['@babel/preset-react']
    ],
    plugins: [
        '@babel/plugin-proposal-class-properties',
        ['@babel/plugin-transform-runtime', { corejs: 3 }]
    ],
    env: {
        test: {
            presets: [
                ['@babel/preset-env', { modules: 'auto', useBuiltIns: 'usage', corejs: 3 }],
                ['@babel/preset-react']
            ],
            plugins: ['@babel/plugin-transform-runtime']
        }
    }
};
