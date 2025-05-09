import React from 'react';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import UI from '../components/QcLabelPrinter';
import config from '../../../config';
import Page from '../../Page';

function QcLabelPrintScreen() {
    const location = useLocation();
    const query = queryString.parse(location.search);
    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Qc Labels">
            <UI
                docType={query.docType}
                orderNumber={query.orderNumber}
                qcState={query.qcState}
                partNumber={query.partNumber}
                partDescription={query.partDescription}
                qtyReceived={query.qtyReceived}
                unitOfMeasure={query.unitOfMeasure}
                reqNumber={query.reqNumber}
                qcInfo={query.qcInfo}
                kardexLocation={query.kardexLocation}
                initialNumContainers={query.initialNumContainers ?? 1}
            />
        </Page>
    );
}

export default QcLabelPrintScreen;
