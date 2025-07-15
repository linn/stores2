import React from 'react';
import { NotFound } from '@linn-it/linn-form-components-library';
import config from '../config';
import Page from './Page';

export default function NotFoundPage() {
    return (
        <Page homeUrl={config.appRoot}>
            <NotFound />
        </Page>
    );
}
