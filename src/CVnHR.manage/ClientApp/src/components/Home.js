import React from 'react';
import { connect } from 'react-redux';
import KvkApiSearch from './KvkApiSearch';

const Home = props => 
  <div>
        <h1>Zoek bij KvK</h1>
        <KvkApiSearch />
  </div>
;

export default connect()(Home);
