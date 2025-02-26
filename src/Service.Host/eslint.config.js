const react = require('eslint-plugin-react');
const globals = require('globals');
const eslintPluginPrettierRecommended = require('eslint-plugin-prettier/recommended');
const reactHooks = require('eslint-plugin-react-hooks');
const unusedImports = require('eslint-plugin-unused-imports');
const importPlugin = require('eslint-plugin-import');
const js = require('@eslint/js');

module.exports = [
    js.configs.recommended,
    {
        files: ['**/*.{js,jsx,mjs,cjs,ts,tsx}'],
        plugins: {
            react,
            'react-hooks': reactHooks,
            'unused-imports': unusedImports,
            import: importPlugin
        },
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: 'module',
            parserOptions: {
                ecmaFeatures: {
                    jsx: true
                }
            },
            globals: {
                ...globals.browser,
                ...globals.node,
                myCustomGlobal: 'readonly'
            }
        },
        rules: {
            // ... any rules you want
            'react/jsx-uses-react': 'error',
            'react/jsx-uses-vars': 'error',
            'import/no-unresolved': 'error',

            'react/jsx-no-undef': 'error',
            'react-hooks/rules-of-hooks': 'error',
            'react-hooks/exhaustive-deps': 'warn',
            'no-unused-vars': 'error',
            'no-console': 'warn',
            'import/order': [
                'error',
                {
                    groups: ['builtin', 'external', 'internal', 'parent', 'sibling', 'index']
                }
            ]
        }
    },
    eslintPluginPrettierRecommended
];
