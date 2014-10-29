/** @jsx React.DOM */
var GameStore = require('../stores/GameStore.js');
var React = require('react');

var Game = React.createClass({
    render: function() {
        return (
          <div className="game">
            <h2>
              {this.props.Name}
            </h2>
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
    },

    render: function () {
        console.log(this.state.games);
        var gameNodes = this.state.games.map(function(game, index) {
            return (
              <Game Name={game.Name} key={index} />
            );
});
return (
  <div className="gameList">
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