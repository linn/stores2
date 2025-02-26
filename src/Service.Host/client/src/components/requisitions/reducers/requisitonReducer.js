function reducer(state, action) {
    switch (action.type) {
        case 'clear': {
            return null;
        }
        case 'load_state': {
            // this action type is for updating the entire state of the form,
            // e.g. to reflect an API result from an update or similar
            return action.payload;
        }
        case 'load_create': {
            // this action type initialses the state for when creating a new record
            return {
                dateCreated: new Date(),
                dateAuthorised: null,
                dateBooked: null,
                lines: [],
                cancelled: 'N',
                createdByName: action.payload.userName
            };
        }
        case 'set_header_value': {
            // this action type combines header field value updates, for the sake of brevity
            if (action.payload.fieldName === 'document1') {
                if (state.storesFunction?.document1Text == 'Loan Number') {
                    return {
                        ...state,
                        document1Name: 'L',
                        document1: action.payload.newValue,
                        loanNumber: action.payload.newValue
                    };
                }
            }

            return { ...state, [action.payload.fieldName]: action.payload.newValue };
        }
        case 'add_line': {
            // need to set the line transaction type based on the function code and req type
            const storesFunctionTransactions = state.storesFunction.transactionTypes;
            let lineTransaction = {};
            if (state.reqType && storesFunctionTransactions) {
                const lineTransactionType = storesFunctionTransactions.find(
                    x => x.reqType === state.reqType
                );
                if (lineTransactionType) {
                    lineTransaction = {
                        transactionCode: lineTransactionType.transactionDefinition,
                        transactionCodeDescription: lineTransactionType.transactionDescription
                    };
                }
            }

            // use the next available line number
            const maxLineNumber = Math.max(...state.lines.map(line => line.lineNumber), 0);
            const newLine = { lineNumber: maxLineNumber + 1, isAddition: true, ...lineTransaction };

            // this behaviour might differ for differing function code parameters
            // but for now...
            newLine.document1Type = 'REQ';
            newLine.document1Line = newLine.lineNumber;

            return { ...state, lines: [...state.lines, newLine] };
        }
        case 'set_line_value':
            return {
                ...state,
                lines: state.lines.map(x =>
                    x.lineNumber === action.payload.lineNumber
                        ? {
                              ...x,
                              [action.payload.fieldName]: action.payload.newValue
                          }
                        : x
                )
            };
        case 'pick_stock':
            return {
                ...state,
                lines: state.lines.map(line =>
                    line.lineNumber === action.payload.lineNumber
                        ? {
                              ...line,
                              qty: action.payload.stockMoves.reduce(
                                  (sum, move) => sum + move.quantityToPick,
                                  0
                              ),
                              stockPicked: true,
                              // todo - simplification: following line assumes stock can only be picked once for each line
                              // so will need to make this able to cope with subsequent changes at some point
                              moves: [
                                  ...action.payload.stockMoves.map(
                                      (move, index) =>
                                          state.reqType === 'F' || !state.reqType
                                              ? {
                                                    seq: index + 1,
                                                    part: move.partNumber,
                                                    qty: move.quantityToPick,
                                                    from: {
                                                        seq: index + 1,
                                                        locationCode: move.locationName,
                                                        locationDescription:
                                                            move.locationDescription,
                                                        palletNumber: move.palletNumber,
                                                        state: move.state,
                                                        batchRef: move.batchRef,
                                                        batchDate: move.stockRotationDate,
                                                        qtyAtLocation: move.quantity,
                                                        qtyAllocated: move.qtyAllocated
                                                    }
                                                }
                                              : {} // todo - behaviour for reqs onto
                                  )
                              ]
                          }
                        : line
                )
            };
        case 'set_options_from_pick': {
            if (action.payload) {
                return {
                    ...state,
                    fromState: action.payload.state,
                    fromStockPool: action.payload.stockPoolCode,
                    fromLocationId: action.payload.locationId,
                    fromLocationCode: action.payload.locationName,
                    fromPalletNumber: action.payload.palletNumber,
                    toState: action.payload.state,
                    toStockPool: action.payload.stockPoolCode,
                    quantity: action.payload.quantityToPick
                };
            }

            return state;
        }
        default: {
            return state;
        }
    }
}

export default reducer;
