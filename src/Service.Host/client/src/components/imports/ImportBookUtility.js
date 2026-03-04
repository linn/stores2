import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import List from '@mui/material/List';
import Grid from '@mui/material/Grid';
import {
    ErrorCard,
    InputField,
    Loading,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import usePost from '../../hooks/usePost';
import Page from '../Page';

function ImportBookUtility({ creating }) {
    const [hasFetched, setHasFetched] = useState(false);
    const { id } = useParams();
    const {
        send: getImportBook,
        isLoading,
        result: importBookGetResult
    } = useGet(itemTypes.importBooks.url);

    const {
        send: createImportBook,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.importBooks.url, true, true);

    if (!creating && !hasFetched) {
        setHasFetched(true);
        getImportBook(id);
    }

    const navigate = useNavigate();

    const [importBook, setImportBook] = useState();
    const [changesMade, setChangesMade] = useState(false);

    if (importBookGetResult && !importBook) {
        setImportBook(importBookGetResult);
    }

    if (creating && !importBook) {
        setImportBook({});
    }

    const handleFieldChange = (propertyName, newValue) => {
        setImportBook(current => ({ ...current, [propertyName]: newValue }));
        setChangesMade(true);
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {createError && (
                    <Grid size={12}>
                        <List>
                            <ErrorCard errorMessage={createError} />
                        </List>
                    </Grid>
                )}

                {isLoading || createLoading ? (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                ) : (
                    importBook && (
                        <>
                            <Grid size={6}>
                                <InputField
                                    disabled
                                    value={importBook.id}
                                    fullWidth
                                    label="Import Book Id"
                                    propertyName="id"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    backClick={() => navigate('/stores2/import-books')}
                                    saveClick={() => {
                                        setChangesMade(false);

                                        if (creating) {
                                            createImportBook(null, importBook);
                                        }
                                    }}
                                    saveDisabled={!changesMade}
                                    cancelClick={() => {
                                        setChangesMade(false);
                                        if (creating) {
                                            setImportBook({});
                                        } else {
                                            setImportBook(importBookGetResult);
                                        }
                                    }}
                                />
                            </Grid>
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}
ImportBookUtility.propTypes = { creating: PropTypes.bool };
ImportBookUtility.defaultProps = { creating: false };

export default ImportBookUtility;
