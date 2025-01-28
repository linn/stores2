/* eslint-disable indent */
function reducer(state, action) {
    switch (action.type) {
        case 'reload': {
            return action.payload;
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
