import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

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

export default defineConfig(({ mode }) => {
    const isProd = mode === 'production';

    return {
        base: '/',
        plugins: [
            react({
                jsxImportSource: 'react',
                babel: {
                    plugins: [['@babel/plugin-transform-react-jsx', { runtime: 'automatic' }]]
                },
                include: [/\.[jt]sx?$/] // handles both .js/.jsx and .ts/.tsx
            }),

            mockCssPlugin()
        ],
        esbuild: {
            loader: { '.js': 'jsx' },
            include: /src\/.*\.js$/
        },
        optimizeDeps: {
            esbuildOptions: {
                loader: {
                    '.js': 'jsx'
                }
            }
        },
        test: {
            globals: true,
            environment: 'jsdom',
            setupFiles: './vitest.setup.js',
            transformMode: {
                web: [/\.[jt]sx?$/]
            },
            pool: 'vmThreads'
        },
        build: {
            outDir: 'client/build',
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
    };
});
