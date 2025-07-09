/**
 * @jest-environment jsdom
 */
// const formComponents = require('@linn-it/linn-form-components-library');
// console.log(formComponents);
import React from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import '@testing-library/jest-dom';
import { render } from '@testing-library/react';
import { TextField } from '@mui/material';

console.log('TextField IN TEST typeof:', typeof TextField);
console.log('InputField IN TEST typeof:', typeof InputField);

describe('App tests', () => {
    test('App renders without crashing...', () => {
        const { getByLabelText } = render(
            <InputField value="test" label="test" propertyName="test" onChange={() => {}} />
        );
        expect(getByLabelText('test')).toBeInTheDocument();
    });
});
