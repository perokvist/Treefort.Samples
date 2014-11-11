var AppDispatcher = require('../dispatcher/AppDispatcher');
var GameWebAPIUtils = require('../utils/ServerUtil.js');

module.exports = {
    createGame: function (gameName, playerName, move) {
        var command = { gameName: gameName, playerName: playerName, move: move };
        var dispatch = function(game) {
            AppDispatcher.handleServerAction({
                type: "GAME_CREATED",
                game: game
            });
        };

        GameWebAPIUtils.createGame(command, dispatch);
    },
    makeMove: function (gameId, playerName, move) {
        var command = { gameId: gameId, playerName: playerName, move: move };
        var dispatch = function (game) {
            AppDispatcher.handleServerAction({
                type: "GAME_ENDED",
                game: game
            });
        };

        GameWebAPIUtils.makeMove(command, dispatch);
    }
};