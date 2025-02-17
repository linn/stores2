module.exports = {
    presets: [
        ['@babel/preset-env', { modules: 'commonjs' }],
        [
            '@babel/preset-react'
            // {
            //     runtime: 'automatic' // React 17+ automatic JSX transform
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
