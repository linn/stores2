/* eslint-disable indent */
function reducer(state, action) {
    switch (action.type) {
        case 'load_state': {
            // this action type is for updating the entire state of the form,
            // e.g. to reflect an API result from an update or similar
            return action.payload;
        }
        case 'load_create': {
            // this action type initialses the state for when creating a new record
            const defaultState = {
                dateCreated: new Date(),
                dateAuthorised: null,
                lines: [],
                cancelled: 'N',
                createdByName: action.payload.userName,
                // just to make it easier to debug creating - delete everything below
                nominal: { nominalCode: 1607 },
                department: { departmentCode: 2963 },
                reqType: 'F'
            };
            return defaultState;
        }
        case 'set_header_value': {
            // this action type combines header field value updates, for the sake of brevity
            return { ...state, [action.payload.fieldName]: action.payload.newValue };
        }
        case 'add_line': {
            // need to set the line transaction type based on the function code and req type
            const functionCodeTransactions = state.functionCode.transactionTypes;
            let lineTransaction = {};
            if (state.reqType && functionCodeTransactions) {
                const lineTransactionType = functionCodeTransactions.find(
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

            return { ...state, lines: [...state.lines, newLine] };
        }
        case 'set_line_part':
            return {
                ...state,
                lines: state.lines.map(x =>
                    x.lineNumber === action.payload.lineNumber
                        ? {
                              ...x,
                              partNumber: action.payload.partNumber,
                              partDescription: action.payload.description
                          }
                        : x
                )
            };

        default: {
            return state;
        }
    }
}

export default reducer;
