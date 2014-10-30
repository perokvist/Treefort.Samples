/** @jsx React.DOM */
var React = require('react');
var GameActionCreators = require('../actions/GameActionCreators');

var PlayForm = React.createClass({

    componentDidMount: function() {
    },
	 handleSubmit: function(e) {
    e.preventDefault();
    var playerName = this.refs.playerName.getDOMNode().value.trim();
    var move = this.refs.move.getDOMNode().value.trim();

    if (!playerName || !move) {
      return;
    }
	GameActionCreators.makeMove(this.props.GameId, playerName, move);
    
    this.refs.playerName.getDOMNode().value = '';

    return;
  },
    render: function() {
    return (
	<div>
	<h5>Play</h5>
      <form className="game-form" onSubmit={this.handleSubmit}>
        <input type="text" placeholder="Your Name" ref="playerName" />
        <select id="move" ref="move">
			<option>Rock</option>
			<option>Paper</option>
			<option>Scissors</option>
		</select>
		<input type="submit" value="Post" />
      </form>
	</div>
    );
    }

});

module.exports = PlayForm;