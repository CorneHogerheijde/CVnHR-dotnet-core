import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Settings';

class Settings extends Component {
    constructor(props) {
        super(props);
        this.updateApiKey = this.updateApiKey.bind(this);
        this.updateBaseUrl = this.updateBaseUrl.bind(this);
        this.updateSearchUrl = this.updateSearchUrl.bind(this);
        this.updateProfileUrl = this.updateProfileUrl.bind(this);
    }

    componentWillMount() {
        this.props.requestSettings();
    }

    updateApiKey(e) {
        this.props.settings.kvkApiSettings.apiKey = e.target.value;
    }

    updateBaseUrl(e) {
        this.props.settings.kvkApiSettings.baseUrl = e.target.value;
    }

    updateSearchUrl(e) {
        this.props.settings.kvkApiSettings.searchUrl = e.target.value;
    }

    updateProfileUrl(e) {
        this.props.settings.kvkApiSettings.profileUrl = e.target.value;
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

                    <label>
                        KvK API key
                        <input
                            type='text'
                            defaultValue={this.props.settings.kvkApiSettings.apiKey}
                            onChange={this.updateApiKey}
                        />
                    </label>
                    <label>
                        Kvk base url
                        <input
                            type='text'
                            defaultValue={this.props.settings.kvkApiSettings.baseUrl}
                            onChange={this.updateBaseUrl}
                        />
                    </label>
                    <label>
                        Kvk search url
                        <input
                            type='text'
                            defaultValue={this.props.settings.kvkApiSettings.searchUrl}
                            onChange={this.updateSearchUrl}
                        />
                    </label>
                    <label>
                        Kvk profile url
                        <input
                            type='text'
                            defaultValue={this.props.settings.kvkApiSettings.profileUrl}
                            onChange={this.updateProfileUrl}
                        />
                    </label>
                    <button onClick={() => this.props.updateApiSettings(this.props.settings.kvkApiSettings)}>update settings</button>
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