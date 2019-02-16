import React, { Component } from 'react';
import { Route, BrowserRouter, Switch } from 'react-router-dom';
import './App.css';

class App extends Component {
  render() {
    return (
     <BrowserRouter>
        <Switch>
           <Route exact path="/" component={} />

        </Switch>
     </BrowserRouter>
    );
  }
}

export default App;
