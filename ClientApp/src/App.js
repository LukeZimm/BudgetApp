import React, { useState, Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Home from './Pages/Home';
import SignUp from './Pages/SignUp';
import Site from './Pages/Site';
import './custom.css'

const App = () => {
  const displayName = App.name;

  return (
    <Layout>
      <Route exact path='/' component={Home} />
      <Route path='/sign-up' component={SignUp} />
      <Route path='/site' component={Site} />
    </Layout>
  );
}

export default App;