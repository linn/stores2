import React from 'react';
import { NotFound } from '@linn-it/linn-form-components-library';
import Page from './Page';
import config from '../config';

export default function NotFoundPage() {
    return (
        <Page homeUrl={config.appRoot}>
            <NotFound />
        </Page>
    );
}
