import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { DictionaryLandmarkTypeList } from './components/DictionaryLandmarkTypeList';
import { SaveLandmark } from './components/SaveLandmark';
import { Counter } from './components/Counter';

export default class App extends Component {
  static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/counter' component={Counter} />
                <Route path='/fetch-data' component={FetchData} />
                <Route path='/list-data' component={DictionaryLandmarkTypeList} />
                <Route path='/save-data' component={SaveLandmark} />
            </Layout>
        );
    }
}
