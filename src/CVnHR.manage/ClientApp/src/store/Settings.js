const requestSettingsType = 'REQUEST_SETTINGS';
const receiveSettingsType = 'RECEIVE_SETTINGS';
const apiKeyUpdatedType = 'APIKEY_UPDATED';

const initialState = {
    settings: {
        certificates: [],
        kvkApiSettings: {}
    },
    isLoading: false,
    dataLoaded: false
};

export const actionCreators = {
    requestSettings: () => async (dispatch, getState) => {
        var state = getState();
        if (state.settings.isLoading || state.settings.dataLoaded) {
            // Don't issue a duplicate  request (we already have or are loading the requested data)
            return;
        }

        dispatch({ type: requestSettingsType });

        const url = `api/Settings/GetSettings`;
        const response = await fetch(url);
        const settings = await response.json();

        dispatch({ type: receiveSettingsType, settings });
    },
    updateApiSettings: (kvkApiSettings) => async (dispatch, getState) => {
        const url = `api/Settings/UpdateKvkApiSettings`;
        const response = await fetch(url, {
            method: 'PUT',
            body: JSON.stringify(kvkApiSettings),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        dispatch({ type: apiKeyUpdatedType, success: response.ok, kvkApiSettings });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === requestSettingsType) {
        return {
            ...state,
            isLoading: true
        };
    }

    if (action.type === receiveSettingsType) {
        return {
            ...state,
            settings: action.settings,
            isLoading: false,
            dataLoaded: true
        };
    }

    if (action.type === apiKeyUpdatedType) {
        if (!action.success) {
            alert('something went wrong!');
            return state;
        }
        return {
            ...state,
            settings: {
                ...state.settings,
                kvkApiSettings: action.kvkApiSettings
            }
        };
    }

    return state;
};
