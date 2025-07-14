import Grid from '@mui/material/Grid';
import {
    EntityList,
    CreateButton,
    useInitialise,
    utilities,
    Loading
} from '@linn-it/linn-form-components-library';
import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';

function StorageSites() {
    const { isLoading, result } = useInitialise(itemTypes.storageSites.url);

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Storage Sites">
            <Grid container spacing={3}>
                <Grid size={11} />
                <Grid size={1}>
                    <CreateButton createUrl="/service/rsns/create" />
                </Grid>
                {isLoading ? (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <Grid size={12}>
                        {result && (
                            <EntityList
                                descriptionFieldName="description"
                                title="Storage Sites"
                                entityList={result.map(x => ({
                                    ...x,
                                    href: utilities.getSelfHref(x)
                                }))}
                                entityId="siteCode"
                            />
                        )}
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default StorageSites;
