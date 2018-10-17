import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Kvk';

class KvkApiSearch extends Component {
    constructor(props) {
        super(props);
        this.search = this.search.bind(this);
    }

    search(e) {
        this.props.kvk.q = e.target.value;
    }

    render() {
        return (
            <>
                <input
                    type='text'
                    defaultValue={this.props.kvk.q}
                    onChange={this.search}
                />
                <button onClick={() => this.props.search(this.props.kvk.q)}>search</button>
            </>
        );
    }
}

export default connect(
    state => state.kvk,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(KvkApiSearch);