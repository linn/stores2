/* eslint-disable indent */
function reducer(state, action) {
    console.log(action);
    switch (action.type) {
        case 'reload': {
            return action.payload;
        }
        case 'set_header_field': {
            // simple primitive field value updates are combined into one action type
            return { ...state, [action.payload.fieldName]: action.payload.newValue };
        }
        case 'update_header_department': {
            return { ...state, department: action.payload };
        }
        case 'update_header_nominal': {
            return { ...state, nominal: action.payload };
        }
        default: {
            return state;
        }
    }
}

export default reducer;
