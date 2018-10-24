﻿const requestKvkSearchType = 'REQUEST_KVK_SEARCH';
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

        console.log(q);
        // TODO!

        /*const url = `api/SampleData/WeatherForecasts?startDateIndex=${startDateIndex}`;
        const response = await fetch(url);
        const forecasts = await response.json();*/
        const result = { item: 'bla' };
        

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
                result: {
                    result: action.result
                }
            },
            isLoading: false
        };
    }

    return state;
};