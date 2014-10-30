/** @jsx React.DOM */
var GameStore = require('../stores/GameStore.js');
var React = require('react');

var Score = React.createClass({
    render: function() {
        return (
          <div className="game">
            <h2>
              {this.props.Name}
            </h2>
			<p>{this.props.Winner}</p>
          </div>
      );
    }
});


function getStateFromStores() {
    return {
        score: GameStore.getScore()
    };
}

var ScoreList = React.createClass({

    getInitialState: function() {
        return { score : []};
    },

    componentDidMount: function() {
        GameStore.addChangeListener(this._onChange);
    },

    render: function () {
        var scoreNodes = this.state.score.map(function(game, index) {
            return (
              <Score Name={game.Name} Winner={game.Winner} key={index} />
            );
});
return (
  <div className="gameList">
    {scoreNodes}
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

module.exports = ScoreList;