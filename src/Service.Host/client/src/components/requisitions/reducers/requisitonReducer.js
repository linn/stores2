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
                } else if (state.storesFunction?.document1Name) {
                    return {
                        ...state,
                        document1Name: state.storesFunction?.document1Name,
                        document1: action.payload.newValue
                    };
                }
            } else if (action.payload.fieldName === 'storesFunction') {
                if (
                    action.payload.newValue?.nominalCode &&
                    action.payload.newValue?.nominalDescription
                ) {
                    return {
                        ...state,
                        storesFunction: action.payload.newValue,
                        nominal: {
                            nominalCode: action.payload.newValue?.nominalCode,
                            description: action.payload.newValue?.nominalDescription
                        }
                    };
                }
            }

            let newValue = action.payload.newValue;
            if (newValue?.length === 0) {
                newValue = null;
            }

            return { ...state, [action.payload.fieldName]: newValue };
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
                        transactionCodeDescription: lineTransactionType.transactionDescription,
                        stockAllocations: lineTransactionType.stockAllocations
                    };
                }
            } else if (storesFunctionTransactions && storesFunctionTransactions.length) {
                const lineTransactionType = storesFunctionTransactions[0];
                if (lineTransactionType) {
                    lineTransaction = {
                        transactionCode: lineTransactionType.transactionDefinition,
                        transactionCodeDescription: lineTransactionType.transactionDescription,
                        stockAllocations: lineTransactionType.stockAllocations
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
                                  ...action.payload.stockMoves.map((move, index) => ({
                                      seq: index + 1,
                                      part: move.partNumber,
                                      qty: move.quantityToPick,
                                      fromLocationCode: move.palletNumber
                                          ? null
                                          : move.locationName,
                                      fromLocationDescription: move.palletNumber
                                          ? null
                                          : move.locationDescription,
                                      fromPalletNumber: move.palletNumber,
                                      fromState: move.state,
                                      fromStockPool: move.stockPoolCode,
                                      fromBatchRef: move.batchRef,
                                      isFrom: state.reqType === 'O' ? false : true,
                                      isTo: state.reqType === 'F' ? false : true,
                                      fromBatchDate: move.stockRotationDate,
                                      qtyAtLocation: move.quantity,
                                      qtyAllocated: move.qtyAllocated,
                                      toStockPool:
                                          state.reqType === 'F'
                                              ? null
                                              : state.toStockPool
                                                ? state.toStockPool
                                                : move.stockPoolCode,
                                      toState:
                                          state.reqType === 'F'
                                              ? null
                                              : state.toState
                                                ? state.toState
                                                : move.state,
                                      toLocationCode:
                                          state.reqType === 'F'
                                              ? null
                                              : state.toLocationCode
                                                ? state.toLocationCode
                                                : null,
                                      toPalletNumber:
                                          state.reqType === 'F'
                                              ? null
                                              : state.toPalletNumber
                                                ? state.toPalletNumber
                                                : null
                                  }))
                              ]
                          }
                        : line
                )
            };
        case 'set_options_from_pick':
            if (action.payload) {
                return {
                    ...state,
                    fromState: action.payload.state,
                    fromStockPool: action.payload.stockPoolCode,
                    fromLocationId: action.payload.locationId,
                    fromLocationCode: action.payload.locationName,
                    fromPalletNumber: action.payload.palletNumber,
                    batchRef: action.payload.batchRef,
                    batchDate: action.payload.stockRotationDate,
                    toState: action.payload.state,
                    toStockPool: action.payload.stockPoolCode,
                    quantity: action.payload.quantityToPick
                };
            }

            return state;
        case 'add_move_onto':
            return {
                ...state,
                lines: state.lines.map(line =>
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
                                      isTo: true
                                  }
                              ]
                          }
                        : line
                )
            };
        case 'add_move':
            return {
                ...state,
                lines: state.lines.map(line =>
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
                                      isFrom: true,
                                      isTo: true
                                  }
                              ]
                          }
                        : line
                )
            };
        case 'update_move_onto':
            return {
                ...state,
                lines: state.lines.map(line => {
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
                              qty: updatedMoves.reduce((sum, item) => sum + item.qty, 0),
                              moves: updatedMoves
                          }
                        : line;
                })
            };
        default:
            return state;
    }
}

export default reducer;
