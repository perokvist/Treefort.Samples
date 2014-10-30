/** @jsx React.DOM */
var GameStore = require('../stores/GameStore.js');
var PlayForm = require('./Play.react');
var React = require('react');

var Game = React.createClass({
    render: function() {
        return (
          <div className="game">
            <h3>
              {this.props.Name} 
            </h3>
			<PlayForm GameId={this.props.GameId} />
          </div>
      );
    }
});


function getStateFromStores() {
    return {
        games: GameStore.getAll()
    };
}

var GameList = React.createClass({

    getInitialState: function() {
        return { games : []};
    },

    componentDidMount: function() {
        GameStore.addChangeListener(this._onChange);
	    //setInterval(this.getStateFromStores, this.props.pollInterval);
    },

    render: function () {
        console.log(this.state.games);
        var gameNodes = this.state.games.map(function(game, index) {
            return (
              <Game Name={game.Name} GameId={game.GameId} key={index} />
            );
});
return (
  <div className="gameList">
	<h2>Games</h2>
    {gameNodes}
  </div>
    );
},
/**
 * Event handler for 'change' events coming from the MessageStore
 */
_onChange: function() {
    this.setState(getStateFromStores());
}

});

module.exports = GameList;