import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import moment from 'moment';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid2';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import {
    Dropdown,
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import Page from './Page';
import config from '../config';
import itemTypes from '../itemTypes';
import useGet from '../hooks/useGet';
import useInitialise from '../hooks/useInitialise';
import usePut from '../hooks/usePut';
import usePost from '../hooks/usePost';

function StorageLocation({ creating }) {
    const [hasFetched, setHasFetched] = useState(false);
    const location = useLocation();
    const query = queryString.parse(location.search);

    const {  isLoading: sitesLoading, result: sitesResult } = useInitialise(itemTypes.storageSites.url);
    const {  isLoading: accountingCompaniesLoading, result: accountingCompaniesResult } = useInitialise(itemTypes.accountingCompany.url);
    const {  isLoading: stockPoolsLoading, result: stockPoolsResult } = useInitialise(itemTypes.stockPools.url);
    const {  isLoading: storageTypesLoading, result: storageTypesResult } = useInitialise(itemTypes.storageTypes.url);

    const { id } = useParams();
    const {
        send: getLocation,
        isLoading,
        result: locationGetResult
    } = useGet(itemTypes.storageLocations.url);

    const {
        send: updateLocation,
        isLoading: updateLoading,
        errorMessage: updateError,
        putResult: updateResult
    } = usePut(itemTypes.storageLocations.url, true);

    const {
        send: createLocation,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.storageLocations.url, true, true);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getLocation(id);
    }

    const navigate = useNavigate();

    const [formValues, setFormValues] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (locationGetResult && !formValues) {
        setFormValues(locationGetResult);
    }

    if (creating && !formValues) {
        setFormValues({
            description: null,
            siteCode: query?.siteCode ? query?.siteCode : null,
            storageAreaCode: query?.storageAreaCode ? query?.storageAreaCode : null,
            locationCode: query?.prefix ? query?.prefix : null,
            accountingCompany: 'LINN',
            storageType: null,
            mixStatesFlag: 'Y',
            stockState: 'A',
            typeOfStock: 'A',
            specProcFlag: null,
            storesKittable: 'Y',
            storesKittingPriority: null,
            accessibleFlag: 'Y',
            defaultStockPool: null,
            auditFrequencyWeeks: null
        });
    }

    const selectedSite = () => {
        if (formValues.siteCode && sitesResult) {
            return sitesResult.find(s => s.siteCode === formValues.siteCode);
        }
        return null;
    };

    const locationCodePrefix = newAreaCode => {
        const site = selectedSite();
        if (!site) {
            return '';
        }
        const area = site.storageAreas.find(s => s.storageAreaCode === newAreaCode);
        return area ? `${site.sitePrefix}-${area.areaPrefix}-` : '';
    };

    const lastAuditInfo = () => {
        if (locationGetResult) {
            if (locationGetResult.dateLastAudited) {
                return `Last audited ${moment(locationGetResult.dateLastAudited).format('DD MMM YY')} by ${locationGetResult.auditedBy} dept ${locationGetResult.auditedByDepartmentCode} ${locationGetResult.auditedByDepartmentName}`;
            }

            return 'Not audited';
        }
        return null;
    };

    const handleFieldChange = (propertyName, newValue) => {
        if (creating && propertyName === 'storageAreaCode') {
            setFormValues(current => ({
                ...current,
                [propertyName]: newValue,
                locationCode: locationCodePrefix(newValue)
            }));
        } else {
            setFormValues(current => ({ ...current, [propertyName]: newValue }));
        }
        setChangesMade(true);
    };

    const okToSave = () => {
        if (creating) {
            return formValues.siteCode && formValues.storageAreaCode && formValues.accountingCompany && formValues.description;
        }
        return true;
    }

    const makeInvalid = () => {
        handleFieldChange("dateInvalid", new Date());
    }

    useEffect(() => {
        if (updateResult) {
            setFormValues(updateResult);
        }
    }, [updateResult]);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                <Grid size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create Storage Location' : 'Storage Location'}
                    </Typography>
                </Grid>
                {updateError && (
                    <Grid size={12}>
                        <List>
                            <ErrorCard errorMessage={updateError} />
                        </List>
                    </Grid>
                )}
                {createError && (
                    <Grid size={12}>
                        <List>
                            <ErrorCard errorMessage={createError} />
                        </List>
                    </Grid>
                )}

                {isLoading || updateLoading || createLoading ? (
                    <Grid size={12}>
                        <List>
                            <Loading />
                        </List>
                    </Grid>
                ) : (
                    formValues && (
                        <>
                            <Grid size={5}>
                                {sitesResult && <Dropdown
                                    value={formValues.siteCode}
                                    fullWidth
                                    label="Site"
                                    propertyName="siteCode"
                                    disabled={!creating}
                                    allowNoValue
                                    items={sitesResult?.map(c => ({
                                        id: c.siteCode,
                                        displayText: c.description
                                    }))}
                                    onChange={handleFieldChange}
                                />}
                            </Grid>
                            <Grid size={7}>
                                <Dropdown
                                    value={formValues.storageAreaCode}
                                    fullWidth
                                    label="Area"
                                    propertyName="storageAreaCode"
                                    disabled={!creating}
                                    allowNoValue
                                    items={selectedSite()?.storageAreas?.map(c => ({
                                        id: c.storageAreaCode,
                                        displayText: c.description
                                    }))}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={5}>
                                <InputField
                                    disabled={
                                        !creating ||
                                        !formValues.siteCode ||
                                        !formValues.storageAreaCode
                                    }
                                    value={formValues.locationCode}
                                    fullWidth
                                    label="Code"
                                    propertyName="locationCode"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={7}>
                                <InputField
                                    value={formValues.description}
                                    fullWidth
                                    label="Description"
                                    propertyName="description"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={5}>
                                {accountingCompaniesResult && (
                                    <Dropdown
                                        value={formValues.accountingCompany}
                                        fullWidth
                                        label="Accounting Company"
                                        propertyName="accountingCompany"
                                        allowNoValue
                                        items={accountingCompaniesResult.map(c => ({
                                            id: c.name,
                                            displayText: c.description
                                        }))}
                                        onChange={handleFieldChange}
                                    />
                                )}
                            </Grid>
                            <Grid size={7}>
                                {storageTypesResult && <Dropdown
                                    value={formValues.storageType}
                                    fullWidth
                                    label="Storage Type"
                                    propertyName="storageType"
                                    allowNoValue
                                    items={storageTypesResult.map(c => ({
                                        id: c.storageTypeCode,
                                        displayText: `${c.storageTypeCode} - ${c.description}` 
                                    }))}
                                    onChange={handleFieldChange}
                                />}
                            </Grid>
                            <Grid size={5}>
                                {stockPoolsResult && <Dropdown
                                    value={formValues.defaultStockPool}
                                    fullWidth
                                    label="Default Stock Pool"
                                    propertyName="defaultStockPool"
                                    allowNoValue
                                    items={stockPoolsResult.map(c => ({
                                        id: c.stockPoolCode,
                                        displayText: c.stockPoolDescription
                                    }))}
                                    onChange={handleFieldChange}
                                />}
                            </Grid>
                            <Grid size={4}>
                                <InputField
                                    value={formValues.salesAccountId}
                                    fullWidth
                                    label="Account Id"
                                    propertyName="salesAccountId"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={formValues.outletNumber}
                                    fullWidth
                                    label="Outlet Num"
                                    propertyName="outletNumber"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    value={formValues.mixStatesFlag}
                                    fullWidth
                                    label="Mix States?"
                                    propertyName="mixStatesFlag"
                                    items={[
                                        { id: 'Y', displayText: 'Yes' },
                                        { id: 'N', displayText: 'No' }
                                    ]}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={3}>
                                <Dropdown
                                    value={formValues.stockState}
                                    fullWidth
                                    label="Stock State"
                                    propertyName="stockState"
                                    items={[
                                        { id: 'A', displayText: 'Any Stock' },
                                        { id: 'I', displayText: 'Inspected Stock' },
                                        { id: 'Q', displayText: 'QC/Fail Stock' }
                                    ]}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={4}>
                                <Dropdown
                                    value={formValues.stockState}
                                    fullWidth
                                    label="Type of Stock"
                                    propertyName="stockState"
                                    items={[
                                        { id: 'A', displayText: 'Any Stock' },
                                        { id: 'R', displayText: 'Raw Materials' },
                                        { id: 'F', displayText: 'Finished Goods' }
                                    ]}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={formValues.salesAccountId}
                                    fullWidth
                                    label="Spec Proc Flag"
                                    propertyName="specProcFlag"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    value={formValues.mixStatesFlag}
                                    fullWidth
                                    label="Stores Kittable"
                                    propertyName="storesKittableFlag"
                                    allowNoValue
                                    items={[
                                        { id: 'Y', displayText: 'Yes' },
                                        { id: 'N', displayText: 'No' }
                                    ]}
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={3}>
                                <InputField
                                    value={formValues.salesAccountId}
                                    type="number"
                                    fullWidth
                                    label="Stores Kit Priority"
                                    propertyName="storesKittingPriority"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={4}>
                                <InputField
                                    value={formValues.auditFrequencyWeeks}
                                    type="number"
                                    fullWidth
                                    label="Audit Freq (wks)"
                                    propertyName="auditFrequencyWeeks"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={3}>
                                <Dropdown
                                    value={formValues.accessibleFlag}
                                    fullWidth
                                    label="Accessible"
                                    propertyName="accessibleFlag"
                                    allowNoValue
                                    items={[
                                        { id: 'Y', displayText: 'Yes' },
                                        { id: 'N', displayText: 'No' }
                                    ]}
                                    onChange={handleFieldChange}
                                />
                            </Grid> 
                            {!creating && <>
                                <Grid size={2}>
                                    <InputField
                                        value={formValues.locationId} 
                                        disabled                                    
                                        fullWidth
                                        label="Location Id"
                                        propertyName="locationId"
                                    />
                                </Grid>
                                <Grid size={3}>
                                    <InputField
                                        value={formValues.dateInvalid ? moment(formValues.dateInvalid).format('DD MMM YYYY') : ""} 
                                        disabled                                    
                                        fullWidth
                                        label="Date Invalid"
                                        propertyName="locationId"
                                    />
                                </Grid>                            
                                <Grid size={7}>
                                    {!formValues.dateInvalid && <Button onClick={makeInvalid}>Make Invalid</Button>}
                                </Grid>                            
                            </>}  
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/storage')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createLocation(null, formValues);
                                        } else {
                                            updateLocation(id, formValues);
                                        }
                                    }}
                                    saveDisabled={!changesMade || !okToSave()}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        if (creating) {
                                            setFormValues({ countryCode: 'GB' });
                                        } else {
                                            setFormValues(locationGetResult);
                                        }
                                    }}
                                />
                            </Grid>
                            {!creating && (
                                <Grid size={12}>
                                    <Typography variant="body2">{lastAuditInfo()}</Typography>
                                </Grid>
                            )}
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}

StorageLocation.propTypes = { creating: PropTypes.bool };
StorageLocation.defaultProps = { creating: false };

export default StorageLocation;
