import { add } from '../../../helpers/numberUtilities';

function reducer(state, action) {
    switch (action.type) {
        case 'clear': {
            return { req: null, popUpMessage: null, document1Details: null, partDetails: null };
        }
        case 'set_loan': {
            console.log('set_loan', action.payload);
            return { ...state, loan: action.payload };
        }
        case 'load_state': {
            // this action type is for updating the entire state of the form,
            // e.g. to reflect an API result from an update or similar
            const newState = action.payload;
            if (newState.workStationCode) {
                newState.document1Details = { workStationCode: action.payload.workStationCode };
            }

            if (newState.reqNumber) {
                return { ...state, req: newState, document1Details: newState.document1Details };
            } else {
                return { ...state, req: { ...state.req, links: newState.links } };
            }
        }
        case 'load_create': {
            // this action type initialses the state for when creating a new record
            return {
                req: {
                    dateCreated: new Date(),
                    dateAuthorised: null,
                    dateReceived: null,
                    dateBooked: null,
                    lines: [],
                    cancelled: 'N',
                    isReverseTransaction: 'N',
                    isReversed: 'N',
                    createdByName: action.payload.req?.userName,
                    createdBy: action.payload.req?.userNumber
                },
                popUpMessage: { showMessage: false },
                document1Details: null,
                partDetails: null
            };
        }
        case 'set_header_value': {
            // this action type combines header field value updates, for the sake of brevity
            if (action.payload.fieldName === 'document1') {
                if (state.req?.storesFunction?.document1Text == 'Loan Number') {
                    return {
                        ...state,
                        req: {
                            ...state.req,
                            document1Name: 'L',
                            document1: action.payload.newValue,
                            loanNumber: action.payload.newValue
                        }
                    };
                } else if (state.req.storesFunction?.document1Name) {
                    return {
                        ...state,
                        req: {
                            ...state.req,
                            document1Name: state.req.document1Name
                                ? state.req.document1Name
                                : state.req.storesFunction?.document1Name,
                            document1: action.payload.newValue
                        }
                    };
                }
            } else if (action.payload.fieldName === 'document2') {
                return {
                    ...state,
                    req: {
                        ...state.req,
                        document2Name: state.req.storesFunction?.document2Name,
                        document2: action.payload.newValue
                    }
                };
            } else if (action.payload.fieldName === 'document1Line') {
                const { storesFunction } = state.req;
                const isLoanSource = storesFunction?.partSource === 'L' && state.loan;

                let updatedReq = {
                    ...state.req,
                    document1Line: action.payload.newValue
                };

                if (isLoanSource) {
                    const loanLine = state.loan?.loanDetails?.find(
                        x => x.lineNumber === action.payload.newValue
                    );

                    if (loanLine) {
                        updatedReq.part = {
                            partNumber: loanLine.articleNumber
                        };
                        updatedReq.fromLocationCode = `A-LN-${state.loan.accountId}-${state.loan.outletNumber}`;
                    }
                }

                return {
                    ...state,
                    req: updatedReq
                };
            } else if (action.payload.fieldName === 'isReverseTransaction') {
                if (action.payload.newValue === 'N') {
                    return {
                        ...state,
                        req: {
                            ...state.req,
                            originalReqNumber: null,
                            quantity: null,
                            reference: null,
                            isReverseTransaction: action.payload.newValue
                        }
                    };
                } else {
                    return {
                        ...state,
                        req: {
                            ...state.req,
                            isReverseTransaction: action.payload.newValue
                        }
                    };
                }
            } else if (action.payload.fieldName === 'toLocationCode') {
                if (
                    action.payload.newValue &&
                    (action.payload.newValue.toUpperCase() === 'E-PARTS-FAIL' ||
                        action.payload.newValue.toUpperCase() === 'E-CAB-FAIL')
                ) {
                    return {
                        ...state,
                        popUpMessage: {
                            showMessage: true,
                            text: 'Have you filled out a part fail log for this? Please make sure every part moved here has one',
                            severity: 'warning'
                        },
                        req: {
                            ...state.req,
                            toLocationCode: action.payload.newValue
                        }
                    };
                } else {
                    return {
                        ...state,
                        req: {
                            ...state.req,
                            toLocationCode: action.payload.newValue
                        }
                    };
                }
            } else if (action.payload.fieldName === 'storesFunction') {
                let newState = {
                    ...state,
                    req: {
                        ...state.req,
                        storesFunction: action.payload.newValue
                    }
                };
                // set default reqType = F - From Stock for LDREQs
                // todo - maybe this applies for more function codes?
                if (action.payload.newValue.code === 'LDREQ') {
                    newState.req.reqType = 'F';
                }
                if (action.payload.newValue.manualPickRequired) {
                    const mapping = { M: 'Y', A: 'N', X: null };
                    newState.req.manualPick = mapping[action.payload.newValue.manualPickRequired];
                }

                if (
                    action.payload.newValue?.nominalCode &&
                    action.payload.newValue?.nominalDescription
                ) {
                    newState.req.nominal = {
                        nominalCode: action.payload.newValue?.nominalCode,
                        description: action.payload.newValue?.nominalDescription
                    };
                }

                if (action.payload.newValue?.defaultFromState) {
                    newState.req.fromState = action.payload.newValue?.defaultFromState;
                }

                if (action.payload.newValue?.defaultToState) {
                    newState.req.toState = action.payload.newValue?.defaultToState;
                }

                // set header from / to state if there is only one possibility for the given stores function
                if (action.payload.newValue.transactionTypes?.length === 1) {
                    if (!action.payload.newValue?.defaultFromState) {
                        // dont override if functions like MOVELOC don't want it
                        if (action.payload.newValue?.fromStateRequired !== 'N') {
                            newState.req.fromState =
                                action.payload.newValue.transactionTypes[0]?.fromStates?.[0];
                        }
                    }
                    if (!action.payload.newValue?.defaultToState) {
                        // dont override if functions like MOVELOC don't want it
                        if (action.payload.newValue?.toStateRequired !== 'N') {
                            newState.req.toState =
                                action.payload.newValue.transactionTypes[0]?.toStates?.[0];
                        }
                    }
                }

                if (action.payload.newValue?.toStockPool) {
                    newState.req.toStockPool = action.payload.newValue?.toStockPool;
                }

                return newState;
            }

            let newValue = action.payload.newValue;
            if (newValue?.length === 0) {
                newValue = null;
            }

            return { ...state, req: { ...state.req, [action.payload.fieldName]: newValue } };
        }
        case 'set_reverse_details': {
            if (action.payload.reqNumber || action.payload.originalReqNumber) {
                // action.payload is the filled out reversal req that the server returns
                return {
                    ...state,
                    req: {
                        ...state.req,
                        originalReqNumber:
                            action.payload.originalReqNumber ?? action.payload.reqNumber,
                        quantity: action.payload.quantity,
                        reference: action.payload.reference,
                        fromState: action.payload.fromState,
                        fromStockPool: action.payload.fromStockPool,
                        toStockPool: action.payload.toStockPool,
                        batchRef: action.payload.batchRef,
                        batchDate: action.payload.batchDate,
                        fromLocationId: action.payload.fromLocationId,
                        fromLocationCode: action.payload.fromLocationCode,
                        fromPalletNumber: action.payload.fromPalletNumber
                    }
                };
            } else {
                return {
                    ...state,
                    req: { ...state.req, originalReqNumber: null, quantity: null, reference: null }
                };
            }
        }
        case 'set_book_in_postings': {
            let originalReqNumber = null;
            if (
                action.payload.bookInOrderDetails?.length === 1 &&
                action.payload.bookInOrderDetails[0].isReverse === 'Y'
            ) {
                originalReqNumber = action.payload.bookInOrderDetails[0].originalReqNumber;
            } else {
                originalReqNumber = null;
            }

            return {
                ...state,
                req: {
                    ...state.req,
                    quantity: action.payload.quantityBooked,
                    originalReqNumber,
                    bookInOrderDetails: action.payload.bookInOrderDetails
                }
            };
        }
        case 'set_default_book_in_location': {
            if (!state.req.toLocationId && action.payload.locationId) {
                return {
                    ...state,
                    req: {
                        ...state.req,
                        toLocationId: action.payload.locationId,
                        toLocationCode: action.payload.locationCode
                    }
                };
            } else {
                return state;
            }
        }
        case 'set_document1_details': {
            const message = { showMessage: false };
            if (action.payload && state.req.storesFunction?.code === 'BOOKLD') {
                if (
                    action.payload.orderDetail?.orderPosting?.nominalAccount?.department
                        ?.projectDepartment === 'Y'
                ) {
                    message.showMessage = true;
                    message.severity = 'info';
                    message.text =
                        'This is a project department. Make a copy of the invoice if you have it';
                }
            }

            const newToStockPool = action.payload.orderDetail
                ? action.payload.orderDetail?.stockPoolCode
                : state.req.toStockPool;

            const doc1Details = action.payload;
            if (action.payload.orderDetail) {
                doc1Details.qtyOutstanding = action.payload.orderDetail.purchaseDeliveries.reduce(
                    (sum, item) => add(sum, item.quantityOutstanding),
                    0
                );
            }

            return {
                ...state,
                req: {
                    ...state.req,
                    document1Name: action.payload.docType,
                    part: {
                        partNumber: action.payload.partNumber,
                        description: action.payload.partDescription
                    },
                    document1Line: action.payload.document1Line,
                    toStockPool: newToStockPool
                },
                document1Details: action.payload,
                popUpMessage: message
            };
        }
        case 'set_part_details': {
            return { ...state, partDetails: action.payload };
        }
        case 'set_part_header_details_for_WO': {
            return {
                ...state,
                req: {
                    ...state.req,
                    toStockPool: action.payload.accountingCompany,
                    fromStockPool: action.payload.accountingCompany,
                    fromState: 'STORES',
                    toState: action.payload.qcOnReceipt === 'Y' ? 'QC' : 'STORES'
                }
            };
        }
        case 'set_part_header_details_for_PO': {
            const newState =
                state.req.storesFunction?.code === 'BOOKSU'
                    ? action.payload.qcOnReceipt === 'Y'
                        ? 'QC'
                        : 'STORES'
                    : state.req.toState;
            return {
                ...state,
                req: {
                    ...state.req,
                    toState: newState,
                    unitOfMeasure: action.payload.ourUnitOfMeasure
                }
            };
        }
        case 'add_line': {
            // need to set the line transaction type based on the function code and req type
            const storesFunctionTransactions = state.req.storesFunction.transactionTypes;
            let lineTransaction = {};
            var transactionReqType = '';
            var headerFromState = null;
            var headerToState = null;
            let lineTransactionType = null;
            if (state.req.reqType && storesFunctionTransactions) {
                lineTransactionType = storesFunctionTransactions.find(
                    x => x.reqType === state.req.reqType
                );
                if (lineTransactionType) {
                    lineTransaction = {
                        transactionCode: lineTransactionType.transactionDefinition,
                        transactionCodeDescription: lineTransactionType.transactionDescription,
                        stockAllocations: lineTransactionType.stockAllocations
                    };
                }
            } else if (storesFunctionTransactions && storesFunctionTransactions.length) {
                lineTransactionType = storesFunctionTransactions[0];
                transactionReqType = lineTransactionType.reqType;
                if (lineTransactionType) {
                    lineTransaction = {
                        transactionCode: lineTransactionType.transactionDefinition,
                        transactionCodeDescription: lineTransactionType.transactionDescription,
                        stockAllocations: lineTransactionType.stockAllocations
                    };
                }
            }

            headerFromState =
                lineTransactionType?.fromStates?.length === 1
                    ? lineTransactionType.fromStates[0]
                    : null;
            headerToState =
                lineTransactionType?.toStates?.length === 1 && !state.req.toState
                    ? lineTransactionType.toStates[0]
                    : null;

            // use the next available line number
            const maxLineNumber = state.req.lines?.length
                ? Math.max(...state.req.lines.map(line => line.lineNumber), 0)
                : 0;
            const newLine = { lineNumber: maxLineNumber + 1, isAddition: true, ...lineTransaction };

            // this behaviour might differ for differing function code parameters
            // but for now...
            newLine.document1Type = lineTransactionType?.document1Type ?? 'REQ';
            newLine.document1Line = newLine.document1Type === 'REQ' ? newLine.lineNumber : null;
            newLine.document1Number = state.req.document1;
            return {
                ...state,
                req: {
                    ...state.req,
                    reqType: state.req.reqType ?? transactionReqType,
                    lines: [...state.req.lines, newLine],
                    // set header states if dictated by new line transaction type,
                    fromState: headerFromState ?? state.req.fromState,
                    toState: headerToState ?? state.req.toState
                }
            };
        }
        case 'remove_line': {
            const newState = { ...state };
            const toRemove = state.req.lines.find(
                x => x.lineNumber === action.payload && x.isAddition
            );
            if (toRemove) {
                newState.req.lines = state.req.lines.filter(x => x.lineNumber !== action.payload);
            }
            return newState;
        }
        case 'set_line_value':
            if (action.payload.fieldName === 'transactionCode') {
                const options = state.req.storesFunction.transactionTypes;
                const selected = options.find(
                    a => a.transactionDefinition === action.payload.newValue
                );

                if (selected) {
                    return {
                        ...state,
                        req: {
                            ...state.req,
                            lines: state.req.lines.map(x =>
                                x.lineNumber === action.payload.lineNumber
                                    ? {
                                          ...x,
                                          transactionCode: action.payload.newValue,
                                          transactionCodeDescription:
                                              selected.transactionDescription
                                      }
                                    : x
                            )
                        }
                    };
                }

                return state;
            } else {
                return {
                    ...state,
                    req: {
                        ...state.req,
                        lines: state.req.lines.map(x =>
                            x.lineNumber === action.payload.lineNumber
                                ? {
                                      ...x,
                                      [action.payload.fieldName]: action.payload.newValue
                                  }
                                : x
                        )
                    }
                };
            }
        case 'pick_stock':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line =>
                        line.lineNumber === action.payload.lineNumber
                            ? {
                                  ...line,
                                  qty: action.payload.stockMoves.reduce(
                                      (sum, move) => add(sum, move.quantityToPick),
                                      0
                                  ),
                                  stockPicked: action.payload.stockMoves.some(
                                      a => a.palletNumber || a.locationName
                                  ),
                                  // todo - simplification: following line assumes stock can only be picked once for each line
                                  // so will need to make this able to cope with subsequent changes at some point
                                  moves: [
                                      ...action.payload.stockMoves.map((move, index) => ({
                                          seq: index + 1,
                                          part: move.partNumber,
                                          qty: move.quantityToPick,
                                          fromLocationCode:
                                              move.isFrom === false || state.req.reqType === 'O'
                                                  ? null
                                                  : move.palletNumber
                                                    ? null
                                                    : move.locationName,
                                          fromLocationDescription:
                                              move.isFrom === false || state.req.reqType === 'O'
                                                  ? null
                                                  : move.palletNumber
                                                    ? null
                                                    : move.locationDescription,
                                          fromPalletNumber:
                                              move.isFrom === false || state.req.reqType === 'O'
                                                  ? null
                                                  : move.palletNumber,
                                          fromState:
                                              move.isFrom === false || state.req.reqType === 'O'
                                                  ? null
                                                  : move.state,
                                          fromStockPool:
                                              move.isFrom === false || state.req.reqType === 'O'
                                                  ? null
                                                  : move.stockPoolCode,
                                          fromBatchRef: move.batchRef,
                                          isFrom:
                                              move.isFrom ??
                                              (state.req.reqType === 'O' ? false : true),
                                          isTo:
                                              move.isTo ??
                                              (state.req.reqType === 'F' ? false : true),
                                          fromBatchDate: move.stockRotationDate,
                                          qtyAtLocation: move.quantity,
                                          qtyAllocated: move.qtyAllocated,
                                          toStockPool:
                                              move.isTo === false || state.req.reqType === 'F'
                                                  ? null
                                                  : state.req.toStockPool
                                                    ? state.req.toStockPool
                                                    : move.stockPoolCode,
                                          toState:
                                              move.isTo === false || state.req.reqType === 'F'
                                                  ? null
                                                  : state.req.toState
                                                    ? state.req.toState
                                                    : move.state,
                                          toLocationCode:
                                              move.isTo === false || state.req.reqType === 'F'
                                                  ? null
                                                  : state.req.toLocationCode
                                                    ? state.req.toLocationCode
                                                    : null,
                                          toPalletNumber:
                                              move.isTo === false || state.req.reqType === 'F'
                                                  ? null
                                                  : state.req.toPalletNumber
                                                    ? state.req.toPalletNumber
                                                    : null
                                      }))
                                  ]
                              }
                            : line
                    )
                }
            };
        case 'set_options_from_pick':
            if (action.payload) {
                return {
                    ...state,
                    req: {
                        ...state.req,
                        fromState: action.payload.state,
                        fromStockPool: action.payload.stockPoolCode,
                        fromLocationId: action.payload.locationId,
                        fromLocationCode: action.payload.locationName,
                        fromPalletNumber: action.payload.palletNumber,
                        batchRef: action.payload.batchRef,
                        batchDate: action.payload.stockRotationDate,
                        fromCategory: action.payload.category,
                        toStockPool: action.payload.stockPoolCode,
                        quantity: action.payload.quantityToPick
                    }
                };
            }

            return state;
        case 'set_audit_location_details':
            if (action.payload?.storagePlace) {
                let location = null;
                let palletNumber = null;
                if (action.payload.palletLocationOrArea === 'P') {
                    palletNumber = action.payload.storagePlace.substring(1);
                } else if (action.payload.palletLocationOrArea === 'L') {
                    location = action.payload.storagePlace;
                }

                return {
                    ...state,
                    req: {
                        ...state.req,
                        auditLocation: action.payload.storagePlace,
                        fromPalletNumber: palletNumber,
                        toPalletNumber: palletNumber,
                        fromLocationCode: location,
                        toLocationCode: location,
                        toStockPool: 'LINN',
                        fromStockPool: 'LINN'
                    },
                    auditLocationDetails: action.payload
                };
            } else {
                return {
                    ...state,
                    req: {
                        ...state.req,
                        auditLocation: null,
                        fromPalletNumber: null,
                        toPalletNumber: null,
                        fromLocationCode: null,
                        toLocationCode: null
                    },
                    auditLocationDetails: null
                };
            }
        case 'add_move_onto':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line =>
                        line.lineNumber === action.payload.lineNumber
                            ? {
                                  ...line,
                                  moves: [
                                      ...(line.moves ? line.moves : []),
                                      {
                                          lineNumber: action.payload.lineNumber,
                                          seq: line.moves ? line.moves.length + 1 : 1,
                                          toStockPool: 'LINN',
                                          toState: 'STORES',
                                          part: line.part.partNumber,
                                          isTo: true,
                                          isAddition: true,
                                          qty: line.qty
                                      }
                                  ]
                              }
                            : line
                    )
                }
            };
        case 'add_move':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line =>
                        line.lineNumber === action.payload.lineNumbto
                            ? {
                                  ...line,
                                  moves: [
                                      ...(line.moves ? line.moves : []),
                                      {
                                          lineNumber: action.payload.lineNumber,
                                          seq: line.moves ? line.moves.length + 1 : 1,
                                          toStockPool: 'LINN',
                                          toState: 'STORES',
                                          part: line.part.partNumber,
                                          isFrom: true,
                                          isTo: true
                                      }
                                  ]
                              }
                            : line
                    )
                }
            };
        case 'update_move_onto':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line => {
                        const updatedMoves = line.moves.map(m =>
                            m.seq === action.payload.seq
                                ? {
                                      ...action.payload
                                  }
                                : m
                        );
                        return line.lineNumber === action.payload.lineNumber
                            ? {
                                  ...line,
                                  qty: updatedMoves.reduce((sum, item) => add(sum, item.qty), 0),
                                  moves: updatedMoves
                              }
                            : line;
                    })
                }
            };
        case 'add_serial_number':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line =>
                        line.lineNumber === action.payload.lineNumber
                            ? {
                                  ...line,
                                  serialNumbers: [
                                      ...(line.serialNumbers ? line.serialNumbers : []),
                                      {
                                          seq: line.serialNumbers
                                              ? line.serialNumbers.length + 1
                                              : 1,
                                          serialNumber: null
                                      }
                                  ]
                              }
                            : line
                    )
                }
            };
        case 'update_serial_number':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line => {
                        const updatedSernos = line.serialNumbers.map(m =>
                            m.seq === action.payload.seq
                                ? {
                                      ...action.payload
                                  }
                                : m
                        );
                        return line.lineNumber === action.payload.lineNumber
                            ? {
                                  ...line,
                                  serialNumbers: updatedSernos
                              }
                            : line;
                    })
                }
            };
        case 'delete_serial_number':
            return {
                ...state,
                req: {
                    ...state.req,
                    lines: state.req.lines.map(line => {
                        return line.lineNumber === action.payload.lineNumber
                            ? {
                                  ...line,
                                  serialNumbers: line.serialNumbers.filter(
                                      s => s.seq !== action.payload.sernosSeq
                                  )
                              }
                            : line;
                    })
                }
            };
        case 'close_message':
            return {
                ...state,
                popUpMessage: { showMessage: false, text: '', severity: 'info' }
            };
        default:
            return state;
    }
}

export default reducer;
