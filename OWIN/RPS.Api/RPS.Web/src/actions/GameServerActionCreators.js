var AppDispatcher = require('../dispatcher/AppDispatcher');

module.exports = {
    receiveAvailable: function(games) {
        AppDispatcher.handleServerAction({
            type: "RECEIVE_AVAILABLE_GAMES",
            games: games
        });
    },
    gameCreated: function(game) {
        AppDispatcher.handleServerAction({
            type: "GAME_CREATED",
            game: game
        });   
    },
    gameEnded : function(game) {
        AppDispatcher.handleServerAction({
            type: "GAME_ENDED",
            game: game
        });
    }
};