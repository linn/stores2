import React, { useEffect, useReducer, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import Typography from '@mui/material/Typography';
import { useNavigate, useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid2';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import {
    InputField,
    Loading,
    DatePicker,
    Dropdown,
    ErrorCard,
    Search,
    SaveBackCancelButtons,
    utilities
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Page from '../Page';
import config from '../../config';
import itemTypes from '../../itemTypes';
import useGet from '../../hooks/useGet';
import useInitialise from '../../hooks/useInitialise';
import usePost from '../../hooks/usePost';
import requisitionReducer from './reducers/requisitonReducer';
import LinesTab from './LinesTab';
import CancelWithReasonDialog from '../CancelWithReasonDialog';
import MovesTab from './MovesTab';
import useUserProfile from '../../hooks/useUserProfile';
import TransactionsTab from './TransactionsTab';
import BookedBy from './components/BookedBy';
import AuthBy from './components/AuthBy';
import DepartmentNominal from './components/DepartmentNominal';
import PartNumberQuantity from './components/PartNumberQuantity';
import StockOptions from './components/StockOptions';
import Document1 from './components/Document1';

function Requisition({ creating }) {
    const navigate = useNavigate();
    const { userNumber, name } = useUserProfile();
    const { reqNumber } = useParams();

    const {
        send: fetchReq,
        isLoading: fetchLoading,
        result
    } = useGet(itemTypes.requisitions.url, true);
    const [hasFetched, setHasFetched] = useState(0);

    const auth = useAuth();
    const token = auth.user?.access_token;

    const { result: stockStates, loading: stockStatesLoading } = useInitialise(
        itemTypes.stockStates.url
    );
    const { result: stockPools, loading: stockPoolsLoading } = useInitialise(
        itemTypes.stockPools.url
    );

    const {
        send: fetchFunctionCodes,
        isLoading: codesLoading,
        result: functionCodes
    } = useGet(itemTypes.functionCodes.url);

    if ((!hasFetched || (reqNumber && hasFetched !== reqNumber)) && token) {
        if (!creating && reqNumber) {
            fetchReq(reqNumber);
        }

        fetchFunctionCodes();
        setHasFetched(reqNumber ?? 1);
    }

    const {
        send: cancel,
        isLoading: cancelLoading,
        errorMessage: cancelError,
        postResult: cancelResult
    } = usePost(`${itemTypes.requisitions.url}/cancel`, true);

    const {
        send: book,
        isLoading: bookLoading,
        errorMessage: bookError,
        postResult: bookResult
    } = usePost(`${itemTypes.requisitions.url}/book`, true);

    const {
        send: authorise,
        isLoading: authoriseLoading,
        errorMessage: authoriseError,
        postResult: authoriseResult
    } = usePost(`${itemTypes.requisitions.url}/authorise`, true);

    const {
        send: createReq,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.requisitions.url, true, true);

    const [tab, setTab] = useState(0);
    const [selectedLine, setSelectedLine] = useState(1);

    const [cancelDialogVisible, setCancelDialogVisible] = useState(false);

    const handleChange = (_, newValue) => {
        setTab(newValue);
    };

    const [formState, dispatch] = useReducer(requisitionReducer, null);

    useEffect(
        () => () => {
            dispatch({ type: 'clear' });
        },
        []
    );

    useEffect(() => {
        if (cancelResult) {
            dispatch({ type: 'load_state', payload: cancelResult });
        } else if (bookResult) {
            dispatch({ type: 'load_state', payload: bookResult });
        } else if (authoriseResult) {
            dispatch({ type: 'load_state', payload: bookResult });
        } else if (result) {
            dispatch({ type: 'load_state', payload: result });
        } else if (creating) {
            dispatch({ type: 'load_create', payload: { userNumber, userName: name } });
        }
    }, [result, cancelResult, bookResult, authoriseResult, creating, name, userNumber]);

    const handleHeaderFieldChange = (fieldName, newValue) => {
        dispatch({ type: 'set_header_value', payload: { fieldName, newValue } });
    };

    const canAddLines = () => {
        if (formState.cancelled !== 'N' || formState.dateBooked) {
            return false;
        }

        if (formState.storesFunction?.code === 'LDREQ') {
            if (
                formState.nominal?.nominalCode &&
                formState.department?.departmentCode &&
                formState.reqType &&
                formState.storesFunction.description
            ) {
                return true;
            }
        }

        if (formState.storesFunction?.code === 'MOVE') {
            if (!formState.part?.partNumber) {
                return true;
            }
        }

        return false;
    };

    const canBookLines = () => {
        if (result && utilities.getHref(result, 'book')) {
            return true;
        }
        return false;
    };

    const optionalOrNeeded = code => {
        if (code === 'O' || code === 'Y') {
            return true;
        }

        return false;
    };

    const okToSaveFrontPageMove = () => {
        if (formState.storesFunction && formState.part?.partNumber && !formState?.lines?.length) {
            if (
                optionalOrNeeded(formState.storesFunction.fromLocationRequired) &&
                !formState.fromLocationCode &&
                !formState.fromPalletNumber
            ) {
                return false;
            }

            if (
                optionalOrNeeded(formState.storesFunction.fromStockPoolRequired) &&
                !formState.fromStockPool
            ) {
                return false;
            }

            if (
                optionalOrNeeded(formState.storesFunction.fromStateRequired) &&
                !formState.fromState
            ) {
                return false;
            }

            if (
                optionalOrNeeded(formState.storesFunction.quantityRequired) &&
                !formState.quantity
            ) {
                return false;
            }

            if (
                optionalOrNeeded(formState.storesFunction.toLocationRequired) &&
                !formState.toLocationCode &&
                !formState.toPalletNumber
            ) {
                return false;
            }

            if (optionalOrNeeded(formState.storesFunction.toStateRequired) && !formState.toState) {
                return false;
            }

            if (
                optionalOrNeeded(formState.storesFunction.toStockPoolRequired) &&
                !formState.toStockPool
            ) {
                return false;
            }

            return true;
        }

        return false;
    };

    const saveIsValid = () => {
        if (creating) {
            if (formState.part?.partNumber) {
                return okToSaveFrontPageMove();
            }

            if (formState.storesFunction?.code == 'LOAN OUT') {
                return formState.document1;
            }

            return !!formState?.lines?.length;
        }

        return false;
    };

    const setDefaultHeaderFieldsForFunctionCode = selectedFunction => {
        if (selectedFunction.manualPickRequired === 'M') {
            dispatch({
                type: 'set_header_value',
                payload: {
                    fieldName: 'manualPick',
                    newValue: 'Y'
                }
            });
        }
    };

    const getAndSetFunctionCode = () => {
        if (formState.storesFunction?.code) {
            const code = functionCodes.find(
                a => a.code === formState.storesFunction.code.toUpperCase()
            );
            if (code) {
                dispatch({
                    type: 'set_header_value',
                    payload: {
                        fieldName: 'storesFunction',
                        newValue: code
                    }
                });
                setDefaultHeaderFieldsForFunctionCode(code);
            }
        }
    };

    const shouldRender = (renderFunction, showOnCreate = true) => {
        if ((renderFunction && !renderFunction()) || (!showOnCreate && creating)) {
            return false;
        }

        return true;
    };

    const doPickStock = moves => {
        if (moves?.length) {
            dispatch({
                type: 'set_options_from_pick',
                payload: moves[0]
            });
        }
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false}>
            <Grid container spacing={3}>
                {cancelDialogVisible && (
                    <CancelWithReasonDialog
                        visible={cancelDialogVisible}
                        closeDialog={() => setCancelDialogVisible(false)}
                        onConfirm={reason => {
                            cancel(null, { reason, reqNumber });
                        }}
                    />
                )}
                <Grid size={12}>
                    <Typography variant="h6">
                        <span>{creating ? 'Create Requisition' : `Requisition ${reqNumber}`}</span>
                        {formState?.cancelled === 'Y' && (
                            <span style={{ color: 'red' }}> [CANCELLED]</span>
                        )}
                    </Typography>
                </Grid>
                {cancelError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={cancelError} />
                    </Grid>
                )}
                {bookError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={bookError} />
                    </Grid>
                )}
                {authoriseError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={authoriseError} />
                    </Grid>
                )}
                {createError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={createError} />
                    </Grid>
                )}
                {(fetchLoading ||
                    cancelLoading ||
                    bookLoading ||
                    authoriseLoading ||
                    createLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {!fetchLoading && !cancelLoading && !createLoading && formState && (
                    <>
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={formState.reqNumber}
                                type="number"
                                onChange={() => {}}
                                label="Req Number"
                                propertyName="reqNumber"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Dropdown
                                fullWidth
                                items={['Y', 'N']}
                                value={formState.reversed}
                                onChange={() => {}}
                                label="Reversed"
                                propertyName="reversed"
                            />
                        </Grid>
                        <BookedBy
                            shouldRender={shouldRender(null, false)}
                            dateBooked={formState.dateBooked}
                            bookedByName={formState.bookedByName}
                            bookUrl={utilities.getHref(result, 'book')}
                            onBook={() => {
                                book(null, { reqNumber });
                            }}
                        />
                        <Grid size={2} />
                        <Grid size={2}>
                            {!codesLoading && functionCodes && (
                                <Search
                                    propertyName="storesFunction"
                                    label="Function"
                                    resultsInModal
                                    resultLimit={100}
                                    disabled={!creating || !!formState.lines?.length}
                                    helperText="Enter a value, or press enter to view all function codes"
                                    value={formState.storesFunction?.code}
                                    handleValueChange={(_, newVal) => {
                                        dispatch({
                                            type: 'set_header_value',
                                            payload: {
                                                fieldName: 'storesFunction',
                                                newValue: { code: newVal }
                                            }
                                        });
                                    }}
                                    search={() => {}}
                                    loading={false}
                                    searchResults={functionCodes.map(f => ({
                                        ...f,
                                        id: f.code,
                                        name: f.code,
                                        description: f.description
                                    }))}
                                    onKeyPressFunctions={[
                                        { keyCode: 9, action: getAndSetFunctionCode }
                                    ]}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={r => {
                                        dispatch({
                                            type: 'set_header_value',
                                            payload: { fieldName: 'storesFunction', newValue: r }
                                        });
                                        setDefaultHeaderFieldsForFunctionCode(r);
                                    }}
                                    clearSearch={() => {}}
                                    autoFocus={false}
                                />
                            )}
                        </Grid>
                        <Grid size={4}>
                            <InputField
                                fullWidth
                                value={formState.storesFunction?.description}
                                onChange={() => {}}
                                label="Function Code Description"
                                propertyName="storesFunctionDescription"
                            />
                        </Grid>
                        <Grid size={6} />
                        <Grid size={2}>
                            <InputField
                                fullWidth
                                value={formState.createdByName}
                                onChange={() => {}}
                                label="Created By"
                                propertyName="createdByName"
                            />
                        </Grid>
                        <Grid size={2}>
                            <DatePicker
                                value={formState.dateCreated}
                                onChange={() => {}}
                                label="Date Created"
                                propertyName="dateCreated"
                            />
                        </Grid>
                        <Grid size={2}>
                            <Button
                                disabled={
                                    formState.cancelled === 'Y' || formState.dateBooked || creating
                                }
                                variant="contained"
                                sx={{ marginTop: '30px', backgroundColor: 'error.light' }}
                                onClick={() => setCancelDialogVisible(true)}
                            >
                                Cancel Req
                            </Button>
                        </Grid>
                        <Grid size={6} />
                        <DepartmentNominal
                            departmentCode={formState.department?.departmentCode}
                            departmentDescription={formState.department?.description}
                            setDepartment={newDept =>
                                dispatch({
                                    type: 'set_header_value',
                                    payload: { fieldName: 'department', newValue: newDept }
                                })
                            }
                            nominalCode={formState.nominal?.nominalCode}
                            nominalDescription={formState.nominal?.description}
                            setNominal={newNominal =>
                                dispatch({
                                    type: 'set_header_value',
                                    payload: { fieldName: 'nominal', newValue: newNominal }
                                })
                            }
                            shouldRender={shouldRender(
                                () =>
                                    !formState.storesFunction?.departmentNominalRequired ||
                                    formState.storesFunction?.departmentNominalRequired !== 'N'
                            )}
                        />
                        <AuthBy
                            dateAuthorised={formState.dateAuthorised}
                            authorisedByName={formState.authorisedByName}
                            shouldRender={shouldRender(null, false)}
                            authoriseUrl={utilities.getHref(result, 'authorise')}
                            onAuthorise={() => {
                                authorise(null, { reqNumber });
                            }}
                        />
                        {shouldRender(() => formState.storesFunction?.code !== 'MOVE') && (
                            <>
                                <Grid size={2}>
                                    <Dropdown
                                        fullWidth
                                        items={['Y', 'N']}
                                        allowNoValue
                                        value={formState.manualPick}
                                        onChange={() => {}}
                                        label="Manual Pick"
                                        propertyName="manualPick"
                                    />
                                </Grid>
                                <Grid size={2}>
                                    <Dropdown
                                        fullWidth
                                        items={['F', 'O']}
                                        allowNoValue
                                        value={formState.reqType}
                                        onChange={handleHeaderFieldChange}
                                        label="Req Type"
                                        propertyName="reqType"
                                    />
                                </Grid>
                                <Grid size={8} />
                            </>
                        )}
                        <Document1
                            document1={formState.document1}
                            document1Text={formState.storesFunction?.document1Text}
                            handleFieldChange={handleHeaderFieldChange}
                            shouldRender={
                                formState.storesFunction &&
                                formState.storesFunction.document1Required
                            }
                            shouldEnter={formState.storesFunction?.document1Entered && creating}
                        />
                        <PartNumberQuantity
                            partNumber={formState.part?.partNumber}
                            partDescription={formState.part?.description}
                            showQuantity
                            quantity={formState.quantity}
                            setPart={newPart => {
                                dispatch({
                                    type: 'set_header_value',
                                    payload: { fieldName: 'part', newValue: newPart }
                                });
                                dispatch({
                                    type: 'set_header_value',
                                    payload: {
                                        fieldName: 'partNumber',
                                        newValue: newPart?.partNumber
                                    }
                                });
                            }}
                            setQuantity={newQty =>
                                dispatch({
                                    type: 'set_header_value',
                                    payload: { fieldName: 'quantity', newValue: newQty }
                                })
                            }
                            shouldRender
                        />
                        <StockOptions
                            fromState={formState.fromState}
                            fromStockPool={formState.fromStockPool}
                            batchDate={formState.batchDate}
                            toState={formState.toState}
                            toStockPool={formState.toStockPool}
                            stockStates={stockStates}
                            stockPools={stockPools}
                            partNumber={formState.part?.partNumber}
                            quantity={formState.quantity}
                            fromLocationCode={formState.fromLocationCode}
                            fromPalletNumber={formState.fromPalletNumber}
                            doPickStock={doPickStock}
                            toLocationCode={formState.toLocationCode}
                            toPalletNumber={formState.toPalletNumber}
                            setItemValue={(fieldName, newValue) =>
                                dispatch({
                                    type: 'set_header_value',
                                    payload: { fieldName, newValue }
                                })
                            }
                            disabled={stockStatesLoading || stockPoolsLoading}
                            shouldRender={shouldRender(
                                () => formState.storesFunction?.code === 'MOVE'
                            )}
                        />
                        <Grid size={6}>
                            <InputField
                                fullWidth
                                value={formState.comments}
                                onChange={handleHeaderFieldChange}
                                label="Comments"
                                propertyName="comments"
                            />
                        </Grid>
                        <Grid size={6}>
                            <InputField
                                fullWidth
                                value={formState.reference}
                                onChange={handleHeaderFieldChange}
                                label="Reference"
                                propertyName="reference"
                            />
                        </Grid>
                        <Grid size={12}>
                            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                <Tabs value={tab} onChange={handleChange}>
                                    <Tab label="Lines" />
                                    <Tab
                                        label={`Moves (L${selectedLine ?? ''})`}
                                        disabled={!selectedLine}
                                    />
                                    <Tab
                                        label={`Transactions (L${selectedLine ?? ''})`}
                                        disabled={
                                            creating ||
                                            !formState.lines?.find(
                                                x => x.lineNumber === selectedLine
                                            )?.moves?.length
                                        }
                                    />
                                </Tabs>
                            </Box>
                        </Grid>
                        <Grid size={12}>
                            {tab === 0 && (
                                <LinesTab
                                    lines={formState.lines}
                                    selected={selectedLine}
                                    setSelected={setSelectedLine}
                                    cancelLine={cancel}
                                    canBook={canBookLines()}
                                    canAdd={canAddLines()}
                                    addLine={() => {
                                        dispatch({ type: 'add_line' });
                                    }}
                                    pickStock={(lineNumber, stockMoves) => {
                                        dispatch({
                                            type: 'pick_stock',
                                            payload: { lineNumber, stockMoves }
                                        });
                                    }}
                                    showPostings={!creating}
                                    updateLine={(lineNumber, fieldName, newValue) => {
                                        dispatch({
                                            type: 'set_line_value',
                                            payload: {
                                                lineNumber,
                                                fieldName,
                                                newValue
                                            }
                                        });
                                    }}
                                    bookLine={lineNumber => {
                                        book(null, { reqNumber, lineNumber });
                                    }}
                                />
                            )}
                            {tab === 1 && (
                                <MovesTab
                                    moves={
                                        formState.lines?.find(x => x.lineNumber === selectedLine)
                                            ?.moves
                                    }
                                />
                            )}
                            {tab === 2 && (
                                <TransactionsTab
                                    transactions={
                                        formState.lines?.find(x => x.lineNumber === selectedLine)
                                            ?.storesBudgets
                                    }
                                />
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            <SaveBackCancelButtons
                                saveDisabled={!saveIsValid()}
                                cancelClick={() => {
                                    if (creating) {
                                        dispatch({ type: 'load_create' });
                                    }
                                }}
                                saveClick={() => {
                                    createReq(null, formState);
                                }}
                                backClick={() => {
                                    navigate('/requisitions');
                                }}
                            />
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default Requisition;
