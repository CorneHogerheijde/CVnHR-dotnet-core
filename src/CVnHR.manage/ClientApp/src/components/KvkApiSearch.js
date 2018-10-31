import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Kvk';
import KvkApiSearchResultTable from './KvkApiSearchResultTable';
import qs from 'query-string';

class KvkApiSearch extends Component {
    constructor(props) {
        super(props);
        this.search = this.search.bind(this);

        if (props.q) {
            props.kvk.q = props.q;
            props.search(props.kvk.q, props.kvk.startPage);
        }
    }

    componentDidUpdate() { // componenentWillUpdate(nextProps) etc.. ??
        const { startPage } = qs.parse(this.props.qs);
        console.log(startPage, this.props.kvk.startPage);
        if (startPage && startPage !== this.props.kvk.startPage) {
            //this.props.search(this.props.kvk.q, startPage);
            console.log('change!');
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
                <button onClick={() => this.props.search(this.props.kvk.q, this.props.kvk.startPage)}>search</button>

                {this.props.isLoading && <div>Loading...</div>}
                {this.props.kvk.result.items
                    && <KvkApiSearchResultTable kvkApiResult={this.props.kvk.result} />}
            </>
        );
    }
}

export default connect(
    state => ({
        ...state.kvk,
        qs: state.router.location.search,
        isLoading: state.kvk.isLoading
    }),    
    dispatch => bindActionCreators(actionCreators, dispatch)
)(KvkApiSearch);