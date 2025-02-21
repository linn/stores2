const react = require('eslint-plugin-react');
const globals = require('globals');
const eslintPluginPrettierRecommended = require('eslint-plugin-prettier/recommended');
const reactHooks = require('eslint-plugin-react-hooks');
const unusedImports = require('eslint-plugin-unused-imports');

module.exports = [
    {
        files: ['**/*.{js,jsx,mjs,cjs,ts,tsx}'],
        plugins: {
            react,
            'react-hooks': reactHooks,
            'unused-imports': unusedImports
        },
        languageOptions: {
            parserOptions: {
                ecmaFeatures: {
                    jsx: true
                }
            },
            globals: {
                ...globals.browser
            }
        },
        rules: {
            // ... any rules you want
            'react/jsx-uses-react': 'error',
            'react/jsx-uses-vars': 'error',
            'react-hooks/rules-of-hooks': 'error',
            'react-hooks/exhaustive-deps': 'warn',
            'no-unused-vars': 'error',
            'arrow-body-style': ['error', 'as-needed']
        }
    },
    eslintPluginPrettierRecommended
];
