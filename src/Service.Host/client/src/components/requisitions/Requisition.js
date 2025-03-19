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
import useUserProfile from '../../hooks/useUserProfile';
import CancelWithReasonDialog from '../CancelWithReasonDialog';
import requisitionReducer from './reducers/requisitonReducer';
import LinesTab from './LinesTab';
import MovesTab from './MovesTab';
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
        result,
        clearData: clearReqResult
    } = useGet(itemTypes.requisitions.url, true);
    const [hasFetched, setHasFetched] = useState(0);
    const [functionCodeError, setFunctionCodeError] = useState(null);

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
    } = useGet(itemTypes.functionCodes.url, true);

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
        postResult: cancelResult,
        clearPostResult: clearCancelResult
    } = usePost(`${itemTypes.requisitions.url}/cancel`, true);

    const {
        send: book,
        isLoading: bookLoading,
        errorMessage: bookError,
        postResult: bookResult,
        clearPostResult: clearBookResult
    } = usePost(`${itemTypes.requisitions.url}/book`, true);

    const {
        send: authorise,
        isLoading: authoriseLoading,
        errorMessage: authoriseError,
        postResult: authoriseResult,
        clearPostResult: clearAuthoriseResult
    } = usePost(`${itemTypes.requisitions.url}/authorise`, true);

    const {
        send: createReq,
        isLoading: createLoading,
        errorMessage: createError
    } = usePost(itemTypes.requisitions.url, true, true);
    const [validated, setValidated] = useState(false);
    const {
        send: validateReq,
        isLoading: validateLoading,
        errorMessage: validationError,
        clearPostResult: clearValidation,
        postResult: validationSuccess
    } = usePost(`${itemTypes.requisitions.url}/validate`, true, true);

    useEffect(() => {
        if (validationSuccess) {
            setValidated(true);
        }
    }, [validationSuccess]);

    useEffect(() => {
        // if any of the fields in the dependency array change, the validation needs to be run again
        setValidated(false);
    }, [
        formState?.storesFunction?.functionCode,
        formState?.nominal?.nominalCode,
        formState?.department?.departmentCode
    ]);

    const {
        send: updateReq,
        isLoading: updateLoading,
        errorMessage: updateError,
        postResult: updateResult,
        clearPostResult: clearUpdateResult
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

    const [hasLoadedDefaultState, setHasLoadedDefaultState] = useState(false);

    const [revertState, setRevertState] = useState({});

    useEffect(() => {
        if (creating && !hasLoadedDefaultState && userNumber) {
            setHasLoadedDefaultState(true);
            const defaults = { userNumber, userName: name };
            dispatch({
                type: 'load_create',
                payload: defaults
            });
            setRevertState(defaults);
        }
        if (cancelResult) {
            dispatch({ type: 'load_state', payload: cancelResult });
            clearCancelResult();
        }
        if (bookResult) {
            dispatch({ type: 'load_state', payload: bookResult });
            clearBookResult();
        }
        if (authoriseResult) {
            dispatch({ type: 'load_state', payload: authoriseResult });
            clearAuthoriseResult();
        }
        if (result) {
            dispatch({ type: 'load_state', payload: result });
            setRevertState(result);
            clearReqResult();
        }
        if (updateResult) {
            dispatch({ type: 'load_state', payload: updateResult });
            clearUpdateResult();
        }
    }, [
        result,
        cancelResult,
        bookResult,
        authoriseResult,
        creating,
        name,
        userNumber,
        hasLoadedDefaultState,
        updateResult,
        clearUpdateResult,
        clearCancelResult,
        clearBookResult,
        clearReqResult,
        clearAuthoriseResult
    ]);

    const handleHeaderFieldChange = (fieldName, newValue) => {
        dispatch({ type: 'set_header_value', payload: { fieldName, newValue } });
    };

    const requiresDepartmentNominal = () => {
        if (formState.storesFunction) {
            // either departmentNominalRequired is null or Y
            return (
                !formState.storesFunction?.departmentNominalRequired ||
                formState.storesFunction?.departmentNominalRequired === 'Y'
            );
        }
        return false;
    };

    const validDepartmentNominal = () => {
        if (!requiresDepartmentNominal()) {
            return true;
        }

        return formState.nominal?.nominalCode && formState.department?.departmentCode;
    };

    const validFromState = () => {
        if (formState.reqType === 'F' && formState.storesFunction?.fromStateRequired === 'Y') {
            return !!formState.fromState;
        }

        return true;
    };

    const canAddLines = () => {
        if (!formState.storesFunction) {
            return false;
        }

        if (formState.cancelled === 'Y' || formState.dateBooked) {
            return false;
        }

        if (formState.part?.partNumber) {
            // if front page part no need for lines
            return false;
        }

        // from hardcoding in REQLINES.when-new-record-instance
        const partNosNotRequiredFuncs = ['LOAN OUT', 'LOAN BACK', 'CUSTRET', 'SUKIT'];
        if (partNosNotRequiredFuncs.includes(formState.storesFunction?.code)) {
            return false;
        }

        return validDepartmentNominal() && validFromState();
    };

    const canBookLines = () => {
        if (result && utilities.getHref(result, 'book')) {
            return true;
        }
        return false;
    };

    // just for now to only allow updates of comments field
    const [commentsUpdated, setCommentsUpdated] = useState(false);

    const newMovesOntoAreValid = () => {
        const newLines = formState.lines?.filter(x => x.isAddition);
        if (newLines?.length) {
            if (
                newLines.every(l =>
                    !l.moves
                        ? false
                        : l.moves.every(m => m.qty && (m.toLocationCode || m.toPalletNumber))
                )
            ) {
                return true;
            }
        }
        return false;
    };

    const saveIsValid = () => {
        if (creating) {
            // validation perfomed on the server
            return true;
        }

        // Allow saving if stock is picked for an either a new or existing line
        if (formState.lines.some(l => l.stockPicked)) {
            return true;
        }

        //  or  a new line has been added with valid "onto" moves
        if (newMovesOntoAreValid()) {
            return true;
        }

        if (!creating) {
            return commentsUpdated;
        }
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
                if (utilities.getHref(code, 'create-req')) {
                    dispatch({
                        type: 'set_header_value',
                        payload: {
                            fieldName: 'storesFunction',
                            newValue: code
                        }
                    });
                    setFunctionCodeError(null);
                    setDefaultHeaderFieldsForFunctionCode(code);
                } else {
                    setFunctionCodeError(`You dont have permission for ${code.code}`);
                }
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

    const handleDocument1Select = selected => {
        dispatch({
            type: 'set_header_value',
            payload: {
                fieldName: 'part',
                newValue: {
                    partNumber: selected.partNumber,
                    description: selected.partDescription
                }
            }
        });
        dispatch({
            type: 'set_header_value',
            payload: {
                fieldName: 'document1Line',
                newValue: selected.document1Line
            }
        });
        if (selected.batchRef) {
            dispatch({
                type: 'set_header_value',
                payload: { fieldName: 'batchRef', newValue: selected.batchRef }
            });
        }
    };

    // for now...
    // might be a better way to work out whether these things are valid operations
    const canAddMovesOnto =
        selectedLine &&
        ((formState?.storesFunction?.code === 'LDREQ' && formState?.reqType === 'O') ||
            (formState?.manualPick && formState?.reqType === 'O'));

    //todo also needs to be improved
    const canAddMoves = selectedLine && formState?.storesFunction?.code === 'MOVE';

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
                {validationError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={validationError} />
                    </Grid>
                )}
                {functionCodeError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={functionCodeError} />
                    </Grid>
                )}
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
                {updateError && (
                    <Grid size={12}>
                        <ErrorCard errorMessage={updateError} />
                    </Grid>
                )}
                {(fetchLoading ||
                    cancelLoading ||
                    bookLoading ||
                    authoriseLoading ||
                    createLoading ||
                    updateLoading) && (
                    <Grid size={12}>
                        <Loading />
                    </Grid>
                )}
                {!fetchLoading &&
                    !cancelLoading &&
                    !createLoading &&
                    !updateLoading &&
                    formState && (
                        <>
                            {shouldRender(null, false) && (
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
                                        bookUrl={utilities.getHref(formState, 'book')}
                                        onBook={() => {
                                            book(null, { reqNumber });
                                        }}
                                    />
                                    <Grid size={2} />
                                </>
                            )}

                            <Grid size={2}>
                                {!codesLoading && functionCodes && (
                                    <Search
                                        propertyName="storesFunction"
                                        label="Function"
                                        resultsInModal
                                        resultLimit={100}
                                        disabled={!creating || !!formState.lines?.length}
                                        helperText="<Enter> to search or <Tab> to select"
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
                                        displayChips
                                        loading={false}
                                        searchResults={functionCodes
                                            .filter(
                                                f =>
                                                    f.functionAvailable &&
                                                    f.code.includes(
                                                        formState.storesFunction?.code
                                                            ?.trim()
                                                            .toUpperCase()
                                                    )
                                            )
                                            .map(f => ({
                                                ...f,
                                                id: f.code,
                                                name: f.code,
                                                description: f.description,
                                                chips: !utilities.getHref(f, 'create-req')
                                                    ? [
                                                          {
                                                              text: 'no permission',
                                                              color: 'light gray'
                                                          }
                                                      ]
                                                    : []
                                            }))}
                                        onKeyPressFunctions={[
                                            { keyCode: 9, action: getAndSetFunctionCode }
                                        ]}
                                        priorityFunction="closestMatchesFirst"
                                        onResultSelect={r => {
                                            if (utilities.getHref(r, 'create-req')) {
                                                dispatch({
                                                    type: 'set_header_value',
                                                    payload: {
                                                        fieldName: 'storesFunction',
                                                        newValue: r
                                                    }
                                                });
                                                setFunctionCodeError(null);
                                                setDefaultHeaderFieldsForFunctionCode(r);
                                            } else {
                                                setFunctionCodeError(
                                                    `You dont have permission for ${r.code}`
                                                );
                                            }
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
                                    disabled
                                    label="Date Created"
                                    propertyName="dateCreated"
                                />
                            </Grid>
                            <Grid size={2}>
                                <Button
                                    disabled={
                                        formState.cancelled === 'Y' ||
                                        formState.dateBooked ||
                                        creating
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
                                disabled={!creating} // todo - maybe disable changing dept/nom if lines have already been added?
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
                                shouldRender={shouldRender(requiresDepartmentNominal)}
                                enterNominal={!formState?.storesFunction?.nominalCode}
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
                                            items={[
                                                { id: 'Y', displayText: 'Yes' },
                                                { id: 'N', displayText: 'No' }
                                            ]}
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
                                            items={[
                                                { id: 'F', displayText: 'From Stock' },
                                                { id: 'O', displayText: 'Return To Stock' }
                                            ]}
                                            allowNoValue
                                            disabled={!creating || formState.lines?.length}
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
                                onSelect={handleDocument1Select}
                                partSource={formState.storesFunction?.partSource}
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
                                disabled={!creating}
                                shouldRender={
                                    formState.storesFunction &&
                                    formState.storesFunction?.partNumberRequired
                                }
                            />
                            <StockOptions
                                fromState={formState.fromState}
                                fromStockPool={formState.fromStockPool}
                                batchDate={formState.batchDate}
                                toState={formState.toState}
                                toStockPool={formState.toStockPool}
                                stockPools={stockPools}
                                partNumber={formState.part?.partNumber}
                                quantity={formState.quantity}
                                fromLocationCode={formState.fromLocationCode}
                                fromPalletNumber={formState.fromPalletNumber}
                                doPickStock={doPickStock}
                                toLocationCode={formState.toLocationCode}
                                toPalletNumber={formState.toPalletNumber}
                                functionCode={formState.storesFunction}
                                batchRef={formState.batchRef}
                                setItemValue={(fieldName, newValue) =>
                                    dispatch({
                                        type: 'set_header_value',
                                        payload: { fieldName, newValue }
                                    })
                                }
                                disabled={stockStatesLoading || stockPoolsLoading || !creating}
                                shouldRender={shouldRender(() => {
                                    const isMoveFunction =
                                        formState.storesFunction?.code === 'MOVE';
                                    const isLocationRequired =
                                        formState.fromLocationRequired !== 'N' ||
                                        formState.toLocationRequired !== 'N';
                                    const isLoanOutWhileCreating =
                                        formState.storesFunction?.code === 'LOAN OUT' && creating;

                                    return (
                                        isMoveFunction ||
                                        (isLocationRequired && !isLoanOutWhileCreating)
                                    );
                                })}
                            />
                            <Grid size={6}>
                                <InputField
                                    fullWidth
                                    value={formState.comments}
                                    onChange={(propertyName, newValue) => {
                                        handleHeaderFieldChange(propertyName, newValue);
                                        setCommentsUpdated(true);
                                    }}
                                    label="Comments"
                                    propertyName="comments"
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    fullWidth
                                    value={formState.reference}
                                    onChange={handleHeaderFieldChange}
                                    disabled={!creating}
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
                                        isFromStock={
                                            formState.reqType === 'F' || !formState.reqType
                                        }
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
                                        fromState={formState.fromState}
                                    />
                                )}
                                {tab === 1 && (
                                    <MovesTab
                                        moves={
                                            formState.lines?.find(
                                                x => x.lineNumber === selectedLine
                                            )?.moves
                                        }
                                        stockPools={stockPools}
                                        stockStates={stockStates}
                                        addMoveOnto={
                                            canAddMovesOnto
                                                ? () => {
                                                      dispatch({
                                                          type: 'add_move_onto',
                                                          payload: {
                                                              lineNumber: selectedLine
                                                          }
                                                      });
                                                  }
                                                : null
                                        }
                                        addMove={
                                            canAddMoves
                                                ? () => {
                                                      dispatch({
                                                          type: 'add_move',
                                                          payload: {
                                                              lineNumber: selectedLine
                                                          }
                                                      });
                                                  }
                                                : null
                                        }
                                        updateMoveOnto={updated =>
                                            dispatch({
                                                type: 'update_move_onto',
                                                payload: {
                                                    lineNumber: selectedLine,
                                                    ...updated
                                                }
                                            })
                                        }
                                    />
                                )}
                                {tab === 2 && (
                                    <TransactionsTab
                                        transactions={
                                            formState.lines?.find(
                                                x => x.lineNumber === selectedLine
                                            )?.storesBudgets
                                        }
                                    />
                                )}
                            </Grid>
                            <Grid size={12}>
                                <Box sx={{ float: 'right' }}>
                                    <Button
                                        disabled={validateLoading || validated}
                                        onClick={() => {
                                            clearValidation();
                                            validateReq(null, formState);
                                        }}
                                    >
                                        {validateLoading ? 'validating...' : 'validate'}
                                    </Button>
                                </Box>
                            </Grid>
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    saveDisabled={!saveIsValid()}
                                    cancelClick={() => {
                                        dispatch({ type: 'load_state', payload: revertState });
                                    }}
                                    saveClick={() => {
                                        if (creating) {
                                            createReq(null, formState);
                                        } else {
                                            updateReq(reqNumber, formState);
                                        }
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
