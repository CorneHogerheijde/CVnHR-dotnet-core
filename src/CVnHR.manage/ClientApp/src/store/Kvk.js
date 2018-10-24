const requestKvkSearchType = 'REQUEST_KVK_SEARCH';
const receiveKvkSearchType = 'RECEIVE_KVK_SEARCH';
const initialState = {
    q: null,
    kvk: {
        q: null,
        result: {  }
    },
    isLoading: false
};

export const actionCreators = {
    search: q => async (dispatch, getState) => {
        const state = getState();
        if (q === state.kvk.q) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        dispatch({ type: requestKvkSearchType, q });

        const url = `api/kvk?q=${q}`;
        const response = await fetch(url);
        const result = await response.json();

        console.log(result);

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
                result: action.result
            },
            isLoading: false
        };
    }

    return state;
};