import path from 'path';
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

const isProd = process.env.BUILD_ENV === 'production';

// simple CSS mock plugin for Vitest to avoid CSS import errors
function mockCssPlugin() {
    return {
        name: 'mock-css',
        enforce: 'pre',
        transform(code, id) {
            if (id.endsWith('.css')) {
                return {
                    code: 'export default {}',
                    map: null
                };
            }
        }
    };
}

export default defineConfig({
    base: '/stores2/',
    plugins: [
        react({
            jsxImportSource: 'react',
            babel: {
                plugins: [['@babel/plugin-transform-react-jsx', { runtime: 'automatic' }]]
            },
            include: [/\.js$/, /\.jsx$/]
        }),
        mockCssPlugin()
    ],
    resolve: {
        alias: {
            '@mui/x-date-pickers': path.resolve(__dirname, 'node_modules/@mui/x-date-pickers'),
            react: path.resolve(__dirname, 'node_modules/react'),
            'react-dom': path.resolve(__dirname, 'node_modules/react-dom'),
            'react-router-dom': path.resolve(__dirname, 'node_modules/react-router-dom'),
            notistack: path.resolve(__dirname, 'node_modules/notistack'),
            '@material-ui/styles': path.resolve(__dirname, 'node_modules/@material-ui/styles')
        }
    },
    test: {
        globals: true,
        environment: 'jsdom',
        setupFiles: './vitest.setup.js',
        pool: 'vmThreads'
    },
    build: {
        outDir: isProd ? 'app/client/build' : 'client/build',
        emptyOutDir: true,
        sourcemap: !isProd,
        minify: isProd ? 'esbuild' : false,
        rollupOptions: {
            input: 'index.html',
            output: {
                entryFileNames: 'app.js',
                assetFileNames: '[name].[ext]'
            }
        }
    },
    define: {
        'PROCESS.ENV.appRoot': JSON.stringify('http://localhost:5050')
    },
    server: {
        port: 3000,
        open: true
    }
});
