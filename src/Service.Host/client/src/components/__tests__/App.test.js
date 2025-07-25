import React from 'react';
import '@testing-library/jest-dom';
import { describe, test, expect } from 'vitest';
import render from '../../test-utils';
import App from '../App';

describe('App tests', () => {
    test('App renders without crashing...', () => {
        const { getByText } = render(<App />);
        expect(getByText('stores2')).toBeInTheDocument();
    });
});
