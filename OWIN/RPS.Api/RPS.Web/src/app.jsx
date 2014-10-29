/** @jsx React.DOM */

var App = require('./components/App.react');
var GameWebAPIUtils = require('./utils/ServerUtil.js');
var React = require('react');

GameWebAPIUtils.getAvailableGames("/api/Games/available");

React.renderComponent(
  <App />,
  document.getElementById('rps')
);