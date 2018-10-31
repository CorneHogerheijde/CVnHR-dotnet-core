import React from 'react';
import { connect } from 'react-redux';
import KvkApiSearch from '../components/KvkApiSearch';

const Home = props =>
    <div>
        <h1>Zoek bij KvK</h1>
        <KvkApiSearch q={props.match.params.q} />
    </div>
    ;

export default connect()(Home);