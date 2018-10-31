import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Kvk';
import KvkApiSearchResultTable from './KvkApiSearchResultTable';
import queryString from 'query-string';
import withRouter from '../common/queryWithRouter';

class KvkApiSearch extends Component {
    constructor(props) {
        super(props);
        this.search = this.search.bind(this);

        if (props.q) {
            props.kvk.q = props.q;
            props.search(props.kvk.q);
        }
    }

    componentDidUpdate(prevProps, prevState) {
        console.log('componentDidUpdate: ', this.props.location.query.startPage);
        console.log(prevProps);

        const { startPage } = this.props.location.query || { startPage: null };

        console.log(startPage);

        if (startPage && startPage !== prevProps.kvk.startPage) {
            /*this.props.search(this.props.kvk.q);*/
            console.log(startPage, prevProps.kvk.startPage);

            // TODO: do the search
        }
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
                    onKeyUp={(e) => e.keyCode === 13 ? this.props.search(this.props.kvk.q) : false}
                />
                <button onClick={() => this.props.search(this.props.kvk.q)}>search</button>

                {this.props.isLoading && <div>Loading...</div>}
                {this.props.kvk.result.items
                    && <KvkApiSearchResultTable kvkApiResult={this.props.kvk.result} />}
            </>
        );
    }
}

export default withRouter(connect(
    state => state.kvk,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(KvkApiSearch));