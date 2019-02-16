import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { routerReducer, routerMiddleware } from 'react-router-redux';

import * as localForage from "localforage";


export default async function configureStore(history, initialState) {
  const reducers = {
    

  };

  const logger = store => next => action => {
    let result;
    console.groupCollapsed("dispatching", action.type)
    console.log('prev state', store.getState())
    console.log('action', action)
    result = next(action)
    console.log('next state', store.getState())
    console.groupEnd()
  };

  const saver = store => next => action => {
    let result = next(action)

      localForage.setItem("escrow", store.getState()).
        then(function (value) {
          // console.log(value);
        }).catch(function (err) {
          // console.log({ forageError: err });
        });
      
    
    return result
  };

  const middleware = [
    thunk,
    logger,
    saver,
    routerMiddleware(history)
  ];

  // In development, use the browser's Redux dev tools extension if installed
  const enhancers = [];
  const isDevelopment = process.env.NODE_ENV === 'development';
  if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
    enhancers.push(window.devToolsExtension());
  }

  const rootReducer = combineReducers({
    ...reducers,
    routing: routerReducer
  });


  let initial = undefined

  try{
  initial = await localForage.getItem("chidelu");
  if(initial== null)
  initial = undefined;
  // console.log({initial})
  }
  catch(err){
    // console.log({err})
    initial = undefined;
  }
   


  
  return createStore(
    rootReducer,
    initial,
    compose(applyMiddleware(...middleware), ...enhancers)
  );
}
