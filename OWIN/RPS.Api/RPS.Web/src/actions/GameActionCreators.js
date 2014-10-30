var AppDispatcher = require('../dispatcher/AppDispatcher');
var GameWebAPIUtils = require('../utils/ServerUtil.js');

module.exports = {
    createGame: function (gameName, playerName, move) {
       GameWebAPIUtils.createGame({ gameName : gameName, playerName : playerName, move : move });
    },
    makeMove: function (gameId, playerName, move) {
        GameWebAPIUtils.makeMove({ gameId: gameId, playerName: playerName, move: move });
    }
};