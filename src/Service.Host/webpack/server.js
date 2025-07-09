const webpack = require('webpack');
const path = require('path');

const Server = require('webpack-dev-server');

const config = require('./webpack.config');

const devServer = new Server(
    {
        static: {
            directory: path.join(__dirname, '../')
        },
        client: {
            overlay: false,
            webSocketURL: {
                hostname: '127.0.0.1',
                port: 3000,
                pathname: '/ws',
                protocol: 'ws'
            }
        },
        watchFiles: {
            paths: ['src/components/', '!node_modules', '!dist'],
            options: {
                ignored: ['*', '**/node_modules', '**/dist', '**/.git'],
                usePolling: true,
                interval: 1000
            }
        },
        devMiddleware: {
            index: true,
            mimeTypes: { 'text/html': ['phtml'] },
            serverSideRender: true,
            writeToDisk: true
        },
        host: '127.0.0.1',
        hot: true,
        historyApiFallback: true,
        port: 3000
    },
    webpack(config)
);

(async () => {
    await devServer.start();

    console.log('Running');
})();
