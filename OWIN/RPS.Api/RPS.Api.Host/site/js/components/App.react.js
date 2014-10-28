/** @jsx React.DOM */
var GameList = require('./GameList.react');
var React = require('react');

var App = React.createClass({

    componentDidMount: function() {
    },

    render: function() {
        return (
          <div className="rps-app">
            <GameList />
          </div>
      );
    }

});

module.exports = App;