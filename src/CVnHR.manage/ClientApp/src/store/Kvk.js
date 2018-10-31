const requestKvkSearchType = 'REQUEST_KVK_SEARCH';
const receiveKvkSearchType = 'RECEIVE_KVK_SEARCH';
const initialState = {
    kvk: {
        q: null,
        result: {},
        startPage: null
    },
    isLoading: false
};

export const actionCreators = {
    search: (q, startPage) => async (dispatch, getState) => {
        const state = getState();
        
        if (state.isLoading || (q === state.kvk.q && startPage === state.kvk.startPage)) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        dispatch({ type: requestKvkSearchType, q });
        
        const url = `api/kvk?q=${q}&startPage=${startPage || 1}`;
        const response = await fetch(url);
        const result = await response.json();

        dispatch({ type: receiveKvkSearchType, q, result });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === requestKvkSearchType) {
        return {
            ...state,
            q: action.q,
            kvk: {
                ...state.kvk,
            },
            isLoading: true
        };
    }

    if (action.type === receiveKvkSearchType) {
        return {
            ...state,
            kvk: {
                ...state.kvk,
                result: action.result,
                startPage: action.result.startPage
            },
            isLoading: false
        };
    }

    return state;
};