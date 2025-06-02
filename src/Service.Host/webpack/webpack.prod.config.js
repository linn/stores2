const path = require('path');
const MomentLocalesPlugin = require('moment-locales-webpack-plugin');
// const { BundleAnalyzerPlugin } = require('webpack-bundle-analyzer');

module.exports = {
    entry: {
        app: ['./client/src/index.js']
    },
    mode: 'production',
    output: {
        path: path.resolve(__dirname, '../client/build'),
        filename: '[name].js',
        publicPath: '/stores2/build/'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                use: {
                    loader: 'babel-loader'
                }
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    {
                        loader: 'css-loader',
                        options: { importLoaders: 1 }
                    },
                    'postcss-loader'
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    'style-loader',
                    {
                        loader: 'css-loader',
                        options: { importLoaders: 1 }
                    },
                    'sass-loader',
                    'postcss-loader'
                ]
            },
            {
                test: /\.(jpe?g|svg|png|gif|ico|eot|ttf|woff2?)(\?v=\d+\.\d+\.\d+)?$/i,
                type: 'asset/resource'
            }
        ]
    },
    resolve: {
        alias: {
            '@mui/x-date-pickers': path.resolve('./node_modules/@mui/x-date-pickers'),
            react: path.resolve('./node_modules/react'),
            'react-dom': path.resolve(__dirname, '../node_modules/react-dom'),
            'react-router-dom': path.resolve(__dirname, '../node_modules/react-router-dom'),
            notistack: path.resolve('./node_modules/notistack'),
            '@material-ui/styles': path.resolve('./node_modules/@material-ui/styles')
        },
        conditionNames: ['mui-modern', '...']
        //modules: [path.resolve('node_modules'), 'node_modules'].concat(/* ... */)
    },
    plugins: [new MomentLocalesPlugin()] // new BundleAnalyzerPlugin()
};
