/**
 * @jest-environment jsdom
 */
import React from 'react';
import '@testing-library/jest-dom/extend-expect';
import render from '../../test-utils';
import App from '../App';

describe('App tests', () => {
    test('App renders without crashing...', () => {
        const { getByText } = render(<App />);
        expect(getByText('Template')).toBeInTheDocument();
    });
});
