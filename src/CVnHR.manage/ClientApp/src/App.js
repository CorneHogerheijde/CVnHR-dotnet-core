import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './containers/Home';
import Counter from './containers/Counter';
import FetchData from './containers/FetchData';
import Settings from './containers/Settings';
import Search from './containers/Search';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/search/:q?' component={Search} />
    <Route path='/counter' component={Counter} />
    <Route path='/fetchdata/:startDateIndex?' component={FetchData} />
    <Route path='/settings' component={Settings} />
  </Layout>
);
