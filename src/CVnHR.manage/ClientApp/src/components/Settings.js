import React from 'react';
import { connect } from 'react-redux';

const Settings = props => (
    <div>
        <h1>Settings</h1>
        <h2>Certificates</h2>
        <p>
            Add your certificates here.
        </p>
    </div>
);

export default connect()(Settings);