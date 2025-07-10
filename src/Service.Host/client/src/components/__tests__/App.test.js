import React from 'react';
import { InputField } from '@linn-it/linn-form-components-library';
import '@testing-library/jest-dom/vitest';
import { render } from '@testing-library/react';
import { describe, test, expect } from 'vitest';
import { TextField } from '@mui/material';

describe('App tests', () => {
    test('App renders without crashing...', () => {
        const { getByLabelText } = render(
            <InputField value="test" label="test" propertyName="test" onChange={() => {}} />
        );
        expect(getByLabelText('test')).toBeInTheDocument();
    });
});
