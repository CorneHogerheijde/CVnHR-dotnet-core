import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Settings';

class Settings extends Component {
    componentWillMount() {
        this.props.requestCertificates();
    }

    render() {
        return (
            <div>
                <h1>Settings</h1>
                <h3>Certificates</h3>
                <p>
                    Current installed certificate(s):
                </p>

                <ul>
                    {this.props.settings.certificates.length > 0
                        && this.props.settings.certificates.map(certificate =>
                           <li>{certificate}</li>
                        )}
                </ul>
                {this.props.settings.certificates.length == 0 &&
                    <p>No Certificate found!
                        Add pfx file in Certificates location if no certificate listed here.
                    </p>}
                <p><strong>TODO: allow upload and edit of certificate</strong></p>
            </div>
        );
    }
}

export default connect(
    state => state.settings,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Settings);