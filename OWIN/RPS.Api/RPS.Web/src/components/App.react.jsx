/** @jsx React.DOM */
var GameList = require('./GameList.react');
var CreateGameForm = require('./CreateGameForm.react');
var ScoreList = require('./ScoreList.react');
var React = require('react');

var App = React.createClass({

    componentDidMount: function() {
    },

    render: function() {
        return (
          <div className="rps-app">
            <GameList pollInterval="200" />
            <CreateGameForm />
            <ScoreList />
          </div>
      );
    }

});

module.exports = App;