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
                createdByName: action.payload.userName
            };
            return defaultState;
        }
        case 'set_header_value': {
            // this action type combines header field value updates, for the sake of brevity
            return { ...state, [action.payload.fieldName]: action.payload.newValue };
        }
        case 'add_line': {
            const maxLineNumber = Math.max(...state.lines.map(line => line.lineNumber), 0);
            const newLine = { lineNumber: maxLineNumber + 1, isAddition: true };
            return { ...state, lines: [...state.lines, newLine] };
        }
        default: {
            return state;
        }
    }
}

export default reducer;
