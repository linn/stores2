const path = require('path');
const webpack = require('webpack');

module.exports = {
    mode: 'development',
    entry: {
        app: [
            'webpack-dev-server/client?http://localhost:3000', // bundle the client for webpack-dev-server and connect to the provided endpoint
            './client/src/index.js' // the entry point of our app
        ]
    },
    output: {
        path: path.join(__dirname, '../client/build'),
        filename: '[name].js',
        publicPath: '/stores2/build/'
    },
    module: {
        rules: [
            {
                test: /.js$/,
                use: {
                    loader: 'babel-loader'
                },
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            importLoaders: 1
                        }
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
                        options: {
                            importLoaders: 1
                        }
                    },
                    'fast-sass-loader',
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
    optimization: {
        moduleIds: 'named'
    },
    devtool: 'eval-cheap-module-source-map',
    // From https://github.com/gaearon/react-hot-boilerplate/blob/next/webpack.config.js
    plugins: [
        new webpack.NoEmitOnErrorsPlugin(), // do not emit compiled assets that include errors
        new webpack.DefinePlugin({
            'PROCESS.ENV': {
                appRoot: JSON.stringify('http://localhost:5050')
            }
        })
    ]
};
