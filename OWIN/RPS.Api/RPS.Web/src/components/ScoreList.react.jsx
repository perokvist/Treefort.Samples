/** @jsx React.DOM */
var GameStore = require('../stores/GameStore.js');
var React = require('react');

var Score = React.createClass({
    render: function() {
        return (
				<tr>
					<td>
						{this.props.Name}
					</td>
					<td>
						{this.props.Winner}
					</td>
				</tr>
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
	<table className="table-striped">
		<tr>
			<th>Game</th>
			<th>Winner</th>
		</tr>
		{scoreNodes}
	</table>
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