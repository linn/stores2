import React, { useEffect, useReducer, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import Typography from '@mui/material/Typography';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import {
    InputField,
    Loading,
    DatePicker,
    Dropdown,
    ErrorCard,
    Search,
    SaveBackCancelButtons,
    utilities,
    LinkField,
    ExportButton
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
import useDebounceValue from '../../hooks/useDebounceValue';
import requisitionReducer from './reducers/requisitonReducer';
import LinesTab from './LinesTab';
import MovesTab from './MovesTab';
import TransactionsTab from './TransactionsTab';
import SerialNumbersTab from './SerialNumbersTab';
import BookedBy from './components/BookedBy';
import AuthBy from './components/AuthBy';
import DepartmentNominal from './components/DepartmentNominal';
import PartNumberQuantity from './components/PartNumberQuantity';
import StockOptions from './components/StockOptions';
import Document1 from './components/Document1';
import Document2 from './components/Document2';
import Document3 from './components/Document3';
import PickRequisitionDialog from './PickRequisitionDialog';
import BookInPostingsDialog from './components/BookInPostingsDialog';
import AuditLocationSearch from './components/AuditLocationSearch';

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
    const [pickRequisitionDialogVisible, setPickRequisitionDialogVisible] = useState(false);
    const [bookInPostingsDialogVisible, setBookInPostingsDialogVisible] = useState(false);
    const [changesMade, setChangesMade] = useState(false);

    const {
        send: fetchReversalPreview,
        result: fetchReversalPreviewResult,
        clearData: clearReversalPreviewResult
    } = useGet(config.appRoot, true);

    const {
        send: getDefaultBookInLocation,
        result: defaultBookInLocationResult,
        clearData: clearDefaultBookInLocationResult
    } = useGet(itemTypes.getDefaultBookInLocation.url, true);

    useEffect(() => {
        if (fetchReversalPreviewResult) {
            dispatch({
                type: 'set_reverse_details',
                payload: fetchReversalPreviewResult
            });
            clearReversalPreviewResult();
        }
    }, [fetchReversalPreviewResult, clearReversalPreviewResult]);

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

    useEffect(() => {
        if (defaultBookInLocationResult) {
            dispatch({
                type: 'set_default_book_in_location',
                payload: defaultBookInLocationResult
            });

            clearDefaultBookInLocationResult();
        }
    }, [clearDefaultBookInLocationResult, defaultBookInLocationResult]);

    if ((!hasFetched || (reqNumber && hasFetched !== reqNumber)) && token) {
        if (!creating && reqNumber) {
            fetchReq(reqNumber);
        }

        if (creating) {
            fetchReq(null, '/application-state');
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
        postResult: validationSuccess,
        cancelRequest: cancelValidation
    } = usePost(`${itemTypes.requisitions.url}/validate`, true, false);

    useEffect(() => {
        if (validationSuccess) {
            setValidated(true);
        }
    }, [validationSuccess]);

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

    const [formState, dispatch] = useReducer(requisitionReducer, { req: null, popUpMessage: null });
    const cancelHref = utilities.getHref(formState.req, 'cancel');
    const bookHref = utilities.getHref(formState.req, 'book');
    const authoriseHref = utilities.getHref(formState.req, 'authorise');
    const reverseHref = utilities.getHref(formState.req, 'create-reverse');
    const createHref = utilities.getHref(formState.req, 'create');
    const printQcLabelsHref = utilities.getHref(formState.req, 'print-qc-labels');
    const deliveryNoteHref = utilities.getHref(formState.req, 'delivery-note');

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
            setHasFetched(false);
            setHasLoadedDefaultState(true);
            const defaults = { req: { userNumber, userName: name } };
            dispatch({
                type: 'load_create',
                payload: defaults
            });
            setRevertState(defaults);
        }
        if (cancelResult) {
            dispatch({ type: 'load_state', payload: cancelResult });
            setRevertState(cancelResult);
            clearCancelResult();
        }
        if (bookResult) {
            dispatch({ type: 'load_state', payload: bookResult });
            setRevertState(bookResult);
            clearBookResult();
        }
        if (authoriseResult) {
            dispatch({ type: 'load_state', payload: authoriseResult });
            setRevertState(authoriseResult);
            clearAuthoriseResult();
        }
        if (result) {
            dispatch({ type: 'load_state', payload: result });
            setRevertState(result);
            clearReqResult();
        }
        if (updateResult) {
            dispatch({ type: 'load_state', payload: updateResult });
            setRevertState(updateResult);
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
        setChangesMade(true);
        dispatch({ type: 'set_header_value', payload: { fieldName, newValue } });
    };

    const requiresDepartmentNominal = () => {
        if (formState.req?.storesFunction) {
            // either departmentNominalRequired is null or Y
            return (
                !formState.req.storesFunction?.departmentNominalRequired ||
                formState.req.storesFunction?.departmentNominalRequired === 'Y'
            );
        }
        return false;
    };

    const validDepartmentNominal = () => {
        if (!requiresDepartmentNominal()) {
            return true;
        }

        return formState.req?.nominal?.nominalCode && formState.req?.department?.departmentCode;
    };

    const validFromState = () => {
        if (
            formState.req?.reqType === 'F' &&
            formState.req?.storesFunction?.fromStateRequired === 'Y'
        ) {
            return !!formState.req.fromState;
        }

        return true;
    };

    const canAddLines = () => {
        if (!formState.req?.storesFunction) {
            return false;
        }

        if (formState.req?.cancelled === 'Y' || formState.req?.dateBooked) {
            return false;
        }

        if (formState.req?.part?.partNumber) {
            // if front page part no need for lines
            return false;
        }

        // from hardcoding in REQLINES.when-new-record-instance
        const partNosNotRequiredFuncs = ['LOAN BACK', 'CUSTRET', 'SUKIT'];
        if (partNosNotRequiredFuncs.includes(formState.req?.storesFunction?.code)) {
            return false;
        }

        if (formState.req?.storesFunction?.linesRequired === 'Y') {
            return true;
        }

        return validDepartmentNominal() && validFromState();
    };

    const canBookLines = () => {
        if (result && bookHref) {
            return true;
        }

        return false;
    };

    const getAndSetFunctionCode = () => {
        setChangesMade(true);
        if (formState.req?.storesFunction?.code) {
            const code = functionCodes.find(
                a => a.code === formState.req.storesFunction.code.toUpperCase()
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
                } else {
                    setFunctionCodeError(`You dont have permission for ${code.code}`);
                }
            }
        }
    };

    const closeMessage = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        dispatch({ type: 'close_message' });
    };

    const shouldRender = (renderFunction, showOnCreate = true) => {
        if ((renderFunction && !renderFunction()) || (!showOnCreate && creating)) {
            return false;
        }

        return true;
    };

    const doPickStock = moves => {
        setChangesMade(true);
        if (moves?.length) {
            dispatch({
                type: 'set_options_from_pick',
                payload: moves[0]
            });
        }
    };

    const handleDocument1Select = selected => {
        setChangesMade(true);
        if (formState.req.storesFunction?.partSource === 'L') {
            dispatch({
                type: 'set_loan',
                payload: selected
            });
            if (formState.req.isReverTransaction !== 'Y') {
                dispatch({
                    type: 'set_header_value',
                    payload: { fieldName: 'document1Line', newValue: 1 }
                });
            }
        } else {
            dispatch({
                type: 'set_document1_details',
                payload: selected
            });

            if (
                formState.req.storesFunction?.code === 'BOOKLD' &&
                selected.document1 &&
                selected.document1Line
            ) {
                setBookInPostingsDialogVisible(true);
            }

            if (formState.req.storesFunction?.code === 'BOOKSU' && selected.partNumber) {
                getDefaultBookInLocation(null, `?partNumber=${selected.partNumber}`);
            }

            if (selected.batchRef) {
                dispatch({
                    type: 'set_header_value',
                    payload: { fieldName: 'batchRef', newValue: selected.batchRef }
                });
            }

            if (selected.toLocationCode) {
                dispatch({
                    type: 'set_header_value',
                    payload: { fieldName: 'toLocationCode', newValue: selected.toLocationCode }
                });
            }

            if (selected.document2) {
                dispatch({
                    type: 'set_header_value',
                    payload: { fieldName: 'document2', newValue: selected.document2 }
                });
            }

            if (selected.document3) {
                dispatch({
                    type: 'set_header_value',
                    payload: { fieldName: 'document3', newValue: selected.document3 }
                });
            }

            if (selected.quantity) {
                dispatch({
                    type: 'set_header_value',
                    payload: { fieldName: 'quantity', newValue: selected.quantity }
                });
            }
        }
        if (
            formState.req?.isReverseTransaction === 'Y' &&
            selected.canReverse &&
            formState.req.storesFunction?.code !== 'BOOKLD'
        ) {
            setPickRequisitionDialogVisible(true);
        }
    };

    const handleDocument1PartSelect = part => {
        setChangesMade(true);
        dispatch({
            type: 'set_part_details',
            payload: part
        });

        if (formState.req?.storesFunction?.partSource === 'WO') {
            dispatch({
                type: 'set_part_header_details_for_WO',
                payload: part
            });
        }

        if (formState.req?.storesFunction?.partSource === 'PO') {
            dispatch({
                type: 'set_part_header_details_for_PO',
                payload: part
            });
        }
    };

    const viewDocument = () => {
        window.open(`${config.appRoot}${deliveryNoteHref}`, '_blank');
    };

    // for now...
    // might be a better way to work out whether these things are valid operations
    const canAddMovesOnto =
        selectedLine &&
        ((formState?.req?.storesFunction?.code === 'LDREQ' && formState?.req?.reqType === 'O') ||
            (formState?.req?.manualPick && formState?.req?.reqreqType === 'O'));
    //todo also needs to be improved
    const canAddMoves = selectedLine && formState?.req?.storesFunction?.code === 'MOVE';

    const requiresSerialNumbers =
        formState?.req?.storesFunction?.code === 'ON DEM' ||
        formState?.req?.storesFunction?.code === 'OFF DEM';

    const canAddSerialNumber =
        selectedLine && !formState?.req?.cancelled !== 'Y' && !formState?.req?.dateBooked;

    const addSerialNumber = () => {
        if (canAddSerialNumber) {
            setChangesMade(true);
            dispatch({
                type: 'add_serial_number',
                payload: {
                    lineNumber: selectedLine
                }
            });
        }
    };

    const [debouncedFormState, isDebouncing] = useDebounceValue(formState);

    useEffect(() => {
        if (!debouncedFormState.req) return;
        clearValidation();
        setValidated(false);
        validateReq(null, debouncedFormState.req, false);

        return () => cancelValidation();
    }, [debouncedFormState, clearValidation, validateReq, cancelValidation]);

    const validToSaveMessage = () => {
        if (!creating && !changesMade) {
            return 'No changes to save';
        }

        if (codesLoading) {
            return 'Loading...';
        }

        if (validateLoading) {
            return 'Thinking...';
        }

        if (validationError) {
            return validationError;
        }

        if (validationSuccess) {
            return 'YES!';
        }

        return '';
    };

    return (
        <Page homeUrl={config.appRoot} showAuthUi={false} title="Req Ut">
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
                <Grid size={7}>
                    <Typography variant="h6">
                        <span>{creating ? 'Create Requisition' : `Requisition ${reqNumber}`}</span>
                        {formState?.req?.cancelled === 'Y' && (
                            <span style={{ color: 'red' }}> [CANCELLED]</span>
                        )}
                    </Typography>
                </Grid>
                <Grid size={1}>
                    <ExportButton
                        buttonText="Pdf"
                        disabled={!reqNumber || creating}
                        accept="application/pdf"
                        fileName={`req ${reqNumber}.pdf`}
                        tooltipText="Download as PDF"
                        accessToken={token}
                        href={`${config.appRoot}/requisitions/${reqNumber}/pdf`}
                    />
                </Grid>
                <Grid size={1}>
                    <LinkField
                        to={`${itemTypes.requisitions.url}/${reqNumber}/view`}
                        disabled={!reqNumber || creating}
                        external={false}
                        openLinksInNewTabs={true}
                        value="Printable View"
                    />
                </Grid>
                <Grid size={1}>
                    <LinkField
                        to={`/requisitions/reports/requisition-cost/${reqNumber}`}
                        disabled={!reqNumber || creating}
                        external={false}
                        value="Cost Of Req"
                    />
                </Grid>
                <Grid size={2}>
                    {!creating && (
                        <Button
                            disabled={!createHref}
                            variant="outlined"
                            onClick={() => {
                                const defaults = { req: { userNumber, userName: name } };
                                clearValidation();
                                clearReqResult();
                                setChangesMade(false);
                                dispatch({
                                    type: 'load_create',
                                    payload: defaults
                                });
                                setTab(0);
                                navigate('/requisitions/create');
                            }}
                        >
                            Create New
                        </Button>
                    )}
                </Grid>
                <Grid size={12}>
                    <Typography variant="subtitle1">
                        {formState?.req?.cancelledReason && (
                            <span style={{ color: 'red' }}>
                                [{formState?.req?.cancelledReason}]
                            </span>
                        )}
                    </Typography>
                </Grid>
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
                    formState &&
                    formState.req && (
                        <>
                            {shouldRender(null, false) && (
                                <>
                                    <Grid size={2}>
                                        <InputField
                                            fullWidth
                                            value={formState.req.reqNumber}
                                            type="number"
                                            onChange={() => {}}
                                            label="Req Number"
                                            propertyName="reqNumber"
                                        />
                                    </Grid>
                                    <Grid size={2}>
                                        <Dropdown
                                            fullWidth
                                            items={[
                                                { id: 'Y', displayText: 'Yes' },
                                                { id: 'N', displayText: 'No ' }
                                            ]}
                                            value={formState.req.isReversed}
                                            onChange={() => {}}
                                            label="Reversed"
                                            disabled
                                            propertyName="isReversed"
                                        />
                                    </Grid>
                                    <BookedBy
                                        shouldRender={shouldRender(null, false)}
                                        dateBooked={formState.req.dateBooked}
                                        bookedByName={formState.req.bookedByName}
                                        bookUrl={bookHref}
                                        onBook={() => {
                                            book(null, { reqNumber });
                                        }}
                                    />
                                    <Grid size={2}>
                                        {printQcLabelsHref && (
                                            <Link to={printQcLabelsHref}>
                                                <Typography variant="subtitle2">
                                                    Print Labels
                                                </Typography>
                                            </Link>
                                        )}
                                        {deliveryNoteHref && (
                                            <Button variant="outlined" onClick={viewDocument}>
                                                Print Delivery Note
                                            </Button>
                                        )}
                                    </Grid>
                                </>
                            )}

                            <Grid size={2}>
                                {!codesLoading && functionCodes && (
                                    <Search
                                        propertyName="storesFunction"
                                        label="Function"
                                        resultsInModal
                                        resultLimit={100}
                                        disabled={!creating || !!formState.req.lines?.length}
                                        helperText={
                                            creating && !formState.req.storesFunction?.description
                                                ? 'Enter a value and press <Enter> to search or <Tab> to select. Alternatively press <Enter> with no input to list all functions'
                                                : ''
                                        }
                                        value={formState.req.storesFunction?.code}
                                        handleValueChange={(_, newVal) => {
                                            setChangesMade(true);
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
                                                    !formState.req.storesFunction?.code ||
                                                    (f.functionAvailable &&
                                                        f.code.includes(
                                                            formState.req.storesFunction?.code
                                                                ?.trim()
                                                                .toUpperCase()
                                                        ))
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
                                    value={formState.req.storesFunction?.description}
                                    onChange={() => {}}
                                    label="Function Code Description"
                                    propertyName="storesFunctionDescription"
                                />
                            </Grid>

                            <Grid size={6}>
                                <InputField
                                    label="Valid to Save?"
                                    fullWidth
                                    rows={2}
                                    error={changesMade && !validated}
                                    propertyName="validToSaveMessage"
                                    value={validToSaveMessage()}
                                />
                            </Grid>

                            <Grid size={2}>
                                <InputField
                                    fullWidth
                                    value={formState.req.createdByName}
                                    onChange={() => {}}
                                    label="Created By"
                                    propertyName="createdByName"
                                />
                            </Grid>
                            <Grid size={2}>
                                <DatePicker
                                    value={formState.req.dateCreated}
                                    onChange={() => {}}
                                    disabled
                                    label="Date Created"
                                    propertyName="dateCreated"
                                />
                            </Grid>
                            <Grid size={2}>
                                <Dropdown
                                    fullWidth
                                    allowNoValue={false}
                                    disabled={
                                        !reverseHref ||
                                        !creating ||
                                        formState.req.storesFunction?.canBeReversed !== 'Y'
                                    }
                                    onChange={handleHeaderFieldChange}
                                    items={[
                                        { id: 'Y', displayText: 'Yes' },
                                        { id: 'N', displayText: 'No ' }
                                    ]}
                                    value={formState.req.isReverseTransaction}
                                    label="Reverse"
                                    propertyName="isReverseTransaction"
                                />
                            </Grid>
                            <Grid size={2}>
                                {shouldRender(
                                    () => formState.req.isReverseTransaction === 'Y' && creating
                                ) && (
                                    <InputField
                                        fullWidth
                                        value={formState.req.originalReqNumber}
                                        onChange={() => {}}
                                        disabled
                                        label="Original Req No"
                                        propertyName="originalReqNumber"
                                    />
                                )}
                                {shouldRender(
                                    () => formState.req.isReverseTransaction === 'Y' && !creating
                                ) && (
                                    <LinkField
                                        value={formState.req.originalReqNumber}
                                        label="Original Req No"
                                        to={`/requisitions/${formState.req.originalReqNumber}`}
                                        external={false}
                                    />
                                )}
                            </Grid>
                            <Grid size={2}>
                                {shouldRender(() => !!cancelHref, false) && (
                                    <Button
                                        disabled={
                                            formState.req.cancelled === 'Y' ||
                                            formState.req.dateBooked ||
                                            creating ||
                                            formState.req.storesFunction?.canBeCancelled !== 'Y'
                                        }
                                        variant="contained"
                                        sx={{ marginTop: '30px', backgroundColor: 'error.light' }}
                                        onClick={() => setCancelDialogVisible(true)}
                                    >
                                        Cancel Req
                                    </Button>
                                )}
                            </Grid>
                            <Grid size={2} />
                            <DepartmentNominal
                                departmentCode={formState.req.department?.departmentCode}
                                departmentDescription={formState.req.department?.description}
                                disabled={!creating} // todo - maybe disable changing dept/nom if lines have already been added?
                                setDepartment={newDept => {
                                    setChangesMade(true);
                                    dispatch({
                                        type: 'set_header_value',
                                        payload: { fieldName: 'department', newValue: newDept }
                                    });
                                }}
                                nominalCode={formState.req.nominal?.nominalCode}
                                nominalDescription={formState.req.nominal?.description}
                                setNominal={newNominal => {
                                    setChangesMade(true);

                                    dispatch({
                                        type: 'set_header_value',
                                        payload: { fieldName: 'nominal', newValue: newNominal }
                                    });
                                }}
                                shouldRender={shouldRender(requiresDepartmentNominal)}
                                enterNominal={!formState.req.storesFunction?.nominalCode}
                            />
                            <AuthBy
                                dateAuthorised={formState.req.dateAuthorised}
                                authorisedByName={formState.req.authorisedByName}
                                shouldRender={shouldRender(null, false)}
                                authoriseUrl={authoriseHref}
                                onAuthorise={() => {
                                    authorise(null, { reqNumber });
                                }}
                            />
                            {shouldRender(
                                () =>
                                    formState.req.storesFunction?.code !== 'MOVE' &&
                                    formState.req.storesFunction?.code !== 'AUDIT'
                            ) && (
                                <>
                                    <Grid size={2}>
                                        {formState.req.storesFunction?.manualPickRequired !==
                                            'X' && (
                                            <Dropdown
                                                fullWidth
                                                items={[
                                                    { id: 'Y', displayText: 'Yes' },
                                                    { id: 'N', displayText: 'No' }
                                                ]}
                                                allowNoValue
                                                value={formState.req.manualPick}
                                                onChange={handleHeaderFieldChange}
                                                label="Manual Pick"
                                                propertyName="manualPick"
                                            />
                                        )}
                                    </Grid>
                                    <Grid size={2}>
                                        {shouldRender(requiresDepartmentNominal) && (
                                            <Dropdown
                                                fullWidth
                                                items={[
                                                    { id: 'F', displayText: 'From Stock' },
                                                    { id: 'O', displayText: 'Return To Stock' }
                                                ]}
                                                allowNoValue
                                                disabled={!creating || formState.req.lines?.length}
                                                value={formState.req.reqType}
                                                onChange={handleHeaderFieldChange}
                                                label="Req Type"
                                                propertyName="reqType"
                                            />
                                        )}
                                    </Grid>
                                    <Grid size={8} />
                                </>
                            )}
                            <AuditLocationSearch
                                auditLocation={formState.req.auditLocation}
                                disabled={!creating}
                                shouldRender={
                                    formState.req.storesFunction?.auditLocationRequired === 'Y'
                                }
                                setAuditLocation={location =>
                                    dispatch({
                                        type: 'set_header_value',
                                        payload: { fieldName: 'auditLocation', newValue: location }
                                    })
                                }
                                setAuditLocationDetails={selectedLocation =>
                                    dispatch({
                                        type: 'set_audit_location_details',
                                        payload: selectedLocation
                                    })
                                }
                            />
                            <Document1
                                document1={formState.req.document1}
                                document1Text={formState.req.storesFunction?.document1Text}
                                document1Line={formState.req.document1Line}
                                document1LineRequired={
                                    formState.req.storesFunction?.document1LineRequired
                                }
                                handleFieldChange={handleHeaderFieldChange}
                                shouldRender={
                                    formState.req.storesFunction &&
                                    formState.req.storesFunction.document1Required
                                }
                                shouldEnter={
                                    formState.req.storesFunction?.document1Entered && creating
                                }
                                onSelect={handleDocument1Select}
                                partSource={formState.req.storesFunction?.partSource}
                                onSelectPart={handleDocument1PartSelect}
                                document1Details={formState.document1Details}
                                storesFunction={formState.req.storesFunction}
                                qtyOutstanding={formState.document1Details?.qtyOutstanding}
                            />
                            <Document2
                                document2={formState.req.document2}
                                document2Text={formState.req.storesFunction?.document2Text}
                                document2Name={formState.req.document2Name}
                                handleFieldChange={handleHeaderFieldChange}
                                shouldRender={
                                    formState.req.storesFunction &&
                                    formState.req.storesFunction.document2Required
                                }
                                shouldEnter={
                                    formState.req.storesFunction?.document2Entered && creating
                                }
                                document3={formState.req.document3}
                            />
                            <Document3
                                document3={formState.req.document3}
                                storesFunction={formState.req.storesFunction}
                            />
                            <PartNumberQuantity
                                partNumber={formState.req.part?.partNumber}
                                partDescription={formState.req.part?.description}
                                partNumberProperty="partNumber"
                                partDescriptionProperty="partDescription"
                                showQuantity
                                quantity={formState.req.quantity}
                                setPart={
                                    // todo - move this into the reducer?
                                    formState.req.storesFunction?.partSource === 'IP'
                                        ? newPart => {
                                              setChangesMade(true);
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
                                          }
                                        : null
                                }
                                setQuantity={
                                    // todo - again looks like state logic, should maybe live in reducer
                                    formState.req.storesFunction?.quantityRequired !== 'X' ||
                                    formState.req.storesFunction?.code === 'BOOKWO'
                                        ? newQty => {
                                              setChangesMade(true);
                                              dispatch({
                                                  type: 'set_header_value',
                                                  payload: {
                                                      fieldName: 'quantity',
                                                      newValue: newQty
                                                  }
                                              });
                                          }
                                        : null
                                }
                                disabled={!creating || formState.req.isReverseTransaction == 'Y'}
                                shouldRender={
                                    formState.req.storesFunction &&
                                    formState.req.storesFunction?.partNumberRequired
                                }
                            />
                            {formState.req.storesFunction?.code === 'PARTNO CH' && (
                                <PartNumberQuantity
                                    partNumber={formState.req.newPart?.partNumber}
                                    partDescription={formState.req.newPart?.description}
                                    partNumberProperty="newPartNumber"
                                    partDescriptionProperty="newPartDescription"
                                    partLabel="New Part"
                                    showQuantity={false}
                                    setPart={newPart => {
                                        setChangesMade(true);
                                        dispatch({
                                            type: 'set_header_value',
                                            payload: {
                                                fieldName: 'newPart',
                                                newValue: newPart
                                            }
                                        });

                                        dispatch({
                                            type: 'set_header_value',
                                            payload: {
                                                fieldName: 'newPartNumber',
                                                newValue: newPart?.partNumber
                                            }
                                        });
                                    }}
                                    disabled={!creating}
                                    shouldRender={
                                        formState.req.storesFunction &&
                                        formState.req.storesFunction?.code === 'PARTNO CH'
                                    }
                                />
                            )}
                            <StockOptions
                                fromState={formState.req.fromState}
                                fromStockPool={formState.req.fromStockPool}
                                batchDate={formState.req.batchDate}
                                toState={formState.req.toState}
                                toStockPool={formState.req.toStockPool}
                                stockPools={stockPools}
                                partNumber={formState.req.part?.partNumber}
                                quantity={formState.req.quantity}
                                fromLocationCode={formState.req.fromLocationCode}
                                fromPalletNumber={formState.req.fromPalletNumber}
                                doPickStock={
                                    formState.req.storesFunction?.manualPickRequired === 'X'
                                        ? null
                                        : doPickStock
                                }
                                toLocationCode={formState.req.toLocationCode}
                                toPalletNumber={formState.req.toPalletNumber}
                                functionCode={formState.req.storesFunction}
                                batchRef={formState.req.batchRef}
                                setItemValue={(fieldName, newValue) => {
                                    setChangesMade(true);
                                    dispatch({
                                        type: 'set_header_value',
                                        payload: { fieldName, newValue }
                                    });
                                }}
                                disabled={stockStatesLoading || stockPoolsLoading || !creating}
                                shouldRender={shouldRender(() => {
                                    const isMoveFunction =
                                        formState.req.storesFunction?.code === 'MOVE';
                                    const isLocationRequired =
                                        formState.req.storesFunction?.fromLocationRequired !==
                                            'N' ||
                                        formState.req.storesFunction?.toLocationRequired !== 'N';
                                    const isLoanOutWhileCreating =
                                        formState.req.storesFunction?.code === 'LOAN OUT' &&
                                        creating;

                                    return (
                                        isMoveFunction ||
                                        (isLocationRequired && !isLoanOutWhileCreating)
                                    );
                                })}
                            />
                            <Grid size={6}>
                                <InputField
                                    fullWidth
                                    value={formState.req.reference}
                                    onChange={handleHeaderFieldChange}
                                    disabled={!creating}
                                    label="Reference"
                                    propertyName="reference"
                                />
                            </Grid>
                            <Grid size={6}>
                                <InputField
                                    fullWidth
                                    value={formState.req.comments}
                                    onChange={(propertyName, newValue) => {
                                        handleHeaderFieldChange(propertyName, newValue);
                                    }}
                                    label="Comments"
                                    propertyName="comments"
                                />
                            </Grid>
                            {shouldRender(
                                () => formState.req.storesFunction?.receiptDateRequired === 'Y'
                            ) && (
                                <>
                                    <Grid size={2}>
                                        <DatePicker
                                            value={formState.req.dateReceived}
                                            onChange={newDate =>
                                                handleHeaderFieldChange('dateReceived', newDate)
                                            }
                                            disabled={!creating}
                                            label="Date Received"
                                            propertyName="dateReceived"
                                        />
                                    </Grid>
                                    <Grid size={10} />
                                </>
                            )}
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
                                                !formState.req.lines?.find(
                                                    x => x.lineNumber === selectedLine
                                                )?.moves?.length
                                            }
                                        />
                                        {requiresSerialNumbers && (
                                            <Tab
                                                label={`Serial Numbers (L${selectedLine ?? ''})`}
                                                disabled={!selectedLine}
                                            />
                                        )}
                                    </Tabs>
                                </Box>
                            </Grid>
                            <Grid size={12}>
                                {tab === 0 && (
                                    <LinesTab
                                        lines={formState.req.lines}
                                        selected={selectedLine}
                                        setSelected={setSelectedLine}
                                        cancelLine={cancel}
                                        removeLine={lineNumber => {
                                            setChangesMade(true);
                                            dispatch({ type: 'remove_line', payload: lineNumber });
                                        }}
                                        canBook={canBookLines()}
                                        canAdd={canAddLines()}
                                        isFromStock={
                                            formState.req.reqType === 'F' || !formState.req.reqType
                                        }
                                        addLine={() => {
                                            setChangesMade(true);
                                            dispatch({ type: 'add_line' });
                                        }}
                                        pickStock={(lineNumber, stockMoves) => {
                                            setChangesMade(true);
                                            dispatch({
                                                type: 'pick_stock',
                                                payload: { lineNumber, stockMoves }
                                            });
                                        }}
                                        showPostings={!creating}
                                        updateLine={(lineNumber, fieldName, newValue) => {
                                            setChangesMade(true);
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
                                        fromState={formState.req.fromState}
                                        fromStockPool={formState.req.fromStockPool}
                                        transactionOptions={
                                            formState.req.storesFunction?.code === 'AUDIT'
                                                ? formState.req.storesFunction.transactionTypes
                                                : null
                                        }
                                        reqHeader={formState.req}
                                        locationCodeRoot={formState.req.auditLocation}
                                    />
                                )}
                                {tab === 1 && (
                                    <MovesTab
                                        moves={
                                            formState.req.lines?.find(
                                                x => x.lineNumber === selectedLine
                                            )?.moves
                                        }
                                        stockPools={stockPools}
                                        stockStates={stockStates}
                                        addMoveOnto={
                                            canAddMovesOnto
                                                ? () => {
                                                      setChangesMade(true);
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
                                                      setChangesMade(true);
                                                      dispatch({
                                                          type: 'add_move',
                                                          payload: {
                                                              lineNumber: selectedLine
                                                          }
                                                      });
                                                  }
                                                : null
                                        }
                                        locationCodeRoot={formState.req.auditLocation}
                                        updateMoveOnto={updated => {
                                            setChangesMade(true);
                                            dispatch({
                                                type: 'update_move_onto',
                                                payload: {
                                                    lineNumber: selectedLine,
                                                    ...updated
                                                }
                                            });
                                        }}
                                    />
                                )}
                                {tab === 2 && (
                                    <TransactionsTab
                                        transactions={
                                            formState.req.lines?.find(
                                                x => x.lineNumber === selectedLine
                                            )?.storesBudgets
                                        }
                                    />
                                )}
                                {tab === 3 && requiresSerialNumbers && (
                                    <SerialNumbersTab
                                        serialNumbers={
                                            formState.req.lines?.find(
                                                x => x.lineNumber === selectedLine
                                            )?.serialNumbers
                                        }
                                        addSerialNumber={() =>
                                            canAddSerialNumber ? addSerialNumber() : null
                                        }
                                        updateSerialNumber={updated =>
                                            dispatch({
                                                type: 'update_serial_number',
                                                payload: {
                                                    lineNumber: selectedLine,
                                                    ...updated
                                                }
                                            })
                                        }
                                        deleteSerialNumber={seq =>
                                            dispatch({
                                                type: 'delete_serial_number',
                                                payload: {
                                                    lineNumber: selectedLine,
                                                    sernosSeq: seq
                                                }
                                            })
                                        }
                                    />
                                )}
                            </Grid>
                            <Grid size={12}>
                                <SaveBackCancelButtons
                                    saveDisabled={
                                        isDebouncing ||
                                        !validated ||
                                        validateLoading ||
                                        !changesMade
                                    }
                                    cancelClick={() => {
                                        dispatch({ type: 'load_state', payload: revertState });
                                        cancelValidation();
                                        setValidated(false);
                                        setChangesMade(false);
                                    }}
                                    saveClick={() => {
                                        setChangesMade(false);
                                        if (creating) {
                                            createReq(null, formState.req);
                                        } else {
                                            updateReq(reqNumber, formState.req);
                                        }
                                    }}
                                    backClick={() => {
                                        navigate('/requisitions');
                                    }}
                                />
                                {changesMade && !validated && (
                                    <Stack direction="row" spacing={2}>
                                        <Chip
                                            label="Invalid to Save"
                                            color="error"
                                            size="small"
                                            variant="outlined"
                                        />
                                        <Typography variant="caption">
                                            {validToSaveMessage()}
                                        </Typography>
                                    </Stack>
                                )}
                            </Grid>
                            {pickRequisitionDialogVisible && (
                                <PickRequisitionDialog
                                    open={pickRequisitionDialogVisible}
                                    setOpen={setPickRequisitionDialogVisible}
                                    functionCode={formState.req.storesFunction?.code}
                                    documentNumber={formState.req.document1}
                                    documentType={formState.req.document1Name}
                                    handleSelect={reqDetails => {
                                        // BOOKLD doesn't specify an original req
                                        // so just fill out reversal details on the client
                                        if (formState.req?.storesFunction?.code === 'BOOKLD') {
                                            dispatch({
                                                type: 'set_reverse_details',
                                                payload: reqDetails
                                            });
                                        } else {
                                            // otherwise we can ask the server to fill out some details
                                            // that result from reversing the chosen req
                                            fetchReversalPreview(
                                                utilities
                                                    .getHref(reqDetails, 'preview-reversal')
                                                    ?.slice(1)
                                            );
                                        }
                                    }}
                                />
                            )}
                            {bookInPostingsDialogVisible && (
                                <BookInPostingsDialog
                                    open={bookInPostingsDialogVisible}
                                    setOpen={setBookInPostingsDialogVisible}
                                    documentNumber={formState.req.document1}
                                    documentLine={formState.req.document1Line}
                                    documentType={formState.req.document1Name}
                                    orderDetail={formState.document1Details.orderDetail}
                                    existingBookInOrderDetails={formState.req.bookInOrderDetails}
                                    isReverse={formState.req.isReverseTransaction}
                                    handleSelect={bookInPostings => {
                                        dispatch({
                                            type: 'set_book_in_postings',
                                            payload: bookInPostings
                                        });
                                    }}
                                />
                            )}
                        </>
                    )}
            </Grid>
            <Snackbar
                open={formState?.popUpMessage?.showMessage}
                autoHideDuration={7000}
                onClose={closeMessage}
            >
                <Alert
                    onClose={closeMessage}
                    severity={formState?.popUpMessage?.severity}
                    variant="filled"
                    sx={{ width: '100%' }}
                >
                    {formState?.popUpMessage?.text}
                </Alert>
            </Snackbar>
        </Page>
    );
}

export default Requisition;
