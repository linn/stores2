import path from 'path';
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// export default defineConfig({
//   base: '/stores2/build/',
//   build: {
//     outDir: 'client/build',  // ✅ Matches dev StaticFileOptions
//     emptyOutDir: true,
//     rollupOptions: {
//       input: 'client/src/index.html',
//       output: {
//         entryFileNames: 'app.js', // or hashed name if you're using manifest lookup
//         assetFileNames: '[name].[ext]'
//       }
//     }
//   }
// });

const isProd = process.env.BUILD_ENV === 'production';

export default defineConfig({
    base: '/',
    plugins: [
        react({
            jsxImportSource: 'react',
            babel: {
                plugins: [['@babel/plugin-transform-react-jsx', { runtime: 'automatic' }]]
            },
            include: [/\.js$/, /\.jsx$/] // 👈 Add .js here explicitly
        })
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
    build: {
        outDir: isProd ? 'app/client/build' : 'client/build',
        emptyOutDir: true,

        sourcemap: !isProd, // Enable source maps only in non-prod
        minify: isProd ? 'esbuild' : false // Minify only in prod
    },
    define: {
        'PROCESS.ENV.appRoot': JSON.stringify('http://localhost:5050')
    },
    server: {
        port: 3000,
        open: true
    }
});
