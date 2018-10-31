import { push } from 'connected-react-router';

const requestKvkSearchType = 'REQUEST_KVK_SEARCH';
const receiveKvkSearchType = 'RECEIVE_KVK_SEARCH';
const initialState = {
    kvk: {
        q: null,
        result: {},
        startPage: 1
    },
    isLoading: false
};

export const actionCreators = {
    search: q => async (dispatch, getState) => {
        const state = getState();
        const { startPage } = state.router.location.query || { startPage: 1 };

        if (q === state.kvk.q && startPage === state.kvk.startPage) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }
        
        dispatch(push(`/search/${q}${state.router.location.query? `?startPage=${startPage}`: ''}`));

        dispatch({ type: requestKvkSearchType, q });

        const url = `api/kvk?q=${q}&startPage=${startPage}`;
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
                result: action.result
            },
            isLoading: false
        };
    }

    return state;
};