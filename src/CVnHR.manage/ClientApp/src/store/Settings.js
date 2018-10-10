const requestSettingsType = 'REQUEST_SETTINGS';
const receiveSettingsType = 'RECEIVE_SETTINGS';
const updateApiKeyType = 'UPDATE_APIKEY';
const apiKeyUpdatedType = 'APIKEY_UPDATED';

const initialState = {
    settings: {
        certificates: [],
        apiKey: null
    },
    isLoading: false
};

export const actionCreators = {
    requestSettings: () => async (dispatch, getState) => {

        dispatch({ type: requestSettingsType });

        const url = `api/Settings/GetSettings`;
        const response = await fetch(url);
        const settings = await response.json();

        dispatch({ type: receiveSettingsType, settings });
    },
    updateApiKey: (apiKey) => async (dispatch, getState) => {
        const url = `api/Settings/UpdateApiKey`;
        const response = await fetch(url, {
            method: 'PUT',
            body: JSON.stringify(apiKey),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        dispatch({ type: apiKeyUpdatedType, success: response.ok, apiKey });
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
            isLoading: false
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
                apiKey: action.apiKey
            }
        };
    }

    return state;
};
