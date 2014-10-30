/** @jsx React.DOM */
var React = require('react');
var GameActionCreators = require('../actions/GameActionCreators');

var CreateGameForm = React.createClass({
  handleSubmit: function(e) {
    e.preventDefault();
    var gameName = this.refs.gameName.getDOMNode().value.trim();
    var playerName = this.refs.playerName.getDOMNode().value.trim();
    var move = this.refs.move.getDOMNode().value.trim();

    if (!gameName || !playerName || !move) {
      return;
    }
	GameActionCreators.createGame(gameName, playerName, move);
    
	this.refs.gameName.getDOMNode().value = '';
    this.refs.playerName.getDOMNode().value = '';
    this.refs.move.getDOMNode().value = '';

    return;
  },
  render: function() {
    return (
	<div>
	  <h3>Create</h3>	
	  <form className="game-form" onSubmit={this.handleSubmit}>
        <input type="text" placeholder="Game Name" ref="gameName" />
        <input type="text" placeholder="Your name" ref="playerName" />
        <input type="text" placeholder="Your move" ref="move" />
        <input type="submit" value="Post" />
      </form>
	</div>
    );
  }
});

module.exports = CreateGameForm;