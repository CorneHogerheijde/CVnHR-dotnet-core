import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Settings';

class Settings extends Component {
    constructor(props) {
        super(props);
        this.updateApiKey = this.updateApiKey.bind(this);
    }

    componentWillMount() {
        this.props.requestSettings();
    }

    updateApiKey(e) {
        this.props.settings.apiKey = e.target.value;
    }

    render() {
        return (
            <div>
                <h1>Settings</h1>
                <h3>Certificates</h3>
                <p>
                    Currently installed certificate(s):
                </p>
                {this.props.isLoading && <div>Loading...</div>}
                {!this.props.isLoading &&
                    <>
                    
                        <ul>
                            {this.props.settings.certificates.length > 0
                                && this.props.settings.certificates.map(certificate =>
                                <li key={certificate}>{certificate}</li>
                                )}
                        </ul>
                        {this.props.settings.certificates.length === 0 &&
                            <p>No Certificate found!
                                Add pfx file in Certificates location if no certificate listed here.
                            </p>}
                        <p><strong>TODO: allow upload and edit of certificate</strong></p>

                        <p>KvK API key</p>
                        <input
                            type='text'
                            defaultValue={this.props.settings.apiKey}
                            onChange={this.updateApiKey}
                            />
                        <button onClick={() => this.props.updateApiKey(this.props.settings.apiKey)}>update</button>
                    </>
                }
            </div>
        );
    }
}

export default connect(
    state => state.settings,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Settings);