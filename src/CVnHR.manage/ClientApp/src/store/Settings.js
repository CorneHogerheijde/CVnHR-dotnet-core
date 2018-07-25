const requestCertificatesType = 'REQUEST_CERTIFICATES';
const receiveCertificatesType = 'RECEIVE_CERTIFICATES';
const initialState = {
    settings: {
        certificates: []
    },
    isLoading: false
};

export const actionCreators = {
    requestCertificates: () => async (dispatch, getState) => {

        dispatch({ type: requestCertificatesType });

        const url = `api/Settings/GetCertificates`;
        const response = await fetch(url);
        const certificates = await response.json();

        dispatch({ type: receiveCertificatesType, certificates });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === requestCertificatesType) {
        return {
            ...state,
            isLoading: true
        };
    }

    if (action.type === receiveCertificatesType) {
        return {
            ...state,
            settings: {
                certificates: action.certificates
            },
            isLoading: false
        };
    }

    return state;
};
